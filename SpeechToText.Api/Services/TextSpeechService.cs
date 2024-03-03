using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.Wave;

namespace SpeechToText2.Services;

public class TextSpeechService: ITextSpeechService
{
    private string SpeechKey = "f1ab29f30ea24d0e85b60360b9f3cf5e";
    private string SpeechRegion = "eastus";
    private readonly IWebHostEnvironment webHostEnvironment;

    public TextSpeechService(IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task<SpeechRecognitionResult> GetTextOfSpeechAsync(string pathOfSpeech)
    {
        // if we want input audio data form microphone!!!
        // var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        
        // if we want input audio data form audio data!!!
        var audioConfig = AudioConfig.FromWavFileInput(fileName: pathOfSpeech);
        
        var speechConfig = SpeechConfig.FromSubscription( 
            subscriptionKey: this.SpeechKey, 
            region: this.SpeechRegion);

        var startTime = DateTime.Now;
        var audioDuration = GetAudioDuration(pathOfSpeech);
        
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