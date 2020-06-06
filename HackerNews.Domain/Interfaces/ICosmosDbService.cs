using HackerNews.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HackerNews.Domain.Interfaces
{
    public interface ICosmosDbService
    {
        Task AddHackerNewsItem(HackerNewsItemEntity item);
        Task<IEnumerable<HackerNewsItemEntity>> GetHackerNewsItems(Expression<Func<HackerNewsItemEntity, bool>> filter = null);
    }
}
