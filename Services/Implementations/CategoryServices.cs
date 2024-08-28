using Mapster;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Services.Implementations
{
    public class CategoryServices : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        

        public CategoryServices(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CategoryResponseModel>> CreateCategory(CategoryRequestModel request)
        {
           var checkForDuplicate = await _categoryRepository.Check(x => x.Name == request.Name && x.IsDeleted == false);
            if(checkForDuplicate)
            {
                return new BaseResponse<CategoryResponseModel>
                {
                    Success = false,
                    Message = "Category Already Exists",
                    Data = null
                };
            }
            var category = new Category()
            {
                Name = request.Name,
                
            };
            var addcategory = await _categoryRepository.Create(category);
            await _unitOfWork.SaveWork();

            return new BaseResponse<CategoryResponseModel>
            {
                Data = category.Adapt<CategoryResponseModel>(),
                Message = "New Category Added",
                Success = true
            };
        }

        public async Task<BaseResponse<CategoryResponseModel>> DeleteCategory(CategoryRequestModel request)
        {
            var getCategory = await _categoryRepository.Get(x => x.Name == request.Name);
            if( getCategory == null)
            {
                return new BaseResponse<CategoryResponseModel>
                {
                    Data = null,
                    Message = "Category Does Not Exist",
                    Success = false
                };
            }
            getCategory.IsDeleted = true;
            var update = _categoryRepository.Update(getCategory);
            await _unitOfWork.SaveWork();
            return new BaseResponse<CategoryResponseModel>
            {
                Data = null,
                Message = "Category Deleted Successfully",
                Success = true
            };
        }

        public async Task<BaseResponse<ICollection<CategoryResponseModel>>> GetAllCategories()
        {
            var getAllCategories = await _categoryRepository.GetAll();
            if(getAllCategories.Count == 0 || getAllCategories.All(a=>a.IsDeleted == true))
            {
                return new BaseResponse<ICollection<CategoryResponseModel>>
                {
                    Data = null,
                    Message = "No Category Exists",
                    Success = false
                    
                };
            }
            return new BaseResponse<ICollection<CategoryResponseModel>>
            {
                Data = getAllCategories.SkipWhile(x=>x.IsDeleted == true).Select(a => a.Adapt<CategoryResponseModel>()).ToList(),
                Message = " All Categories",
                Success = true
            };
        }

        public async Task<BaseResponse<CategoryResponseModel>> GetCategory(CategoryRequestModel request)
        {
            var getCategory = await _categoryRepository.Get(x => x.Name == request.Name);
            if (getCategory == null || getCategory.IsDeleted == true)
            {
                return new BaseResponse<CategoryResponseModel>
                {
                    Data = null,
                    Message = "Category Not Found",
                    Success = false

                };
            }
            return new BaseResponse<CategoryResponseModel>
            {
                Data = getCategory.Adapt<CategoryResponseModel>(),
                Message = "Category Found",
                Success = true
            };
        }

        public async Task<BaseResponse<CategoryResponseModel>> GetCategory(Guid id)
        {
            var getCategory = await _categoryRepository.Get(id);
                if(getCategory == null || getCategory.IsDeleted == true)
            {
                return new BaseResponse<CategoryResponseModel>
                {
                    Data = null,
                    Message = "not Found",
                    Success = false
                };
            }
            return new BaseResponse<CategoryResponseModel>
            {
                Data = getCategory.Adapt<CategoryResponseModel>(),
                Message = "Category Found",
                Success = true,
            };


        }

        public async Task<BaseResponse<CategoryResponseModel>> UpdateCategory(UpdateCategoryModel request)
        {
            var getCategory = await _categoryRepository.Get(x => x.Name == request.PreviousName);
            if(getCategory == null)
            {
                return new BaseResponse<CategoryResponseModel>
                {
                    Message = "Not Found",
                    Success = false
                };
            }
            getCategory.Name = request.NewName;
            var update = _categoryRepository.Update(getCategory);
            await _unitOfWork.SaveWork();
            return new BaseResponse<CategoryResponseModel>
            {
                Message = "Updated successfully",
                Success = true
            };

        }
    }
}
