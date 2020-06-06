using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.HackerNews;
using HackerNews.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Controllers
{
    public class StoryController : Controller
    {
        private readonly IStoryService _newsService;

        public StoryController(IStoryService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Returns a specified number of the latest stories.
        /// </summary>
        /// <param name="count">The number of stories to return.</param>
        /// <returns><see cref="StoryResponseModel"/> containing the specified number of stories.</returns>
        [HttpGet]
        [Route("/Story")]
        public async Task<IActionResult> GetStories([FromQuery]int count)
        {
            var viewModel = new StoryResponseModel();
            viewModel.Stories = (await _newsService.GetStories(count))?.ToList();

            if (viewModel.Stories == null || !viewModel.Stories.Any())
            {
                viewModel.Stories = Enumerable.Empty<HackerNewsItem>();
            }
            else
            {
                viewModel.Count = viewModel.Stories.Count();
            }

            return Ok(viewModel);
        }
    }
}
