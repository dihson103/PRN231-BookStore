using App_API.Dtos.Users;
using App_API.Exceptions;
using App_API.Models;
using App_API.Repositories;
using AutoMapper;
using System.Net;

namespace App_API.Services
{
    public class UserService : IUserService
    {
        private readonly IRoleService _roleService;
        private readonly IPublisherService _publisherService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, 
            IMapper mapper, 
            IRoleService roleService,
            IPublisherService publisherService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleService = roleService;
            _publisherService = publisherService;
        }

        public void ChangeUserRole(int userId, int roleId)
        {
            checkUserExist(userId);
            _roleService.ChekRoleExist(roleId);

            _userRepository.ChangeUserRole(userId, roleId);
        }

        public void CreateUser(UserAdminCreateRequest userAdminCreateRequest)
        {
            checkEmailExist(userAdminCreateRequest.EmailAddress);
            _roleService.ChekRoleExist(userAdminCreateRequest.RoleId);
            _publisherService.CheckPublisherExist(userAdminCreateRequest.PubId);

            var user = _mapper.Map<User>(userAdminCreateRequest);
            
            _userRepository.RegisterUser(user);
        }

        public void DeleteUser(int id)
        {
            checkUserExist(id);

            _userRepository.DeleteUser(id);
        }

        public List<UserResponse> GetAllUser()
        {
            var users = _userRepository.GetAllUsers();

            if(users == null || users.Count == 0 ) 
                throw new MyException((int)HttpStatusCode.NotFound, "UserService:: Can not find any users!");

            var userResponses = _mapper.Map<List<UserResponse>>(users);
           
            return userResponses;
        }

        public UserResponse GetUserById(int id)
        {
            checkUserExist(id);

            var user = _userRepository.GetUserById(id);
            var userResponse = _mapper.Map<UserResponse>(user);

            return userResponse;
        }

        public UserResponse Login(UserAuthRequest authRequest)
        {
            var user = _userRepository.GetUserByEmailAndPassword(authRequest.EmailAddress, authRequest.Password);
            if (user == null) throw new MyException((int)HttpStatusCode.NotFound, "UserService:: Wrong email or password!");

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.FullName = $"{user.FirstName} {user.MiddleName} {user.LastName}";

            return userResponse;

        }

        public void Registration(UserAuthRequest authRequest)
        {
            if (authRequest == null || string.IsNullOrEmpty(authRequest.EmailAddress) || string.IsNullOrEmpty(authRequest.Password))
                throw new MyException((int)HttpStatusCode.BadRequest, "UserService:: Please input all email and password");

            checkEmailExist(authRequest.EmailAddress);

            var user = _mapper.Map<User>(authRequest);
            user.RoleId = 2;

            _userRepository.RegisterUser(user);
        }

        private void checkUserExist(int id)
        {
            var isUserExist = _userRepository.IsUserExist(id);
            if (!isUserExist) throw new MyException((int)HttpStatusCode.NotFound, $"UserService:: Can not found user has id: {id}");
        }

        private void checkEmailExist(string email)
        {
            var isEmailExist = _userRepository.IsEmailWasUsed(email);
            if (isEmailExist) throw new MyException((int)HttpStatusCode.BadRequest, "UserService:: Email is already exist!");
        }

        public void UpdateUser(int id, UserUpdateProfile userUpdateProfile)
        {
            ValidateRequest(id, userUpdateProfile);

            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new MyException((int)HttpStatusCode.NotFound, $"UserService:: Cannot find user with id: {id}");
            }

            CheckAndUpdateFields(user, userUpdateProfile);

            _userRepository.UpdateUser(user);
        }

        private void ValidateRequest(int id, UserUpdateProfile userUpdateProfile)
        {
            if (id != userUpdateProfile.UserId)
            {
                throw new MyException((int)HttpStatusCode.Conflict, "UserService:: There is a conflict with the userID");
            }

            _publisherService.CheckPublisherExist(userUpdateProfile.PubId);
        }

        private void CheckAndUpdateFields(User user, UserUpdateProfile request)
        {
            if (user.EmailAddress != request.EmailAddress)
            {
                checkEmailExist(request.EmailAddress);
            }

            user.EmailAddress = request.EmailAddress;
            user.Source = request.Source;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.MiddleName = request.MiddleName;
            user.HireDate = request.HireDate;
        }

        public void ChangePassord(int id, UserChangePasswordRequest changePasswordRequest)
        {
            if (id != changePasswordRequest.UserId)
            {
                throw new MyException((int)HttpStatusCode.Conflict, "UserService:: There is a conflict with the userID");
            }

            if(string.IsNullOrEmpty(changePasswordRequest.Password))
            {
                throw new MyException((int) (HttpStatusCode.BadRequest), "Password is required!");
            }

            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new MyException((int)HttpStatusCode.NotFound, $"Can not find user has id: {id}");
            }

            user.Password = changePasswordRequest.Password;
            _userRepository.UpdateUser(user);
        }
    }
}
