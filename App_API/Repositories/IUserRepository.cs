using App_API.Models;

namespace App_API.Repositories
{
    public interface IUserRepository
    {
        void RegisterUser(User user);
        User GetUserById(int id);
        User GetUserByEmailAndPassword(string email, string password);
        List<User> GetAllUsers();
        void UpdateUser(User user);
        void DeleteUser(int userId);
        void ChangeUserRole(int userId, int roleId);    
        bool IsUserExist(int userId);
        bool IsEmailWasUsed(string email);
    }
}
