using App_API.Models;

namespace App_API.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly Assignment2Prn231Context _context;
        public PublisherRepository(Assignment2Prn231Context context)
        {
            _context = context;
        }

        public bool IsPublisherExist(int id)
        {
            return _context.Publishers.Any(x => x.PubId== id);
        }
    }
}
