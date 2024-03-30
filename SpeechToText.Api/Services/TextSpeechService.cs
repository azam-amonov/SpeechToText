using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.Wave;

namespace SpeechToText2.Services;

public class TextSpeechService: ITextSpeechService
{
    private string SpeechKey = "YOUR_SPEECH_KEY_HERE";
    private string SpeechRegion = "eastus";
    private readonly IWebHostEnvironment webHostEnvironment;

    public TextSpeechService(IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
    }

    // public async Task<string> GetTextOfSpeechAsync(string pathOfSpeech)
    public async Task<SpeechRecognitionResult> GetTextOfSpeechAsync(string pathOfSpeech)
    {
        // if we want input audio data form microphone!!!
        // var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        
        // if we want input audio data form audio data!!!
        pathOfSpeech = "/Users/azamamonov/RiderProjects/SpeechToText/SpeechToText.Api/wwwroot/welcome.wav";
        var audioConfig = AudioConfig.FromWavFileInput(fileName: pathOfSpeech);
        
        var speechConfig = SpeechConfig.FromSubscription( 
            subscriptionKey: this.SpeechKey, 
            region: this.SpeechRegion);

        using (var speechRecognizer = new SpeechRecognizer(audioConfig: audioConfig, speechConfig: speechConfig))
        {
            return await speechRecognizer.RecognizeOnceAsync();
        }

    }

    private static double GetAudioDuration(string pathOfSpeech)
    {
        using (var audioFileReader = new AudioFileReader(pathOfSpeech))
        {
            var durationInSeconds = audioFileReader.TotalTime.TotalSeconds;
            return durationInSeconds;
        }
    }
}
