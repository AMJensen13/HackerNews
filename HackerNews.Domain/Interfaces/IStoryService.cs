using HackerNews.Domain.Models.HackerNews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNews.Domain.Interfaces
{
    public interface IStoryService
    {
        Task<IEnumerable<HackerNewsItem>> GetStories(int? count);
    }
}
