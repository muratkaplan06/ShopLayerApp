using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApp.API.Middlewares;
using ShopApp.Business.Abstract;
using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.DataContext.Initializer;

namespace ShopApp.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
     

        public ProductController(IProductService productService)
        {
            _productService= productService;
            
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetProductList([FromQuery] ProductQueryParameters productQueryParameters)
        {
            var result = _productService.GetAll(productQueryParameters);
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public JsonResult GetCategoryById([FromQuery] QueryParametersModel queryParameters,int categoryId)
        {
            var result = _productService.GetByCategoryId(queryParameters,categoryId);
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetProductById(int id)
        {
            var result = _productService.GetById(id);
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };
        }

        [HttpPost]
        [AuthorizeRoles(UserRoles.Admin)]
        public IActionResult AddProduct(CrudProductModel productModel) 
        {
            var result = _productService.AddProduct(productModel);
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };

        }

        [HttpPut]
        [AuthorizeRoles(UserRoles.Admin)]
        public IActionResult Update(CrudProductModel productModel)
        {
            var result = _productService.UpdateProduct(productModel);
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };

        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRoles.Admin)]
        public JsonResult DeleteEmployeeById(int id)
        {
            var result = _productService.DeleteProductById(id);
            return new JsonResult(result);
        }
    }
}
