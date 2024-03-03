using Microsoft.CognitiveServices.Speech;

namespace SpeechToText2.Services;

public interface ITextSpeechService
{
    Task<SpeechRecognitionResult> GetTextOfSpeechAsync(string pathOfSpeech);
    // Task<string> GetTextOfSpeechAsync(string pathOfSpeech);
}