using App_API.Dtos.Books;

namespace App_API.Services
{
    public interface IBookService
    {
        List<BookResponse> GetBooks();
        BookResponse GetById(int id);
        void Add(BookCreateRequest bookCreateRequest);
        void Update(int id, BookUpdateRequest bookUpdateRequest);
        void Delete(int id);

    }
}
