using App_API.Models;

namespace App_API.Repositories
{
    public interface IPublisherRepository
    {
        bool IsPublisherExist(int id);
        List<Publisher> GetAll();
        Publisher GetById(int id);
        void Update(Publisher publisher);
        void Add(Publisher publisher);
        void Delete(int id);
        Publisher GetPublisherWithUsersAndBook(int id);
    }
}
