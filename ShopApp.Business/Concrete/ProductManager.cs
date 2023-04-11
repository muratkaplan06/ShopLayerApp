using AutoMapper;
using ShopApp.Business.Abstract;
using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework;
using ShopApp.DataAccess.Entities;

namespace ShopApp.Business.Concrete
{
    public class ProductManager:IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IMapper _mapper;

        public ProductManager(IProductDal productDal,IMapper mapper)
        {
            _productDal = productDal;
            _mapper = mapper;
        }

        public CustomResponseModel<PaginationModel> GetAll(ProductQueryParameters productQueryParameters)
        {
            var result = !string.IsNullOrEmpty(productQueryParameters.Name)? 
                _productDal.GetProductWithCategoryName(productQueryParameters, opt =>
                        (opt.Name.ToLower()).Contains(productQueryParameters.Name.ToLower())).OrderBy(opt => opt.Name).ToList() :
                _productDal.GetProductWithCategoryName(productQueryParameters);

            var resultNoPagination = _productDal.GetListNoPagination();
            var productsCount = resultNoPagination.Count;

            var paginationResult = new PaginationModel()
            {

                List = _mapper.Map<List<ProductListModel>>(result),
                SizePerPage = productQueryParameters.Size,
                TotalCount =productsCount
            };

            return CustomResponseModel<PaginationModel>.Success(paginationResult, 201);
           
        }

        public List<Product> GetByCategoryId(QueryParametersModel queryParameters,int categoryId)
        {
            return _productDal.GetList(queryParameters,p => p.CategoryId == categoryId);
        }

        public CustomResponseModel<ProductModel> GetById(int productId)
        {

            var result = _productDal.Get(i => i.Id == productId);

            var productModelsCard = _mapper.Map<ProductModel>(result);

            return CustomResponseModel<ProductModel>.Success( productModelsCard, 201);
        }

        public CustomResponseModel<CrudProductModel> AddProduct(CrudProductModel productModel)
        {
            var productDb = _mapper.Map<CrudProductModel,Product>(productModel);

            _productDal.Add(productDb);

            return CustomResponseModel<CrudProductModel>.Success(productModel, 201);
        }

        public CustomResponseModel<NoContentModel> UpdateProduct(CrudProductModel productModel)
        {
            var currentEmployee = _mapper.Map<CrudProductModel, Product>(productModel);

            _productDal.Update(currentEmployee);

            return CustomResponseModel<NoContentModel>.Success(204);
        }

        public CustomResponseModel<NoContentModel> DeleteProductById(int productId)
        {

            var product = _productDal.Get(i => i.Id == productId);
            if (product == null)
            {
                return CustomResponseModel<NoContentModel>.Fail(404, "No product found with this id");
            }

            _productDal.Delete(product);

            return CustomResponseModel<NoContentModel>.Success(204);
        }
    }
}
