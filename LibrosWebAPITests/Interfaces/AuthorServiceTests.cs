using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibrosWebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrosWebAPI.Models;
using System.ComponentModel.Design;
using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;

namespace LibrosWebAPI.Interfaces.Tests
{
    [TestClass()]
    public class AuthorServiceTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private AuthorService _authorService;
        private JsonSerializerOptions _jsonOptions;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;

        [TestInitialize]
        public void Setup()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Configurar un HttpClient simulado
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                                .Returns(_httpClient);

            _authorService = new AuthorService(_mockHttpClientFactory.Object);
        }

        [TestMethod()]
        public async Task GetAllAuthorsAsyncTest_ReturnsListOfAuthors()
        {
            // Arrange
            var expectedAuthors = new List<Author>
        {
            new Author { Id = 1, IdBook = 101, FirstName = "John", LastName = "Doe" },
            new Author { Id = 2, IdBook = 102, FirstName = "Jane", LastName = "Smith" }
        };
            var jsonResponse = JsonSerializer.Serialize(expectedAuthors, _jsonOptions);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var actualAuthors = await _authorService.GetAllAuthorsAsync();

            // Assert
            Assert.IsNotNull(actualAuthors);
            Assert.AreEqual(expectedAuthors.Count, actualAuthors.Count());
            Assert.IsTrue(actualAuthors.Any(a => a.FirstName == "John"));
            Assert.IsTrue(actualAuthors.Any(a => a.LastName == "Smith"));
        }

        [TestMethod()]
        public async Task GetAllAuthorsAsyncTest_ReturnsEmptyListOnError()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ThrowsAsync(new HttpRequestException());

            // Act
            var actualAuthors = await _authorService.GetAllAuthorsAsync();

            // Assert
            Assert.IsNotNull(actualAuthors);
            Assert.AreEqual(0, actualAuthors.Count());
        }

        [TestMethod()]
        public async Task GetAuthorByIdAsyncTest_ReturnsAuthorIfExists()
        {
            // Arrange
            int authorId = 1;
            var expectedAuthor = new Author { Id = authorId, IdBook = 201, FirstName = "Peter", LastName = "Jones" };
            var jsonResponse = JsonSerializer.Serialize(expectedAuthor, _jsonOptions);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains($"/{authorId}")),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var actualAuthor = await _authorService.GetAuthorByIdAsync(authorId);

            // Assert
            Assert.IsNotNull(actualAuthor);
            Assert.AreEqual(expectedAuthor.Id, actualAuthor.Id);
            Assert.AreEqual(expectedAuthor.FirstName, actualAuthor.FirstName);
        }

        [TestMethod()]
        public async Task GetAuthorByIdAsyncTest_ReturnsNullIfNotExists()
        {
            // Arrange
            int authorId = 1;
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains($"/{authorId}")),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var actualAuthor = await _authorService.GetAuthorByIdAsync(authorId);

            // Assert
            Assert.IsNull(actualAuthor);
        }

        [TestMethod()]
        public async Task GetAuthorsByBookIdAsyncTest_ReturnsAuthorsForGivenBookId()
        {
            // Arrange
            int bookId = 101;
            var allAuthors = new List<Author>
        {
            new Author { Id = 1, IdBook = bookId, FirstName = "John", LastName = "Doe" },
            new Author { Id = 2, IdBook = 102, FirstName = "Jane", LastName = "Smith" },
            new Author { Id = 3, IdBook = bookId, FirstName = "Peter", LastName = "Jones" }
        };
            var jsonResponse = JsonSerializer.Serialize(allAuthors, _jsonOptions);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var actualAuthors = await _authorService.GetAuthorsByBookIdAsync(bookId);

            // Assert
            Assert.IsNotNull(actualAuthors);
            Assert.AreEqual(2, actualAuthors.Count());
            Assert.IsTrue(actualAuthors.All(a => a.IdBook == bookId));
        }

        [TestMethod()]
        public async Task AddAuthorAsyncTest_ReturnsAddedAuthor()
        {
            // Arrange
            var authorToAdd = new Author { IdBook = 301, FirstName = "Alice", LastName = "Brown" };
            var addedAuthor = new Author { Id = 3, IdBook = 301, FirstName = "Alice", LastName = "Brown" };
            var jsonResponse = JsonSerializer.Serialize(addedAuthor, _jsonOptions);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var actualAddedAuthor = await _authorService.AddAuthorAsync(authorToAdd);

            // Assert
            Assert.IsNotNull(actualAddedAuthor);
            Assert.AreEqual(addedAuthor.Id, actualAddedAuthor.Id);
            Assert.AreEqual(authorToAdd.FirstName, actualAddedAuthor.FirstName);
        }

        [TestMethod()]
        public async Task UpdateAuthorAsyncTest_ReturnsTrueOnSuccess()
        {
            // Arrange
            var authorToUpdate = new Author { Id = 1, IdBook = 401, FirstName = "Updated", LastName = "Author" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var isUpdated = await _authorService.UpdateAuthorAsync(authorToUpdate);

            // Assert
            Assert.IsTrue(isUpdated);
        }

        [TestMethod()]
        public async Task UpdateAuthorAsyncTest_ReturnsFalseOnNotFound()
        {
            // Arrange
            var authorToUpdate = new Author { Id = 1, IdBook = 401, FirstName = "Updated", LastName = "Author" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var isUpdated = await _authorService.UpdateAuthorAsync(authorToUpdate);

            // Assert
            Assert.IsFalse(isUpdated);
        }

        [TestMethod()]
        public async Task DeleteAuthorAsyncTest_ReturnsTrueOnSuccess()
        {
            // Arrange
            int authorIdToDelete = 1;
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains($"/{authorIdToDelete}")),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var isDeleted = await _authorService.DeleteAuthorAsync(authorIdToDelete);

            // Assert
            Assert.IsTrue(isDeleted);
        }

        [TestMethod()]
        public async Task DeleteAuthorAsyncTest_ReturnsFalseOnNotFound()
        {
            // Arrange
            int authorIdToDelete = 1;
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains($"/{authorIdToDelete}")),
                                      ItExpr.IsAny<System.Threading.CancellationToken>()
                                  )
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var isDeleted = await _authorService.DeleteAuthorAsync(authorIdToDelete);

            // Assert
            Assert.IsFalse(isDeleted);
        }
    }
}