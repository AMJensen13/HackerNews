using HackerNews.Domain.Models.HackerNews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Interfaces
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<NewsArticle>> GetNews(int startId, int count);
    }
}
