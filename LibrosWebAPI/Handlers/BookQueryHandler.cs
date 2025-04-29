using BooksWebAPI.Models;
using BooksWebAPI.Queries;
using MediatR;
using System.Net;

namespace BooksWebAPI.Handlers
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooks, IEnumerable<Book>>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllBooksHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Book>> Handle(GetAllBooks request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("BaseURL");
            try
            {
                var response = await httpClient.GetAsync("api/v1/Books", cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<Book>>(cancellationToken: cancellationToken);
            }
            catch (Exception)
            {
                throw new Exception($"No se pudo connectar con el servicio de libros");
            }
            
        }
    }

    public class GetBookByIdHandler : IRequestHandler<GetBookById, Book>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetBookByIdHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Book> Handle(GetBookById request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("BaseURL");
            try
            {
                var response = await httpClient.GetAsync($"api/v1/Books/{request.Id}", cancellationToken);
                response.EnsureSuccessStatusCode();
                var book = await response.Content.ReadFromJsonAsync<Book>(cancellationToken: cancellationToken);
                if (book == null)
                {
                    return null;
                }
                return book;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
