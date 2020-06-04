using HackerNews.Domain.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HackerNews.Domain.Models.HackerNews
{
    public class HackerNewsItem
    {
        /// <summary>
        /// The item's unique identifier
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The story's score or the votes for a poll opt
        /// </summary>
        [JsonPropertyName("score")]
        public int Score { get; set; }

        /// <summary>
        /// Creation date of the item in unix time
        /// </summary>
        [JsonPropertyName("time")]
        public long CreatedDateUnix { get; set; }

        /// <summary>
        /// The title of the story, poll, or job. May contain HTML content.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The author of the item.
        /// </summary>
        [JsonPropertyName("by")]
        public string By { get; set; }

        /// <summary>
        /// The comment, story, or poll text. May contain HTML content.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// The URL of the story.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// A boolean value indicating if the item is dead.
        /// </summary>
        [JsonPropertyName("dead")]
        public bool IsDead { get; set; }

        /// <summary>
        /// A boolean value indicating if the item has been deleted.
        /// </summary>
        [JsonPropertyName("deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The type of the item.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// For stories or polls, the total comment count.
        /// </summary>
        [JsonPropertyName("descendants")]
        public int Descendants { get; set; }

        /// <summary>
        /// A poll option's associated poll identifier
        /// </summary>
        [JsonPropertyName("poll")]
        public int PollId { get; set; }

        /// <summary>
        /// A list containing a poll's related poll options in ranked display order
        /// </summary>
        [JsonPropertyName("parts")]
        public IEnumerable<int> PollOptsIds { get; set; }

        /// <summary>
        /// A comment's parentId. Either a story or another comment.
        /// </summary>
        [JsonPropertyName("parent")]
        public int ParentId { get; set; }

        /// <summary>
        /// The ids of the item's comments, in ranked display order
        /// </summary>
        [JsonPropertyName("kids")]
        public IEnumerable<int> KidsIds { get; set; }
    }
}
