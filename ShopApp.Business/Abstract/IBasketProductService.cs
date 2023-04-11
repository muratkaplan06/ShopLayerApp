using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;

namespace ShopApp.Business.Abstract
{
    public interface IBasketProductService
    {
        CustomResponseModel<PaginationBasketProductModel> GetAll(QueryParametersModel query, string userId);
        CustomResponseModel<bool> AddBasketProduct(QueryParametersModel query, BasketProductModel model);
        CustomResponseModel<NoContentModel> DelBasketProductById(QueryParametersModel query, string userId, int productId);
        CustomResponseModel<bool> IsAddedBasketProductTable(QueryParametersModel query, int productId, string userId);
    }
}
