using HackerNews.Domain.Enums;
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
    public class HackerNewsService : INewsService
    {
        private readonly HttpClient _httpClient;
        private readonly HackerNewsConfig _hackerNewsConfig;
        private const string MaxItemPath = "maxitem.json";
        private const string ItemPath = "item/{0}.json"; // where {0} is replaced with the item id

        public HackerNewsService(HttpClient httpClient, IOptions<HackerNewsConfig> _configOptions)
        {
            _httpClient = httpClient;
            _hackerNewsConfig = _configOptions.Value;
            _httpClient.BaseAddress = new Uri(_hackerNewsConfig.BaseUrl);
        }

        private async Task<int> GetMaxItemId()
        {
            var response = await _httpClient.GetAsync(MaxItemPath);

            if (!response.IsSuccessStatusCode)
            {
                return 1; // change to return current cached max item count
            }

            var maxItemString = await response.Content.ReadAsStringAsync();

            if (!int.TryParse(maxItemString, out int maxItemId))
            {
                return 1; // change to return current cached max item count
            }

            return maxItemId;
        }

        public async Task<IEnumerable<HackerNewsItem>> GetStories(int startId, int count)
        {
            count = count == default ? _hackerNewsConfig.DefaultArticleCount : count;
            count += 50;
            startId = startId == default ? await GetMaxItemId() : startId;

            var tasks = new List<Task<HackerNewsItem>>();

            for(int id = startId; id >= startId - count; id--)
            {
                var task = GetHackerNewsItem(id);

                tasks.Add(task);
            }

            var articles = await Task.WhenAll(tasks);

            return articles.Where(x => x != null && x.Type == NewsType.story.ToString() && x.IsDeleted == false).Take(count);
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

            return JsonSerializer.Deserialize<HackerNewsItem>(articleString);
        }
    }
}
