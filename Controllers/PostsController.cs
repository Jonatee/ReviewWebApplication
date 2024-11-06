using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Review_Web_App.Constants;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Controllers
{
    public class PostsController : Controller
    {
        private readonly ReviewAppContext _context;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly IReviewerService _reviewerService;
        private readonly IPostService _postService;
        private readonly ILikeService _likeService;


        public PostsController(ReviewAppContext context, ILikeService likeService, ICommentService commentService, IReviewerService reviewerService, ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _reviewerService = reviewerService;
            _commentService = commentService;
            _likeService = likeService;
            _postService = postService;
            _context = context;
        }

        public async Task<IActionResult> Feed()
        {
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            var response = await _postService.GetAllPosts();
            if (response.Success)
            {
                await PopulateCategoriesDropdown();
                return View(response);
            }
            ViewBag.Message = response.Message;
            return View();
        }
        [Authorize(Roles = RoleConstants.Reviewer)]
        public async Task<IActionResult> Post(Guid id)
        {
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            var post = await _postService.GetPost(id);
            if (post.Success)
            {
                await PopulateCategoriesDropdown();
                return View(post.Data);
            }
            ViewBag.Message = post.Message;
            return View();
        }
        [Authorize(Roles = RoleConstants.Reviewer)]
        public async Task<IActionResult> Comment(string comment, Guid postId, IFormFile file)
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var model = new CommentRequestModel { CommentText = comment, PostId = postId, FileUrl = file, ReviewerId = reviewerId.Data.Id };
            var response = await _commentService.CreateComment(model);
            if (response.Success)
            {
                return RedirectToAction("Post", new { id = postId });
            }
            return View();
        }


        public async Task<IActionResult> DeleteComment(Guid postId, Guid commentId)
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var response = await _commentService.DeleteComments(postId, reviewerId.Data.Id, commentId);
            if (response.Success)
            {
                return RedirectToAction("Post", new { id = postId });
            }
            ViewBag.Message = response.Message;
            return Content(response.Message);
        }


        [Authorize(Roles = RoleConstants.Reviewer)]
        public async Task<IActionResult> MyPosts()
        {
            var response = await _postService.GetReviewerPosts();
            if (response.Success)
            {
                return View(response.Data);

            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            await PopulateCategoriesDropdown();
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> SearchResults(string title,Guid? categoryId)
        {
            await PopulateCategoriesDropdown();
            var response = await _postService.Search(title, categoryId);
           
            return View(response.Data);
        }


        [Authorize(Roles = RoleConstants.Reviewer)]
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropdown();
            return View();
        }

        [Authorize(Roles = RoleConstants.Reviewer)]
        [HttpPost]
        public async Task<IActionResult> Create(PostRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _postService.CreatePost(model);
                if(response.Success)
                {
                    TempData["Message"] = response.Message;
                    return RedirectToAction("Feed");
                }
                TempData["Message"] = response.Message;
            }
            await PopulateCategoriesDropdown();
            return View(model);
        }


        private async Task PopulateCategoriesDropdown()
        {
            var categoryResponse = await _categoryService.GetAllCategories();
            var categories = categoryResponse.Data?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            ViewBag.Categories = new SelectList(categories, "Value", "Text");
        }

        [Authorize(Roles = RoleConstants.Reviewer)]
        [HttpPost]
        public async Task<IActionResult> Like(Guid postId)
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var model = new LikeRequestModel { PostId = postId, ReviewerId = reviewerId.Data.Id};
            var likeResponse = await _likeService.CreateLike(model);
            if(likeResponse.Success)
            {
                return RedirectToAction("Post", new { id = postId });
            }
            ViewBag.Message = likeResponse.Message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Unlike(Guid postId)
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var model = new LikeRequestModel { PostId = postId, ReviewerId = reviewerId.Data.Id };
            var likeResponse = await _likeService.RemoveLike(model);
            if (likeResponse.Success)
            {
                return RedirectToAction("Post", new { id = postId });
            }
            ViewBag.Message = likeResponse.Message;
            return View();
        }


        [Authorize(Roles = RoleConstants.Reviewer)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _postService.DeletePost(id);
            if(response.Success)
            {
                return RedirectToAction("MyPosts", "Posts");
            }
            return RedirectToAction("Profile","Reviewer");
        }
     
        public IActionResult BackToPreviousPage()
        {
           
                string previousPageUrl = HttpContext.Request.GetDisplayUrl();
                return Redirect(previousPageUrl);
        }
    }
}
