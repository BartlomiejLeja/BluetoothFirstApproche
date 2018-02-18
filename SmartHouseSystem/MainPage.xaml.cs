using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Media.Core;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using System.Diagnostics;
using SmartHouseSystem.Camera;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using SmartHouseSystem.Camera.PhotoTaker;
using SmartHouseSystem.Camera.FaceIdentifyer;
using Windows.Media.Streaming.Adaptive;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartHouseSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string currentVisualState;
        FaceDetectionFrameProcessor faceDetectionProcessor;
        CancellationTokenSource requestStopCancellationToken;
        CameraPreviewManager cameraPreviewManager;
        FacialDrawingHandler drawingHandler;
        volatile TaskCompletionSource<SoftwareBitmap> copiedVideoFrameComplete;
        PhotoTaker photoTaker;
        FaceIdentifyer faceRecogition;

        FaceServiceClient faceService;

        //Image<Gray, byte> result, TrainedFace = null;
        DispatcherTimer dispatcherTimer = null; //My dispatcher timer to update UI
        TimeSpan updatUITime = TimeSpan.FromMilliseconds(30); //I update UI every 60 milliseconds
        private StreamSocket _socket;
        DataReader dataReaderObject;
        private CancellationTokenSource ReadCancellationTokenSource;

        private RfcommDeviceService _service;

        public MainPage()
        {
            this.InitializeComponent();
            dispatcherTimer = new DispatcherTimer(); //Initilialize the dispatcher
            dispatcherTimer.Interval = updatUITime;
            dispatcherTimer.Tick += DispatcherTimer_Tick; //Update UI
            photoTaker = new PhotoTaker();
            faceService = new FaceServiceClient("181da876aac14e2a82157836b95e882f", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");
         faceRecogition = new FaceIdentifyer();
         //   training.TrainToIdnetifyFaces(faceService);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.dispatcherTimer.Start(); //Start dispatcher

        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            this.dispatcherTimer.Stop(); //Stop dispatcher
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            //   getFrame();
        }

        private async void btnSend_Click(object sender,
                                         RoutedEventArgs e)
        {
            //int dummy;

            //if (!int.TryParse(tbInput.Text, out dummy))
            //{
            //    tbError.Text = "Invalid input";
            //}

            //var noOfCharsSent = await Send(tbInput.Text);

            //if (noOfCharsSent != 0)
            //{
            //    tbError.Text = noOfCharsSent.ToString();
            //}
            Listen();
        }

        private async Task<uint> Send(string msg)
        {
            tbError.Text = string.Empty;

            try
            {
                var writer = new DataWriter(_socket.OutputStream);

                writer.WriteString(msg);

                // Launch an async task to 
                //complete the write operation
                var store = writer.StoreAsync().AsTask();

                return await store;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;

                return 0;
            }
        }

        private async void Listen()
        {
            ReadCancellationTokenSource = new CancellationTokenSource();
            if (_socket.InputStream != null)
            {
                dataReaderObject = new DataReader(_socket.InputStream);
                // keep reading the serial input
                while (true)
                {
                    await ReadAsync(ReadCancellationTokenSource.Token);
                }
            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            // Create a task object to wait for data on the serialPort.InputStream
            var loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);

            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                string recvdtxt = dataReaderObject.ReadString(bytesRead);
                tbError.Text = recvdtxt;
            }
        }

        private async void btnConnect_Click(object sender,
                                            RoutedEventArgs e)
        {
            //tbError.Text = string.Empty;

            //try
            //{
            //    var devices =
            //          await DeviceInformation.FindAllAsync(
            //            RfcommDeviceService.GetDeviceSelector(
            //              RfcommServiceId.SerialPort));

            //    var selector = BluetoothDevice.GetDeviceSelector();

            //    var device = devices.Single(x => x.Name == "SPP Dev");

            //    _service = await RfcommDeviceService.FromIdAsync(
            //                                            device.Id);

            //    _socket = new StreamSocket();

            //    await _socket.ConnectAsync(
            //          _service.ConnectionHostName,
            //          _service.ConnectionServiceName,
            //          SocketProtectionLevel.
            //          BluetoothEncryptionAllowNullAuthentication);
            //}
            //catch (Exception ex)
            //{
            //    tbError.Text = ex.Message;
            //}
            await ConnectAsync();
        }

        public async Task ConnectAsync()
        {
            tbError.Text = string.Empty;

            try
            {
                var devices =
                      await DeviceInformation.FindAllAsync(
                        RfcommDeviceService.GetDeviceSelector(
                          RfcommServiceId.SerialPort));

                var selector = BluetoothDevice.GetDeviceSelector();

                var device = devices.Single(x => x.Name == "SPP Dev");

                _service = await RfcommDeviceService.FromIdAsync(
                                                        device.Id);

                _socket = new StreamSocket();

                await _socket.ConnectAsync(
                      _service.ConnectionHostName,
                      _service.ConnectionServiceName,
                      SocketProtectionLevel.
                      BluetoothEncryptionAllowNullAuthentication);
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
            Listen();
        }


        private async void btnDisconnect_Click(object sender,
                                             RoutedEventArgs e)
        {
            tbError.Text = string.Empty;

            try
            {
                await _socket.CancelIOAsync();
                _socket.Dispose();
                _socket = null;
                _service.Dispose();
                _service = null;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
        }

        public async void getFrame()
        {
            string sourceURL = "http://192.168.1.100/image/jpeg.cgi";
            //var streamUri = new Uri(sourceURL); //replace your URL  
            //var streamResponse = await AdaptiveMediaSource.CreateFromUriAsync(streamUri);
           
            //if (streamResponse.Status == AdaptiveMediaSourceCreationStatus.Success)
            //    liveMedia.SetMediaStreamSource(streamResponse.MediaSource);
            //else
            //{
            //    Debug.WriteLine(streamResponse.ExtendedError.ToString());
                
            //}

        
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sourceURL);
            req.Credentials = new NetworkCredential("admin", "Sim13vetson");
            WebResponse resp = await req.GetResponseAsync();

            Stream stream = resp.GetResponseStream();

            var memStream = new MemoryStream();

            // Convert the stream to the memory stream, because a memory stream supports seeking.
            await stream.CopyToAsync(memStream);

            // Set the start position.
            memStream.Position = 0;

            // Create a new bitmap image.
            var bitmap = new BitmapImage();

            // Set the bitmap source to the stream, which is converted to a IRandomAccessStream.
            bitmap.SetSource(memStream.AsRandomAccessStream());

           
            videoImage.Source = bitmap;

            var faceAttribute = new List<FaceAttributeType>();
            faceAttribute.Add(FaceAttributeType.Age);
            faceAttribute.Add(FaceAttributeType.Gender);

            try
            {
                Face[] faces = await faceService.DetectAsync(
            stream,true,true, faceAttribute);

              var faceRecognitionInfo= await faceRecogition.RecognizeFaces(faceService, faces);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            getFrame();
        }


        async void OnStart(object sender, RoutedEventArgs args)
        {
          
            this.CurrentVisualState = "Playing";
            this.requestStopCancellationToken = new CancellationTokenSource();

            this.cameraPreviewManager = new CameraPreviewManager(this.cePreview);
        

            var videoProperties =
              await this.cameraPreviewManager.StartPreviewToCaptureElementAsync(
                vcd => vcd.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);

            this.faceDetectionProcessor = new FaceDetectionFrameProcessor(
              this.cameraPreviewManager.MediaCapture,
              this.cameraPreviewManager.VideoProperties);

            this.drawingHandler = new FacialDrawingHandler(
           this.drawCanvas, videoProperties, Colors.White);

            this.faceDetectionProcessor.FrameProcessed += (s, e) =>
            {
           //     Debug.WriteLine($"Saw {e.Results.Count} faces");
                this.drawingHandler.SetLatestFrameReceived(e.Results);
                this.CopyBitmapForOxfordIfRequestPending(e.Frame.SoftwareBitmap);
            };

            try
            {
                await this.faceDetectionProcessor.RunFrameProcessingLoopAsync(
                  this.requestStopCancellationToken.Token);
            }
            catch (OperationCanceledException)
            {
            }
            await this.cameraPreviewManager.StopPreviewAsync();

            this.requestStopCancellationToken.Dispose();
            this.CurrentVisualState = "Stopped";
        }
        void OnStop(object sender, RoutedEventArgs e)
        {
            this.requestStopCancellationToken.Cancel();
        }
        string CurrentVisualState
        {
            get
            {
                return (this.currentVisualState);
            }
            set
            {
                if (this.currentVisualState != value)
                {
                    this.currentVisualState = value;
                    this.ChangeStateAsync();
                }
            }
        }
        async Task ChangeStateAsync()
        {
            await Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal,
              () =>
              {
                  VisualStateManager.GoToState(this, this.currentVisualState, false);
              }
            );
        }

        void CopyBitmapForOxfordIfRequestPending(SoftwareBitmap bitmap)
        {
            if ((this.copiedVideoFrameComplete != null) &&
             (!this.copiedVideoFrameComplete.Task.IsCompleted))
            {
                // We move to RGBA16 because that is a format that we will then be able
                // to use a BitmapEncoder on to move it to PNG and we *cannot* do async
                // work here because it'll break our processing loop.
                var convertedRgba16Bitmap = SoftwareBitmap.Convert(bitmap,
                  BitmapPixelFormat.Rgba16);

                this.copiedVideoFrameComplete.SetResult(convertedRgba16Bitmap);
            }
        }

        async void OnSubmitToOxfordAsync(object sender, RoutedEventArgs e)
        {
            // Because I constantly change visual states in the processing loop, I'm just doing
            // this with some code rather than with visual state changes because those would
            // get quickly overwritten while this work is ongoing.
            // this.progressIndicator.Visibility = Visibility.Visible;

            // We create this task completion source which flags our main loop
            // to create a copy of the next frame that comes through and then
            // we pick that up here when it's done...
            FaceRecognitonInfo.Text = "";
            this.copiedVideoFrameComplete = new TaskCompletionSource<SoftwareBitmap>();

            var bgra16CopiedFrame = await this.copiedVideoFrameComplete.Task;

            this.copiedVideoFrameComplete = null;

            InMemoryRandomAccessStream destStream = new InMemoryRandomAccessStream();

            // Now going to JPEG because Project Oxford can accept those.
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId,
              destStream);

            encoder.SetSoftwareBitmap(bgra16CopiedFrame);

            await encoder.FlushAsync();

            var faceAttribute = new List<FaceAttributeType>();
            faceAttribute.Add(FaceAttributeType.Age);
            faceAttribute.Add(FaceAttributeType.Gender);
            faceAttribute.Add(FaceAttributeType.Emotion);
            faceAttribute.Add(FaceAttributeType.Smile);

            Face[] faces = await faceService.DetectAsync(
              destStream.AsStream(),true,true, faceAttribute);

            var faceRecognitionInfo = await faceRecogition.RecognizeFaces(faceService, faces);


            // We now get a bunch of face data for each face but I'm ignoring most of it
            // (like facial landmarks etc) and simply displaying the guess of the age
            // and the gender for the moment.
            if (faces != null)
            {
                
              Debug.WriteLine($"Saw {faces[0].FaceAttributes.Age.ToString()}");
                //  txtAge.Text = faces[0].Attributes.Age.ToString();
               Debug.WriteLine($"Saw { faces[0].FaceAttributes.Gender.ToString()}");
                // txtGender.Text = faces[0].Attributes.Gender.ToString();
                FaceRecognitonInfo.Text = $"Saw { faces[0].FaceAttributes.Gender.ToString()} age {faces[0].FaceAttributes.Age.ToString()} " +
                    $"name of this { faces[0].FaceAttributes.Gender.ToString()} is {faceRecognitionInfo} and this { faces[0].FaceAttributes.Gender.ToString()} " +
                $"is {faces[0].FaceAttributes.Smile.ToString()} ";

            }
            else
            {
                //txtAge.Text = "no age";
              //  txtGender.Text = "no gender";
            }
         //   this.progressIndicator.Visibility = Visibility.Collapsed;
        }

       async void Button_Click(object sender, RoutedEventArgs e)
        {
           photoTaker.TakePhoto();
        }

        private void Train_Click(object sender, RoutedEventArgs e)
        {
            faceRecogition.TrainToIdnetifyFaces(faceService);
        }
    }
}



