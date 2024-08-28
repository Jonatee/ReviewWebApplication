using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review_Web_App.Constants;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICategoryService _categoryService;
        public CategoryController(IHttpContextAccessor contextAccessor, ICategoryService categoryService)
        {
            _contextAccessor = contextAccessor;
            _categoryService = categoryService;
        }
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAllCategories();
            if(response.Success)
            {
                var categories = response.Data;
                return View(categories);
            }
            else
            {
                ViewBag.Message = response.Message;
                return View();
            }
        }
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _categoryService.GetCategory(id);
            if(response.Success)
            {
                var category = response.Data;
                return View(category);
            }
            else
            {
                ViewBag.Message = response.Message;
                return View();
            }
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequestModel model)
        {
            var response =  await _categoryService.CreateCategory(model);
            if (response.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = response.Message;
                return View();
            }
            
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public  IActionResult Edit(string name)
        {
            var model = new UpdateCategoryModel { PreviousName = name };
            return View(model);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryModel model)
        {
            
            var response = await _categoryService.UpdateCategory(model);
            if(response.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = response.Message;
                return View();
            }
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public IActionResult Delete(int id)
        {
            return View();
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost]
        public async Task<IActionResult> Delete(CategoryRequestModel model)
        {
            var response = await _categoryService.DeleteCategory(model);
            if (response.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = response.Message;
                return View();
            }
        }
    }
}
