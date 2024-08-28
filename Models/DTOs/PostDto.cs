using Review_Web_App.Entities;
using System.ComponentModel.DataAnnotations;

namespace Review_Web_App.Models.DTOs
{
    public class PostRequestModel
    {
        [Required(ErrorMessage = "Input a Title of the post")]
        [StringLength(30, ErrorMessage = "Please keep your Title Short")]
        public string PostTitle { get; set; } = default!;

        [Required(ErrorMessage = "Input the Text of the post")]
        [StringLength(500, ErrorMessage = "Can't input more than 250 characters")]
        public string PostText { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public Guid CategoryId { get; set; } = default!;
        public IFormFile? PostFile { get; set; }
    }
    public class PostResponseModel
    {
        public Guid Id { get; set; } = default!;
        public string PostTitle { get; set; } = default!;
        public string PostText { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public Guid CategoryId { get; set; } = default!;
        public string? PostFile { get; set; }
        public DateTime DateCreated { get; set; }
        public string RevieweUsername { get; set; } = default!;
        public string? ReviewerProfilePicture { get; set; }
        public ICollection<CommentResponseModel> Comments { get; set; } = new HashSet<CommentResponseModel>();
        public void SetIsCommentByUser(Guid reviewerId)
        {
            foreach(var comment in Comments)
            {
                comment.IsCommentByUser = comment.ReviewerId == reviewerId;
            }
        }
        public ICollection<LikeResponseModel> Likes { get; set; } = new HashSet<LikeResponseModel>();
        public bool IsLikedByUser { get; set; }
        public bool IsBookmarkedByUser { get; set; }
      
    }
}
