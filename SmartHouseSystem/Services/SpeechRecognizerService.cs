using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Core;
using SmartHouseSystem.SpeechRecognition;

/// <summary>
/// Handel exiting from listning
/// </summary>
namespace SmartHouseSystem.Services
{
    public class SpeechRecognizerService : ISpeechRecognizerService
    {
        private string cmd;

        public string Cmd { get => cmd; set { cmd = value; CmdNotifyPropertyChanged(nameof(cmd)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        internal void CmdNotifyPropertyChanged(String propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public SpeechRecognizer speechRecognizer{ get; set; }

        private ResourceContext speechContext;
        private ResourceMap speechResourceMap;
        private static uint HResultRecognizerNotFound = 0x8004503a;
        private ISignalRService _signalRService;

        public async Task SpeechRecognizerStartAsync(ISignalRService signalRService)
        {
            _signalRService = signalRService;
            bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
            if (permissionGained)
            {
                Language speechLanguage = SpeechRecognizer.SystemSpeechLanguage;
                string langTag = speechLanguage.LanguageTag;
                speechContext = ResourceContext.GetForCurrentView();
                speechContext.Languages = new string[] { langTag };

                speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationSpeechResources");

                await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
            }
            else
            {
                Debug.WriteLine("Something went wrong");
            }
        }

        private async Task InitializeRecognizer(Language recognizerLanguage)
        {
            if (speechRecognizer != null)
            {
                // cleanup prior to re-initializing this scenario.
                speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
                speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;
                speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;

                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }

            try
            {
                string languageTag = recognizerLanguage.LanguageTag;
                string fileName = String.Format("SRGS\\grammar.xml");
                StorageFile grammarContentFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);
                
                speechRecognizer = new SpeechRecognizer(recognizerLanguage);
                speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;

                SpeechRecognitionGrammarFileConstraint grammarConstraint = new SpeechRecognitionGrammarFileConstraint(grammarContentFile);
                speechRecognizer.Constraints.Add(grammarConstraint);
                SpeechRecognitionCompilationResult compilationResult = await speechRecognizer.CompileConstraintsAsync();

                if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
                {
                    // Let the user know that the grammar didn't compile properly.
                    Debug.WriteLine("Unable to compile grammar.");
                }
                else
                {
                    // Set EndSilenceTimeout to give users more time to complete speaking a phrase.
                    speechRecognizer.Timeouts.EndSilenceTimeout = TimeSpan.FromSeconds(1.2);

                    // Handle continuous recognition events. Completed fires when various error states occur. ResultGenerated fires when
                    // some recognized phrases occur, or the garbage rule is hit.
                    speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
                    speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
                }
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == HResultRecognizerNotFound)
                {
                    Debug.WriteLine("Speech Language pack for selected language not installed.");
                }
                else
                {
                    Debug.WriteLine(ex.Message, "Exception");
                }
            }
        }

        private void SpeechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            Debug.WriteLine("StateChanged");
        }

        private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if (args.Result.Confidence == SpeechRecognitionConfidence.Medium ||
                args.Result.Confidence == SpeechRecognitionConfidence.High)
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    HandleRecognitionResultAsync(args.Result);
                });
            }
            else if (args.Result.Confidence == SpeechRecognitionConfidence.Rejected ||
                     args.Result.Confidence == SpeechRecognitionConfidence.Low)
            {
                // In some scenarios, a developer may choose to ignore giving the user feedback in this case, if speech
                // is not the primary input mechanism for the application.
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Debug.WriteLine("try again");
                    });
            }
        }

        private void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            Debug.WriteLine("Continuous Recognition completed");
        }

        private async Task HandleRecognitionResultAsync(SpeechRecognitionResult recoResult)
        {
            // Check the confidence level of the recognition result.
            if (recoResult.Confidence == SpeechRecognitionConfidence.High ||
            recoResult.Confidence == SpeechRecognitionConfidence.Medium)
            {
                // Declare a string that will contain messages when the color rule matches GARBAGE.
                var garbagePrompt = "";

                // BACKGROUND: Check to see if the recognition result contains the semantic key for the background color,
                // and not a match for the GARBAGE rule, and change the color.
                if (recoResult.SemanticInterpretation.Properties.ContainsKey("lightID"))
                {
                    var lightID = recoResult.SemanticInterpretation.Properties["lightID"][0].ToString();
                    var command = recoResult.SemanticInterpretation.Properties["action"][0].ToString();
                    Debug.WriteLine(lightID);
                    Debug.WriteLine(command);
                    if (lightID == "ONE" && command == "ON")
                    {
                        await _signalRService.InvokeTurnOnLight(true, 124);
                        Cmd = "On1";
                    }
                    else if (lightID == "ONE" && command == "OFF")
                    {
                        await _signalRService.InvokeTurnOnLight(false, 124);
                        Cmd = "On2";
                    }
                    if (lightID == "THREE" && command == "ON")
                    {
                        await _signalRService.InvokeTurnOnLight(true, 125);
                        Cmd = "On3";
                    }
                    else if (lightID == "THREE" && command == "OFF")
                    {
                        await _signalRService.InvokeTurnOnLight(false, 125);
                        Cmd = "On4";
                    }
                }
                else 
                {
                    Debug.WriteLine("Something went wrong in handle recognition resoult");
                }
                
            }

            // Prompt the user if recognition failed or recognition confidence is low.
            else if (recoResult.Confidence == SpeechRecognitionConfidence.Rejected ||
            recoResult.Confidence == SpeechRecognitionConfidence.Low)
            {
                Debug.WriteLine("Spech rejected");
            }
        }
    }
}
