using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

namespace WordSearcher.Controllers
{
    [Route("api/[controller]")]
    public class WordSearcherController : Controller
    {
        private readonly ILogger<WordSearcherController> logger;

        public WordSearcherController(ILogger<WordSearcherController> l)
        {
            logger = l;
        }

        [HttpGet]
        public string Get()
        {
            return "nothing";
        }

        [HttpPost]
        public CreateGridResponse Post([FromBody]CreateGridRequest data)
        {
            var ws = new WordSearch(data, logger);
            ws.MaxPlacementTries = 100;

            var response = new CreateGridResponse
            {
                Grid = ws.BuildGrid()
            };

            return response;
        }
    }
}