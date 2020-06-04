using HackerNews.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index([FromQuery]int count)
        {
             var stories = (await _newsService.GetStories(count)).ToList();

            return Ok(new { stories, count = stories.Count()});
        }
    }
}
