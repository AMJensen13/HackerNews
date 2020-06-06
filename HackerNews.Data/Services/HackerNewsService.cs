using HackerNews.Domain.Entities;
using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.HackerNews;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HackerNews.Data.Services
{
    public class HackerNewsService : IStoryService
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly HttpClient _httpClient;
        private readonly HackerNewsConfig _hackerNewsConfig;
        private const string LatestStories = "newstories.json";
        private const string ItemPath = "item/{0}.json"; // where {0} is replaced with the item id

        public HackerNewsService(ICosmosDbService cosmosDbService, HttpClient httpClient, IOptions<HackerNewsConfig> _configOptions)
        {
            _cosmosDbService = cosmosDbService;
            _httpClient = httpClient;
            _hackerNewsConfig = _configOptions.Value;
            _httpClient.BaseAddress = new Uri(_hackerNewsConfig.BaseUrl);
        }

        private async Task<int[]> GetLatestStoryIds()
        {
            var response = await _httpClient.GetAsync(LatestStories);

            if (!response.IsSuccessStatusCode)
            {
                return Array.Empty<int>(); // change to return current cached max item count
            }

            var latestStories = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<int[]>(latestStories);
        }

        public async Task<IEnumerable<HackerNewsItem>> GetStories(int? count)
        {
            count ??= _hackerNewsConfig.DefaultArticleCount;
            var newStoryIds = (await GetLatestStoryIds()).Take(count.Value).ToArray();

            var newsItems = (await GetItemsFromCosmos(newStoryIds)).ToList();

            var tasks = new List<Task<HackerNewsItem>>();

            foreach(var id in newStoryIds.Where(x => newsItems.FirstOrDefault(y => y.Id == x) == null))
            {
                var task = GetHackerNewsItem(id);

                tasks.Add(task);
            }

            newsItems.AddRange(await Task.WhenAll(tasks));

            return newsItems.Where(x => x != null).OrderByDescending(x => x.Id);
        }

        private async Task<IEnumerable<HackerNewsItem>> GetItemsFromCosmos(int[] ids)
        {
            return (await _cosmosDbService.GetHackerNewsItems(x => ids.Contains(int.Parse(x.id))))
                .Select(x => new HackerNewsItem(x));
        }

        private async Task<HackerNewsItem> GetHackerNewsItem(int id)
        {
            var url = string.Format(ItemPath, id);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var articleString = await response.Content.ReadAsStringAsync();

            var newsItem = JsonSerializer.Deserialize<HackerNewsItem>(articleString);

            if (newsItem != null)
            {
                await _cosmosDbService.AddHackerNewsItem(new HackerNewsItemEntity(newsItem));
            }

            return newsItem;
        }
    }
}
