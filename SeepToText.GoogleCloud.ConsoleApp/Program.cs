using Google.Cloud.Speech.V2;
using Google.Protobuf;
using Google.Apis.Auth.OAuth2;

namespace SeepToText.GoogleCloud.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string jsonKeyPath = "/home/azam-amonov/repos/tarteeb-speech-to-text-08c335ff75f4.json";
        string projectId = "tarteeb-speech-to-text";
        
        string audioFilePath = Path.GetFullPath("/home/azam-amonov/repos/SpeechToText/SeepToText.GoogleCloud.ConsoleApp/wwwroot/salom-google.wav");
        
        Console.WriteLine($"Looking for audio at: {Path.GetFullPath(audioFilePath)}");
        
        Console.WriteLine("Google is ready to hearing!!!");
        Console.WriteLine("Press any key to continue...");
        Console.WriteLine($"Project ID: {projectId}");
        Console.WriteLine($"Key path: {jsonKeyPath}");
        
        if (!File.Exists(jsonKeyPath))
        {
            Console.WriteLine("ОШИБКА: Не могу найти файл ключа json! Проверь путь.");
            return;
        }
        var credential = GoogleCredential.FromFile(jsonKeyPath);
        var clientBuilder = new SpeechClientBuilder
        {
            GoogleCredential = credential
        };
        var client = clientBuilder.Build();
        
        string location = "global";
        string parent = $"projects/{projectId}/locations/{location}";
        string recognizerId = "uzbek-recognizer-1";
        string recognizerName = $"{parent}/recognizers/{recognizerId}";
        
        Console.WriteLine("Initializing recognizers...");
        Recognizer recognizer;

        try
        {
            recognizer = client.GetRecognizer(recognizerName);
            Console.WriteLine("Recognizer is ready to use!");
        }
        catch (Grpc.Core.RpcException e) when (e.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            Console.WriteLine("Recognizer not found!");
            Console.WriteLine("Creating new recognizer for 'uz-Uz'....");


            var request = new CreateRecognizerRequest
            {
                Parent = parent,
                RecognizerId = recognizerId,
                Recognizer = new Recognizer
                {
                    DefaultRecognitionConfig = new RecognitionConfig
                    {
                        LanguageCodes = { "uz-UZ" },
                        Model = "long"
                    }
                }
            };

            var operation = client.CreateRecognizer(request);
            recognizer = operation.PollUntilCompleted().Result;
            Console.WriteLine("Recognizer is ready to use!");
        }
        catch (Grpc.Core.RpcException e)
        {
            Console.WriteLine($"ERROR: GRPC!!!");
            Console.WriteLine($"ERROR: {e.Message}");
            return;
        }

        if (!File.Exists(audioFilePath))
        {
            Console.WriteLine($"ERROR: {audioFilePath} is not found!");
            return;
        }
        
        Console.WriteLine("Audio file is reading....");
        byte[] audioBytes = File.ReadAllBytes(audioFilePath);
        var recognizeRequest = new RecognizeRequest
        {
            Recognizer = recognizer.Name,
            Content = ByteString.CopyFrom(audioBytes),
            Config = new RecognitionConfig 
            {
            AutoDecodingConfig = new AutoDetectDecodingConfig(),// <--- Эта строчка решает ошибку
            LanguageCodes = { "uz-UZ" },
            Model = "long"
        }
        };

        try
        {
            var response = client.Recognize(recognizeRequest);
            Console.WriteLine("Speech recognized and ready to use!");

            foreach (var res in response.Results)
            {
                if (res.Alternatives.Count > 0)
                {
                    Console.WriteLine(res.Alternatives[0].Transcript);
                    
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR: {e.Message}");
        }
    }
}

// Speech recognized and ready to use!
// FINAL RESULT
// salom google men sen bilan o'zbek tilida gaplashmoqdaman meni gaplarimni text formatiga oʻtkaz























