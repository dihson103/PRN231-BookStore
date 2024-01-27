using App_API.Models;
using Microsoft.EntityFrameworkCore;

namespace App_API.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly Assignment2Prn231Context _context;
        public PublisherRepository(Assignment2Prn231Context context)
        {
            _context = context;
        }

        public void Add(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var publisher = GetById(id);
            _context.Publishers.Remove(publisher);
            _context.SaveChanges();
        }

        public List<Publisher> GetAll()
        {
            return _context.Publishers.ToList();
        }

        public Publisher GetById(int id)
        {
            return _context.Publishers.SingleOrDefault(p => p.PubId == id);
        }

        public Publisher GetPublisherWithUsersAndBook(int id)
        {
            return _context.Publishers
                .Include(p => p.Users)
                .Include(p => p.Books)
                .SingleOrDefault(p => p.PubId == id);
        }

        public bool IsPublisherExist(int id)
        {
            return _context.Publishers.Any(x => x.PubId== id);
        }

        public void Update(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            _context.SaveChanges();
        }
    }
}
