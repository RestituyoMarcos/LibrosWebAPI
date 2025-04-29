using BooksWebAPI.Models;
using MediatR;

namespace BooksWebAPI.Queries
{
    public class GetAllBooks : IRequest<IEnumerable<Book>>;
    public class GetBookById : IRequest<Book>
    {
        public int Id { get; set; }
    }
}
