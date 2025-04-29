using BooksWebAPI.Commands;
using BooksWebAPI.Models;
using MediatR;
using System.Net;

namespace BooksWebAPI.Handlers
{
    public class CreateBookHandler : IRequestHandler<CreateBook, Book>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateBookHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Book> Handle(CreateBook request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("BaseURL");
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/v1/Books", request.Book, cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Book>(cancellationToken: cancellationToken);
            }
            catch (Exception)
            {
                throw new Exception($"No se pudo crear el libro");
            }
            
        }
    }

    public class UpdateBookHandler : IRequestHandler<UpdateBook, Unit>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UpdateBookHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Unit> Handle(UpdateBook request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("BaseURL");

            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/v1/Books/{request.Id}", request.Book, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"No se encontró el libro con ID {request.Id}");
            }

            return Unit.Value;
        }

        public class DeleteBookHandler : IRequestHandler<DeleteBook, Unit>
        {
            private readonly IHttpClientFactory _httpClientFactory;

            public DeleteBookHandler(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }

            public async Task<Unit> Handle(DeleteBook request, CancellationToken cancellationToken)
            {
                var httpClient = _httpClientFactory.CreateClient("BaseURL");

                try
                {
                    var response = await httpClient.DeleteAsync($"api/v1/Books/{request.Id}", cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception($"No se encontró el libro con ID {request.Id}");
                }

                return Unit.Value;
            }
        }
    }
}
