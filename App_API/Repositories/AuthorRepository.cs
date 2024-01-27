using App_API.Models;

namespace App_API.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly Assignment2Prn231Context _context;
        public AuthorRepository(Assignment2Prn231Context context)
        {
            _context = context;
        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void Delete(Author author)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }

        public List<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        }

        public Author GetById(int id)
        {
            return _context.Authors.SingleOrDefault(a => a.AuthorId == id);
        }

        public bool IsAuthorExist(int id)
        {
            return _context.Authors.Any(a => a.AuthorId == id);
        }

        public bool IsEmailExist(string email)
        {
            return _context.Authors.Any(a => a.EmailAddress== email);
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
        }
    }
}
