using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review_Web_App.Constants;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Controllers
{
    public class ReviewerController : Controller
    {
        private readonly IReviewerService _reviewerService;

        public ReviewerController(IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }
        [Authorize(Roles = RoleConstants.Reviewer)]
        public async Task<IActionResult> Profile()
        {
            var response = await _reviewerService.GetReviewerByLoggedInUser();
            if (response.Success)
            {
                return View(response.Data);
            }
            return View();
        }
        [Authorize(Roles = RoleConstants.Reviewer)]
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var response = await _reviewerService.GetReviewerByLoggedInUser();
            if(response.Success)
            {
                var model = new ReviewerUpdateModel
                {
                     Email = response.Data.Email,
                      FirstName = response.Data.FirstName,
                      LastName = response.Data.LastName,
                      UserName = response.Data.UserName
                };
                return View(model);
            }
            return View("Profile");
        }
        [Authorize(Roles = RoleConstants.Reviewer)]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ReviewerUpdateModel model)
        {
            var updateResponse = await _reviewerService.UpdateReviewer(model);
            if(updateResponse.Success)
            {
                return RedirectToAction("Profile");
            }
            return View();
        }
        public async Task<IActionResult> DeleteProfile()
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var response = await _reviewerService.DeleteReviewer(reviewerId.Data.Id);
            if(response.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Profile");
        }
         
        
    }
}
