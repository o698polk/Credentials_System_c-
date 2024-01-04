using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using OpenAI_API;
using NuGet.Protocol.Plugins;
using System.Diagnostics.Metrics;
using carnetutelvt.Models;
using Microsoft.EntityFrameworkCore;

namespace carnetutelvt.Controllers
{
    [Route("chatbot")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        private readonly IHttpContextAccessor _conter;
        public OpenAIController( IHttpContextAccessor conter)
        {
            
            _conter = conter;
           

        }

        [HttpPost]
        [Route("cahtutlvt")]
        public IActionResult ChatResponse([FromBody] string prompt)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0)
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                string apikey = "";
                string respuetsa = string.Empty;
                var chatbotIA = new OpenAIAPI(apikey);
                var completion = new CompletionRequest();
                completion.Prompt = prompt;
                completion.Model = OpenAI_API.Models.Model.DavinciText;
                completion.MaxTokens = 100;
                var result = chatbotIA.Completions.CreateCompletionAsync(completion);
                if (result != null)
                {
                    foreach (var item in result.Result.Completions)
                    {
                        respuetsa = item.Text;
                    }
                    return Ok(respuetsa);
                }
                else
                {
                    return BadRequest("Not Found");
                }
            }
        }
    }
}
