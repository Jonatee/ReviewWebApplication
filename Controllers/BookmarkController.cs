using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Services.Interfaces;
using System.Security.AccessControl;

namespace Review_Web_App.Controllers
{
    public class BookmarkController : Controller
    {
        private readonly IReviewerService _reviewerService;
        private readonly IBookmarkService _bookmarkService;
        public BookmarkController(IReviewerService reviewerService,IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
            _reviewerService = reviewerService;
        }
        public async Task<IActionResult> AllBookmarks()
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var response = await _bookmarkService.GetReviewerBookmark(reviewerId.Data.Id);
            if(response.Success)
            {
                return View(response.Data);
            }
            ViewBag.Message = response.Message;
            return Content(response.Message);
        }


        [HttpPost]
        public async Task<IActionResult> AddToBookmark(Guid postId)
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var requestModel = new BookmarkRequestModel { PostId = postId, ReviewerId = reviewerId.Data.Id };
            var responseBookmark = await _bookmarkService.CreateBookmark(requestModel);
            if(responseBookmark.Success)
            {
                return RedirectToAction("Post", "Posts", new { id = requestModel.PostId });
            }
            ViewBag.Message = responseBookmark.Message;
            return Content(responseBookmark.Message);
        }


       

        [HttpPost]
        public async Task<IActionResult> RemoveFromBookmarks(Guid postId)
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var requestModel = new BookmarkRequestModel { PostId = postId, ReviewerId = reviewerId.Data.Id };
            var responseBookmark = await _bookmarkService.DeleteBookmark(requestModel);
            if (responseBookmark.Success)
            {
                return RedirectToAction("Post", "Posts", new { id = requestModel.PostId });
            }
            ViewBag.Message = responseBookmark.Message;
            return Content(responseBookmark.Message);
        }
    }
}
