using LibrosWebAPI.Models;

namespace LibrosWebAPI.Interfaces
{
    public class AuthorService : IAuthorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public AuthorService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("BaseURL");
            _endpoint = "api/v1/Authors";
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_endpoint);
                response.EnsureSuccessStatusCode();
                var authors = await response.Content.ReadFromJsonAsync<IEnumerable<Author>>();
                return authors ?? new List<Author>();
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error obteniendo los autores: {ex.Message}");
                return new List<Author>();
            }
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Author>();
                }
                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error obteniendo el autor {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Author>> GetAuthorsByBookIdAsync(int bookId)
        {
            try
            {
                var allAuthors = await GetAllAuthorsAsync();
                return allAuthors.Where(a => a.IdBook == bookId);
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error obteniendo los autores para el libro de ID {bookId}: {ex.Message}");
                return new List<Author>();
            }
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_endpoint, author);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Author>() ?? author;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error añadiendo el autor: {ex.Message}");
                return author;
            }
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{author.Id}", author);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error actualizando el autor del ID {author.Id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error eliminando el autor del ID {id}: {ex.Message}");
                return false;
            }
        }
    }
}
