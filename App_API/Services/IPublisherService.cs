using App_API.Dtos.Publishers;
using App_API.Models;

namespace App_API.Services
{
    public interface IPublisherService
    {
        void CheckPublisherExist(int? id);
        List<PublisherResponse> GetAll();
        PublisherResponse GetById(int id);
        void Update(int id, PublisherUpdateRequest publisherUpdateRequest);
        void Add(PublisherCreateRequest publisherCreateRequest);
        void Delete(int id);
    }
}
