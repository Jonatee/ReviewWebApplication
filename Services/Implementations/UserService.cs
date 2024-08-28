using Mapster;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IFileRepository fileRepository, IUnitOfWork unitOfWork,IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<BaseResponse<ICollection<UserResponse>>> GetAllUsers()
        {
            var getAll = await _userRepository.GetAll();
            if(getAll.Count == 0 || getAll.All(x=>x.IsDeleted == true))
            {
                return new BaseResponse<ICollection<UserResponse>>
                {
                    Message = "No users Found"
                };
            }
            return new BaseResponse<ICollection<UserResponse>>
            {
                Message = "All Users",
                Success = true,
                Data = getAll.SkipWhile(x=>x.IsDeleted == true).Select(x => x.Adapt<UserResponse>()).ToList()
            };
        }

        public async Task<BaseResponse<UserResponse>> GetUser(Guid id)
        {
            var getUser = await _userRepository.Get(id);
            if (getUser == null || getUser.IsDeleted == true) 
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "User not Found"
                };
            }
            return new BaseResponse<UserResponse>
            {
                Message = "User Found ",
                Success = true,
                Data = new UserResponse
                {
                  Email = getUser.Email,
                  Id = getUser.Id,
                  UserName = getUser.UserName,
                  Role = getUser.Role
                }
            };
        }

        public async Task<BaseResponse<UserResponse>> Login(LoginRequest model)
        {
            var user = await _userRepository.GetByEmail(model.Email);
            if(user == null || user.IsDeleted == true)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "Incorrect Credentials",
                    Success = false,
                    
                };
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (isPasswordValid)
            {
                return new BaseResponse<UserResponse>
                {
                    Data = new UserResponse
                    {
                         UserName = user.UserName,
                         Id = user.Id,
                         Role = user.Role,
                          Email = user.Email
                    },
                    Message = "Login Successfull",
                    Success = true,
                };
            }
            else
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "Invalid Credentials",
                };
            }

        }

        public Task<BaseResponse<UserResponse>> RemoveUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
