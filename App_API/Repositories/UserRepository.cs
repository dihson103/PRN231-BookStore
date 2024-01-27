using App_API.Models;

namespace App_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Assignment2Prn231Context _context;
        public UserRepository(Assignment2Prn231Context context)
        {
            _context = context;
        }

        public void ChangeUserRole(int userId, int roleId)
        {
            var user = GetUserById(userId);
            user.RoleId = roleId;

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = GetUserById(userId);

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users.SingleOrDefault(u => u.EmailAddress== email && u.Password == password);
        }

        public User GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(x => x.UserId== id);
        }

        public bool IsEmailWasUsed(string email)
        {
            return _context.Users.Any(u => u.EmailAddress== email);
        }

        public bool IsUserExist(int userId)
        {
            return _context.Users.Any(x => x.UserId== userId);
        }

        public void RegisterUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
