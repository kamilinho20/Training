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

    public class IndividualTrainingServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly IndividualTrainingService _individualTrainingService;

        public IndividualTrainingServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new System.Uri("http://localhost")
            };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _individualTrainingService = new IndividualTrainingService(_httpClientFactoryMock.Object);
        }

        private bool CheckJsonContent(HttpRequestMessage req, IndividualTraining expectedTraining)
        {
            if (req.Content == null) return false;
            var actualTraining = JsonSerializer.Deserialize<IndividualTraining>(req.Content.ReadAsStream(), new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            req.Content.ReadAsStream().Seek(0, SeekOrigin.Begin);
            return expectedTraining.Description == actualTraining?.Description;
        }

        [Fact]
        public async Task GetIndividualTrainingsAsync_ReturnsIndividualTrainings()
        {
            // Arrange
            var individualTrainings = new List<IndividualTraining>
        {
            new IndividualTraining { Id = 1, ClientId = 1, TrainerId = 1, Date = DateTime.Now, Description = "Training 1" },
            new IndividualTraining { Id = 2, ClientId = 2, TrainerId = 2, Date = DateTime.Now, Description = "Training 2" }
        };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(individualTrainings)
                });

            // Act
            var result = await _individualTrainingService.GetIndividualTrainingsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateIndividualTrainingAsync_SendsPostRequest()
        {
            // Arrange
            var individualTraining = new IndividualTraining { Id = 3, ClientId = 1, TrainerId = 1, Date = DateTime.Now, Description = "Training 3" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, individualTraining)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            // Act
            await _individualTrainingService.CreateIndividualTrainingAsync(individualTraining);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, individualTraining)),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteIndividualTrainingAsync_SendsDeleteRequest()
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
            await _individualTrainingService.DeleteIndividualTrainingAsync(trainingId);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>());
        }
    }

}
