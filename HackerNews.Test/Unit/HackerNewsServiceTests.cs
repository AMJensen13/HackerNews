using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using HackerNews.Data.Services;
using HackerNews.Domain.Entities;
using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.HackerNews;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HackerNews.Tests.Unit
{
    [TestClass]
    public class HackerNewsServiceTests
    {
        public HackerNewsItem BasicStory => new HackerNewsItem
        {
            Id = RandomNumberGenerator.GetInt32(100000)
        };

        public static IEnumerable<object[]> GetStoriesData()
        {
            yield return new object[]
            {
                null,
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                },
                Enumerable.Empty<HackerNewsItemEntity>()
            };
            yield return new object[]
            {
                5,
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[ 12, 10 ]")
                },
                Enumerable.Empty<HackerNewsItemEntity>()
            };
            yield return new object[]
            {
                5,
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[ 12, 10 ]")
                },
                Enumerable.Empty<HackerNewsItemEntity>()
            };
            yield return new object[]
            {
                5,
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[ 12, 10 ]")
                },
                new List<HackerNewsItemEntity>
                {
                    new HackerNewsItemEntity
                    {
                        id = "123"
                    }
                }
            };
            yield return new object[]
            {
                5,
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[ 12, 10 ]")
                },
                new List<HackerNewsItemEntity>
                {
                    new HackerNewsItemEntity
                    {
                        id = "123"
                    }
                }
            };
        }

        private Task<HttpResponseMessage> GetResponseMessage(HttpRequestMessage message, HttpResponseMessage latestStoriesResponse)
        {
            if (message.RequestUri.ToString().Contains("newstories"))
            {
                return Task.FromResult(latestStoriesResponse);
            }

            var content = new StringContent(JsonSerializer.Serialize(BasicStory));

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            });
        }

        [DataTestMethod]
        [DynamicData("GetStoriesData", DynamicDataSourceType.Method)]
        public async Task CanGetStories(int? count, HttpResponseMessage latestStoriesResponse, IEnumerable<HackerNewsItemEntity> newsItems)
        {
            // Arrange 
            var config = new HackerNewsConfig
            {
                BaseUrl = "http://test.com",
                DefaultArticleCount = 10
            };
            var cosmosDbService = new Mock<ICosmosDbService>();
            cosmosDbService.Setup(x => x.GetHackerNewsItems(It.IsAny<Expression<Func<HackerNewsItemEntity, bool>>>())).ReturnsAsync(newsItems);
            cosmosDbService.Setup(x => x.AddHackerNewsItem(It.IsAny<HackerNewsItemEntity>()));

            var httpMessageHandler = new Mock<HttpMessageHandler>();

            httpMessageHandler.Protected()
                              .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                              .Returns((HttpRequestMessage y, CancellationToken z) => GetResponseMessage(y, latestStoriesResponse));

            var httpClient = new HttpClient(httpMessageHandler.Object);

            var service = new HackerNewsService(cosmosDbService.Object, httpClient, Options.Create(config));

            // Act
            var response = await service.GetStories(count);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count() <= count.GetValueOrDefault());
        }
    }
}
