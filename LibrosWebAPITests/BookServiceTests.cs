using BooksWebAPI.Commands;
using BooksWebAPI.Handlers;
using BooksWebAPI.Models;
using BooksWebAPI.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;
using static BooksWebAPI.Handlers.UpdateBookHandler;

namespace BookWebApi.Test
{
    public class BookServiceTests : IClassFixture<TestConnection>
    {
        private readonly TestConnection _connection;

        public BookServiceTests(TestConnection connection)
        {
            _connection = connection;
        }

        [Fact]
        public async Task Handle_CreatesBook()
        {
            System.Diagnostics.Debugger.Launch();
            // Arrange
            var newBook = new Book
            {
                Id = 1073,
                Title = "Test",
                Description = "Test Descripcion",
                PageCount = 5555,
                Excerpt = "Test Excerpt",
                PublishDate = DateTime.Now
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonSerializer.Serialize(newBook), System.Text.Encoding.UTF8, "application/json")
            };

            _connection.MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var handler = new CreateBookHandler(_connection.HttpClientFactory.Object);
            var command = new CreateBook(newBook);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [Fact]
        public async Task Handle_UpdateBookTest()
        {
            System.Diagnostics.Debugger.Launch();
            int bookId = 2;
            var newBook = new Book
            {
                Id = bookId,
                Title = "Test",
                Description = "Test Descripcion",
                PageCount = 5555,
                Excerpt = "Test Excerpt",
                PublishDate = DateTime.Now
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonSerializer.Serialize(newBook), System.Text.Encoding.UTF8, "application/json")
            };

            _connection.MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var handler = new UpdateBookHandler(_connection.HttpClientFactory.Object);
            var command = new UpdateBook(bookId, newBook);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);

        }

        [Fact]
        public async Task Handle_DeleteBookTest()
        {
            System.Diagnostics.Debugger.Launch();
            int bookId = 2;

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NoContent)
            {
                Content = new StringContent(JsonSerializer.Serialize(bookId), System.Text.Encoding.UTF8, "application/json")
            };

            _connection.MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var handler = new DeleteBookHandler(_connection.HttpClientFactory.Object);
            var command = new DeleteBook(bookId);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);

        }

        [Fact]
        public async Task Handle_GetAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book {
                    Id = 1,
                    Title = "Test",
                    Description = "Test Descripcion",
                    PageCount = 5555,
                    Excerpt = "Test Excerpt",
                    PublishDate = DateTime.Now 
                },
                new Book { 
                    Id = 2,
                    Title = "Test",
                    Description = "Test Descripcion",
                    PageCount = 5555,
                    Excerpt = "Test Excerpt",
                    PublishDate = DateTime.Now
                }
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(books), System.Text.Encoding.UTF8, "application/json")
            };

            _connection.MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var handler = new GetAllBooksHandler(_connection.HttpClientFactory.Object);
            var query = new GetAllBooks();

            System.Diagnostics.Debugger.Launch();
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);

        }


        [Fact]
        public async Task Handle_GetBookById()
        {
            // Arrange
            var book = 
                new Book { 
                    Id = 2,
                    Title = "Book 2",
                    Description = "Lorem lorem lorem. Lorem lorem lorem. Lorem lorem lorem.",
                    PageCount = 200,
                    Excerpt = "Test Excerpt",
                    PublishDate = DateTime.Now
                };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(book), System.Text.Encoding.UTF8, "application/json")
            };

            _connection.MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var handler = new GetBookByIdHandler(_connection.HttpClientFactory.Object);
            var query = new GetBookById() { Id = 2 };

            System.Diagnostics.Debugger.Launch();
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);

        }
    }

    public class TestConnection
    {
        private readonly IConfiguration _configuration;
        public Mock<HttpMessageHandler> MockHttpMessageHandler { get; }
        public Mock<IHttpClientFactory> HttpClientFactory { get; }

        public TestConnection()
        {

            MockHttpMessageHandler = new Mock<HttpMessageHandler>();
            HttpClientFactory = new Mock<IHttpClientFactory>();

            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = configurationBuilder.Build();

            // Obtener la BaseURL del appsettings.json
            var apiBaseUrl = _configuration.GetSection("ExternalApi:BaseUrl").Value;

            HttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient(MockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(apiBaseUrl)
            });

        }
    }

}
