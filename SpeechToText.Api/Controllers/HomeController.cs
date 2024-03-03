using Microsoft.AspNetCore.Mvc;

namespace SpeechToText2.Controllers;

[ApiController]
[Route("api/[controller]")]

public class HomeController: ControllerBase
{
    [HttpGet]
    public  ActionResult<string> GetMessage() 
        => Ok("Hello Mario princess in another castle");
}