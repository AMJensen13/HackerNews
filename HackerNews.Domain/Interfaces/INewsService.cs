using HackerNews.Domain.Models.HackerNews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNews.Domain.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<HackerNewsItem>> GetStories(int startId, int count);
    }
}
