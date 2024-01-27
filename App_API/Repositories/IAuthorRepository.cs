using App_API.Models;

namespace App_API.Repositories
{
    public interface IAuthorRepository
    {
        List<Author> GetAuthors();
        Author GetById(int id);
        void Add(Author author);
        void Update(Author author);
        void Delete(Author author);
        bool IsAuthorExist(int id);
        bool IsEmailExist(string email);    
    }
}
