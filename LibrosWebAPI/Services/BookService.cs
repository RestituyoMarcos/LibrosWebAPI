using BooksWebAPI.Models;

namespace BooksWebAPI.Services
{
    public class BookService : IBookService
    {

        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://fakerestapi.azurewebsites.net/");
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/Books");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Book>>();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/v1/Books/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>();
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/Books", book);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>();
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/v1/Books/{id}", book);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>();
        }

        public async Task DeleteBookAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/v1/Books/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
