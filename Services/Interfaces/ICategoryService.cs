using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<BaseResponse<CategoryResponseModel>> CreateCategory(CategoryRequestModel request);
        Task<BaseResponse<CategoryResponseModel>> UpdateCategory(UpdateCategoryModel request);
        Task<BaseResponse<CategoryResponseModel>> DeleteCategory(CategoryRequestModel request);
        Task<BaseResponse<CategoryResponseModel>> GetCategory(CategoryRequestModel request);
        Task<BaseResponse<CategoryResponseModel>> GetCategory(Guid id);
        Task<BaseResponse<ICollection<CategoryResponseModel>>> GetAllCategories();
    }
}
