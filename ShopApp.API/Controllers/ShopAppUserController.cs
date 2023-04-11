using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;

namespace ShopApp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ShopAppUserController : ControllerBase
    {
        private readonly IBasketProductService _productService;

        public ShopAppUserController(IBasketProductService productService)
        {
            _productService= productService;
        }

        [HttpPost]
        public IActionResult AddBasketProduct([FromQuery] QueryParametersModel queryParameters, BasketProductModel model)
        {
            model.UserId = GetUserId(User);
            var result = _productService.AddBasketProduct(queryParameters, model);
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };

        }


        [HttpGet]
        public IActionResult GetBasketProductList([FromQuery] QueryParametersModel queryParameters)
        {
            var result = _productService.GetAll(queryParameters, GetUserId(User));
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };
        }


        [HttpDelete]
        public JsonResult DelBasketProductById([FromQuery] QueryParametersModel queryParameters, int id)
        {

            var result = _productService.DelBasketProductById(queryParameters, GetUserId(User), id);
            return new JsonResult(result);
        }

        [HttpGet]
        public JsonResult IsAddedBasketProduct([FromQuery] QueryParametersModel queryParameters, int id)
        {
            var result = _productService.IsAddedBasketProductTable(queryParameters, id, GetUserId(User));
            return new JsonResult(result);
        }




        private string GetUserId(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

    }

}

