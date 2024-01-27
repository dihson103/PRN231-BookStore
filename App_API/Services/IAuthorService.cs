using App_API.Dtos.Authors;

namespace App_API.Services
{
    public interface IAuthorService
    {
        List<AuthorResponse> GetAll();
        AuthorResponse GetById(int id);
        void Add(AuthorCreateRequest authorCreateRequest);
        void Update(int id, AuthorUpdateRequest authorUpdateRequest);
        void Delete(int id);
    }
}
