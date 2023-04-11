using System.Net.Sockets;
using AutoMapper;
using ShopApp.Business.Abstract;
using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Entities;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager:ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly IProductDal _productDal;
        private readonly IMapper _mapper;

        public CategoryManager(ICategoryDal categoryDal,IProductDal productDal,IMapper mapper)
        {
            _categoryDal = categoryDal;
            _productDal = productDal;
            _mapper = mapper;
        }
        public CustomResponseModel<PaginationModel> GetAllByCategoryId(CategoryQueryParameters categoryQueryParameters)
        {
            var result = _productDal.GetProductWithCategoryName(categoryQueryParameters,
                opt => opt.CategoryId == categoryQueryParameters.CategoryId);
            var resultNoPagination = _productDal.GetListNoPagination(opt=>opt.CategoryId==categoryQueryParameters.CategoryId);
            var productsCount = resultNoPagination.Count;

            var paginationResult = new PaginationModel()
            {

                List = _mapper.Map<List<ProductListModel>>(result),
                SizePerPage = categoryQueryParameters.Size,
                TotalCount = productsCount
            };

            return CustomResponseModel<PaginationModel>.Success(paginationResult, 201);
        }
    }
}
