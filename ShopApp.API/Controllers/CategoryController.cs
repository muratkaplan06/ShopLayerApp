using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Core.QueryParameters;

namespace ShopApp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService= categoryService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] CategoryQueryParameters categoryQueryParameters)
        {
            var result = _categoryService.GetAllByCategoryId(categoryQueryParameters);
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };
        }

    }
}
