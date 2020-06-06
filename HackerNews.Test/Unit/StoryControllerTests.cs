using HackerNews.Controllers;
using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.HackerNews;
using HackerNews.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Tests.Unit
{
    [TestClass]
    public class StoryControllerTests
    {
        public static IEnumerable<object[]> GetStoriesData()
        {
            yield return new object[]
            {
                10,
                null
            };
            yield return new object[]
            {
                10,
                Enumerable.Empty<HackerNewsItem>()
            };
            yield return new object[]
            {
                10,
                new List<HackerNewsItem>
                {
                    new HackerNewsItem()
                }
            };
        }

        [DataTestMethod]
        [DynamicData("GetStoriesData", DynamicDataSourceType.Method)]
        public async Task CanGetStories(int count, IEnumerable<HackerNewsItem> items)
        {
            // Arrange
            var newsService = new Mock<IStoryService>();
            newsService.Setup(x => x.GetStories(It.IsAny<int>())).ReturnsAsync(items);

            var controller = new StoryController(newsService.Object);

            // Act
            var response = await controller.GetStories(count);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

            var responseObject = (response as ObjectResult).Value;

            Assert.IsInstanceOfType(responseObject, typeof(StoryResponseModel));

            Assert.IsNotNull((responseObject as StoryResponseModel).Stories);
            Assert.AreEqual((responseObject as StoryResponseModel).Stories.Count(), (responseObject as StoryResponseModel).Count);
        }
    }
}
