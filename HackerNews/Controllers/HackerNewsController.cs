using HackerNews.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        public IActionResult Index()
        {

            return Ok();
        }
    }
}
