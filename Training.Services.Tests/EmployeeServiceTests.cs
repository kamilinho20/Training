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

    public class EmployeeServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new System.Uri("http://localhost")
            };
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _employeeService = new EmployeeService(_httpClientFactoryMock.Object);
        }

        private bool CheckJsonContent(HttpRequestMessage req, Employee expectedEmployee)
        {
            if (req.Content == null) return false;
            var actualEmployee = JsonSerializer.Deserialize<Employee>(req.Content.ReadAsStream(), new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            req.Content.ReadAsStream().Seek(0, SeekOrigin.Begin);
            return expectedEmployee.FirstName == actualEmployee?.FirstName;
        }

        [Fact]
        public async Task GetEmployeesAsync_ReturnsEmployees()
        {
            // Arrange
            var employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "John", LastName = "Smith", Role = "Trainer" },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", Role = "Manager" }
        };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(employees)
                });

            // Act
            var result = await _employeeService.GetEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateEmployeeAsync_SendsPostRequest()
        {
            // Arrange
            var employee = new Employee { Id = 3, FirstName = "New", LastName = "Employee", Role = "Assistant" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, employee)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            // Act
            await _employeeService.CreateEmployeeAsync(employee);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && CheckJsonContent(req, employee)),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteEmployeeAsync_SendsDeleteRequest()
        {
            // Arrange
            var employeeId = 1;
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
            await _employeeService.DeleteEmployeeAsync(employeeId);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>());
        }
    }

}
