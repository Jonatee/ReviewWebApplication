using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface IUserService
    {
     
        Task<BaseResponse<UserResponse>> GetUser(Guid id);
        Task<BaseResponse<ICollection<UserResponse>>> GetAllUsers();
        Task<BaseResponse<UserResponse>> RemoveUser(Guid id);
        Task<BaseResponse<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request);
        Task<BaseResponse<UserResponse>> Login(LoginRequest model);
    }
}
