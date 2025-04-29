using LibrosWebAPI.Models;
using System.Threading.Tasks;

namespace LibrosWebAPI.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<IEnumerable<Author>> GetAuthorsByBookIdAsync(int bookId);
        Task<Author> AddAuthorAsync(Author author);
        Task<bool> UpdateAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(int id);
    }
}
