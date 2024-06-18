using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Moq;
using Moq.Protected;
using Training.Core.Model;
using Training.Services;

public class ClientServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private readonly HttpClient _httpClient;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new System.Uri("http://localhost")
        };
        _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_httpClient);
        _clientService = new ClientService(_httpClientFactoryMock.Object);
    }

    private bool CheckJsonContent(HttpRequestMessage req, Client expectedClient)
    {
        if (req.Content == null) return false;
        var actualClient = JsonSerializer.Deserialize<Client>(req.Content.ReadAsStream(), new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        req.Content.ReadAsStream().Seek(0, SeekOrigin.Begin);
        return expectedClient.FirstName == actualClient?.FirstName;
    }

    [Fact]
    public async Task GetClientsAsync_ReturnsClients()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = 1, FirstName = "John", LastName = "Doe" },
            new Client { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(clients)
            });

        // Act
        var result = await _clientService.GetClientsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateClientAsync_SendsPostRequest()
    {
        // Arrange
        var client = new Client { Id = 3, FirstName = "New", LastName = "Client" };
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, client)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created
            });

        // Act
        await _clientService.CreateClientAsync(client);

        // Assert
        _handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, client)),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task DeleteClientAsync_SendsDeleteRequest()
    {
        // Arrange
        var clientId = 1;
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
        await _clientService.DeleteClientAsync(clientId);

        // Assert
        _handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
            ItExpr.IsAny<CancellationToken>());
    }
}
