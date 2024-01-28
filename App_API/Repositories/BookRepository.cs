using App_API.Models;
using Microsoft.EntityFrameworkCore;

namespace App_API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly Assignment2Prn231Context _context;
        public BookRepository(Assignment2Prn231Context context)
        {
            _context = context;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var book = GetById(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public List<Book> GetBooks()
        {
            return _context.Books.ToList();
        }

        public Book GetById(int id)
        {
            return _context.Books.SingleOrDefault(b => b.BookId == id);
        }

        public Book GetByIdWithBookAuthor(int id)
        {
            return _context.Books.Include(b => b.BookAuthors).SingleOrDefault(x => x.BookId == id);
        }

        public bool IsBookExist(int id)
        {
            return _context.Books.Any(b => b.BookId == id);
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}
