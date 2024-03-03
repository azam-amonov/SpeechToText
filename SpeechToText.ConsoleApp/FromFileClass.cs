using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechToText.ConsoleApp;

public class FromFileClass
{
    static string speechKey = "f1ab29f30ea24d0e85b60360b9f3cf5e";
    static string speechRegion = "eastus";
    async static Task FromStream(SpeechConfig speechConfig)
    {
        var audioPath = "/Users/azamamonov/RiderProjects/SpeechToText/SpeechToText.Api/wwwroot/welcome.wav";
        var reader = new BinaryReader(File.OpenRead(path: audioPath));
        
        using var audioConfigStream = AudioInputStream.CreatePushStream();
        using var audioConfig = AudioConfig.FromStreamInput(audioConfigStream);
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        byte[] readBytes;
        do
        {
            readBytes = reader.ReadBytes(1024);
            audioConfigStream.Write(readBytes, readBytes.Length);
        } while (readBytes.Length > 0);

        var result = await speechRecognizer.RecognizeOnceAsync();
        Console.WriteLine($"RECOGNIZED: Text={result.Text}");
    }

    static async Task Main2(string[] args)
    {
        var speechConfig = SpeechConfig.FromSubscription(
            subscriptionKey: speechKey, 
            region: speechKey);
        await FromStream(speechConfig);
    }
}