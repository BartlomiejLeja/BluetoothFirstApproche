using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;

namespace SmartHouseSystem.Services
{
    public interface ISpeechRecognizerService
    {
        Task SpeechRecognizerStartAsync(ISignalRService signalRService);
        SpeechRecognizer speechRecognizer { get; set; }
        string Cmd { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}