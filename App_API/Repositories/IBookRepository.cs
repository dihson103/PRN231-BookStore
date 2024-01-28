using App_API.Models;

namespace App_API.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetBooks();
        Book GetById(int id);
        void DeleteById(int id);
        void Update(Book book);
        void Add(Book book);
        bool IsBookExist(int id);
        Book GetByIdWithBookAuthor(int id);
    }
}
