using App_API.Dtos.Users;

namespace App_API.Services
{
    public interface IUserService
    {
        UserResponse Login(UserAuthRequest authRequest);
        void Registration(UserAuthRequest authRequest);
        List<UserResponse> GetAllUser();
        UserResponse GetUserById(int id);
        void DeleteUser(int id);
        void ChangeUserRole(int userId, int roleId);
        void CreateUser(UserAdminCreateRequest userAdminCreateRequest);
        void UpdateUser(int id, UserUpdateProfile userUpdateProfile);
        void ChangePassord(int id, UserChangePasswordRequest changePasswordRequest);
    }
}
