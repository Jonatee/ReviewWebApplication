using Mapster;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileRepository _fileRepository;
        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IFileRepository fileRepository)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
        }

        public async Task<BaseResponse<CommentResponseModel>> CreateComment(CommentRequestModel request)
        {
            var comment = new Comment()
            {
                 CommentText = request.CommentText,
                 FileUrl = await _fileRepository.UploadAsync(request.FileUrl),
                 PostId= request.PostId,
                 ReviewerId = request.ReviewerId,
            };
           await _commentRepository.Create(comment);
            await _unitOfWork.SaveWork();
            return new BaseResponse<CommentResponseModel>
            {
                Data = comment.Adapt<CommentResponseModel>(),
                Success = true,
                Message = "Comment Posted"
            };

        }

        public async Task<BaseResponse<CommentResponseModel>> DeleteComments(Guid postId, Guid rewiewerId, Guid commentId)
        {
            var delete = await _commentRepository.Delete(commentId, postId, rewiewerId);
            if (delete != true)
            {
                return new BaseResponse<CommentResponseModel>
                {
                    Message = "Comment not Found"
                };
            }
            await _unitOfWork.SaveWork();
            return new BaseResponse<CommentResponseModel>
            {
                Message =  "Comment Deleted",
                Success = true,
            };

        }

        public async Task<BaseResponse<ICollection<CommentResponseModel>>> GetAllComments()
        {
           var allComments = await _commentRepository.GetAll();
            if(allComments.Count == 0 || allComments.All(a => a.IsDeleted == true))
            {
                return new BaseResponse<ICollection<CommentResponseModel>>
                {
                    Message = "No Commments Yet",
                };
            }
            return new BaseResponse<ICollection<CommentResponseModel>>
            {
                Data = allComments.SkipWhile(x => x.IsDeleted == true).Select(a => a.Adapt<CommentResponseModel>()).ToList(),
                Message = "All Comments",
                Success = true
            };
        }

        public async Task<BaseResponse<CommentResponseModel>> GetComment(CommentRequestModel request)
        {
           var getComment = await _commentRepository.Get(x => x.ReviewerId == request.ReviewerId && x.PostId == request.PostId && x.IsDeleted == false);
            if(getComment == null)
            {
                return new BaseResponse<CommentResponseModel>
                {
                    Message = "Comment not Found"
                };
            }
            return new BaseResponse<CommentResponseModel>
            {
                Data = getComment.Adapt<CommentResponseModel>(),
                Message = "Comment Found",
                Success = true
            };

        }
    }
}
