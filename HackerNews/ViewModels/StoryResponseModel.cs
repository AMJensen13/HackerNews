using HackerNews.Domain.Models.HackerNews;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HackerNews.ViewModels
{
    public class StoryResponseModel
    {
        [JsonPropertyName("stories")]
        public IEnumerable<HackerNewsItem> Stories { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
