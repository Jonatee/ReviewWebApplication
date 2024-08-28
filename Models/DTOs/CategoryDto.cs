using Review_Web_App.Entities;
using System.ComponentModel.DataAnnotations;

namespace Review_Web_App.Models.DTOs
{
    public class CategoryRequestModel
    {
        [Required]
        public string Name { get; set; } = default!;
    }
    public class UpdateCategoryModel
    {
        [Required]
        public string PreviousName { get; set; } = default!;
        [Required]
        public string NewName { get; set; } = default!;
    }
    public class CategoryResponseModel
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public ICollection<PostResponseModel> Posts { get; set; } = new HashSet<PostResponseModel>();
    }
}
