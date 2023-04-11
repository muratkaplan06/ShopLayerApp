using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.Entities;

namespace ShopApp.Business.Abstract
{
    public interface IProductService
    {
        CustomResponseModel<PaginationModel> GetAll(ProductQueryParameters productQueryParameters);
        List<Product> GetByCategoryId(QueryParametersModel queryParameters,int categoryId);
        CustomResponseModel<ProductModel> GetById(int productId);
        CustomResponseModel<CrudProductModel> AddProduct(CrudProductModel productModel);
        CustomResponseModel<NoContentModel> UpdateProduct(CrudProductModel productModel);
        CustomResponseModel<NoContentModel> DeleteProductById(int productId);
    }
}
