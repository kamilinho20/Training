namespace Training.Services.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Moq.Protected;
    using Training.Core.Model;
    using Training.Services;
    using Xunit;

    public class GroupTrainingServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly GroupTrainingService _groupTrainingService;

        public GroupTrainingServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new System.Uri("http://localhost")
            };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _groupTrainingService = new GroupTrainingService(_httpClientFactoryMock.Object);
        }

        private bool CheckJsonContent(HttpRequestMessage req, GroupTraining expectedTraining)
        {
            if (req.Content == null) return false;
            var actualTraining = JsonSerializer.Deserialize<GroupTraining>(req.Content.ReadAsStream(), new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            req.Content.ReadAsStream().Seek(0, SeekOrigin.Begin);
            return expectedTraining.Name == actualTraining?.Name;
        }

        [Fact]
        public async Task GetGroupTrainingsAsync_ReturnsGroupTrainings()
        {
            // Arrange
            var groupTrainings = new List<GroupTraining>
        {
            new GroupTraining { Id = 1, Name = "Yoga", TrainerId = 1, Date = DateTime.Now },
            new GroupTraining { Id = 2, Name = "Pilates", TrainerId = 2, Date = DateTime.Now }
        };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(groupTrainings)
                });

            // Act
            var result = await _groupTrainingService.GetGroupTrainingsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateGroupTrainingAsync_SendsPostRequest()
        {
            // Arrange
            var groupTraining = new GroupTraining { Id = 3, Name = "Boxing", TrainerId = 1, Date = DateTime.Now };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, groupTraining)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            // Act
            await _groupTrainingService.CreateGroupTrainingAsync(groupTraining);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, groupTraining)),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteGroupTrainingAsync_SendsDeleteRequest()
        {
            // Arrange
            var trainingId = 1;
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            // Act
            await _groupTrainingService.DeleteGroupTrainingAsync(trainingId);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>());
        }
    }

}
