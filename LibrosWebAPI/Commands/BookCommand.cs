using BooksWebAPI.Models;
using MediatR;

namespace BooksWebAPI.Commands
{
    public class CreateBook : IRequest<Book>
    {
        public Book Book { get; }

        public CreateBook(Book book)
        {
            Book = book;
        }
    }

    // Clase para actualizar un libro
    public class UpdateBook : IRequest<Unit>
    {
        public int Id { get; }
        public Book Book { get; }

        public UpdateBook(int id, Book book)
        {
            Id = id;
            Book = book;
        }
    }

    // Clase para eliminar un libro
    public class DeleteBook : IRequest<Unit>
    {
        public int Id { get; }

        public DeleteBook(int id)
        {
            Id = id;
        }
    }

}
