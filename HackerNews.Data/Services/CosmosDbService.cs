﻿using HackerNews.Domain.Entities;
using HackerNews.Domain.Exceptions;
using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.CosmosDb;
using HackerNews.Domain.Models.HackerNews;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HackerNews.Data.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;
        public CosmosDbService(Container container)
        {
            _container = container;
        }

        public async Task AddHackerNewsItem(HackerNewsItemEntity item)
        {
            try
            {
                ItemResponse<HackerNewsItemEntity> response = await _container.UpsertItemAsync(item);

                Debug.WriteLine($"Item was successfully saved to cosmos. Id: {response.Resource.id}");
            }
            catch(CosmosException ex)
            {
                Debug.WriteLine($"Item could not be added or updated. StatusCode: {ex.StatusCode} Message: {ex.Message}");
            }
        }

        public async Task<IEnumerable<HackerNewsItemEntity>> GetHackerNewsItems(Expression<Func<HackerNewsItemEntity, bool>> filter = null)
        {
            var query = _container.GetItemLinqQueryable<HackerNewsItemEntity>()
                      .OrderByDescending(x => x.id)
                      .Where(x => !x.IsDeleted);

            if (filter != null)
            {
                query.Where(filter);
            }

            var itemsIterator = query.ToFeedIterator();

            List<HackerNewsItemEntity> items = new List<HackerNewsItemEntity>();

            while (itemsIterator.HasMoreResults)
            {
                foreach(var item in await itemsIterator.ReadNextAsync())
                {
                    items.Add(item);
                }
            }

            return items;
        }

    }
}
