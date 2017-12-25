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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartHouseSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
            getFrame();
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
            string sourceURL = "http://192.168.1.107/image/jpeg.cgi";
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

        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            getFrame();
        }
    }
}
