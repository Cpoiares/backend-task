using BackendTask.Database.Repository;
using BackendTask.Models;
using BackendTask.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [Authorize(Policy = "Policy")]
        [HttpGet("category/getall")]
        public ActionResult<IDictionary<string, CategoryResponse>> Get()
        {
            var categories = _categoryService.GetAllCategories();
            if (categories == null)
                return NotFound("No categories found");
            return Ok(categories);
        }

        [Authorize(Policy = "Policy")]
        [HttpPost("category/addchild")]
        public ActionResult AddChildByName(string name, string? parentName)
        {
            try
            {
                Category child = new Category() { };
                child.Name = name;
                _categoryService.AddChildCategory(child, parentName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [Authorize(Policy = "Policy")]
        [HttpPost("category/delete")]
        public ActionResult DeleteCategoryByName(string categoryName)
        {
            try
            {
                _categoryService.DeleteCategoryByName(categoryName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
