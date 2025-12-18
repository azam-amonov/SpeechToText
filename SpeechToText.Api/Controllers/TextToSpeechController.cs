using Microsoft.AspNetCore.Mvc;
using SpeechToText2.Services;

namespace SpeechToText2.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TextToSpeechController(ITextSpeechService textToSpeechService) : ControllerBase
{
    private readonly ITextSpeechService textToSpeechService = textToSpeechService;

    [HttpGet]
    public async ValueTask<IActionResult> GetTextToSpeech(string speechPath)
    {
        var result = await this.textToSpeechService.GetTextOfSpeechAsync(speechPath);
        return Ok(result);
    }
}