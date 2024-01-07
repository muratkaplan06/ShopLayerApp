using AutoMapper;
using ShopApp.Business.Abstract;
using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Entities;

namespace ShopApp.Business.Concrete
{
    public class BasketProductManager : IBasketProductService
    {

        private readonly IBasketProductDal _basketProductDal;
        private readonly IProductDal _productDal;
        private readonly IMapper _mapper;

        public BasketProductManager(IBasketProductDal basketProductDal, IProductDal employeeDal, IMapper mapper)
        {
            _basketProductDal = basketProductDal;
            _productDal = employeeDal;
            _mapper = mapper;
        }

        //deneme amaçlı yazıldı

        public CustomResponseModel<PaginationBasketProductModel> GetAll(QueryParametersModel query, string userId)
        {
            var results = _basketProductDal.GetList(query, x => x.ApplicationUserId == userId);
            var resultNoPagination = _basketProductDal.GetListNoPagination(x => x.ApplicationUserId == userId);
            var basketProductsCout = resultNoPagination.Count;
            var basketProductIdList = results.Select(x => x.ProductId);
            var productList = _productDal.GetListNoPagination(x => basketProductIdList.Contains(x.Id));
            var productModelList = _mapper.Map<List<ProductListModel>>(productList);
            var basketList = new List<BasketProductListModel>();

            foreach (var result in results)
            {
                var basketProductListModel = new BasketProductListModel();
                basketProductListModel.Id = result.ProductId;
                var product = productModelList.SingleOrDefault(x => x.Id == result.ProductId);

                if (product != null)
                {
                    basketProductListModel.Name = product.Name;
                    basketProductListModel.Description = product.Description;
                    basketProductListModel.Price=product.Price;
                    basketProductListModel.image1 = product.image1;
                    basketProductListModel.image2=product.image2;
                    basketProductListModel.image3=product.image3;
                    basketProductListModel.CategoryName=product.CategoryName;
                }

                basketList.Add(basketProductListModel);
            }

            var paginationBasketProductResult = new PaginationBasketProductModel()
            {
                List = basketList,
                SizePerPage = query.Size,
                TotalCount = basketProductsCout,
                TotalPrice = GetTotalPrice(userId)
            };
            
            return CustomResponseModel<PaginationBasketProductModel>.Success(paginationBasketProductResult, 201);
        }
        public CustomResponseModel<bool> AddBasketProduct(QueryParametersModel query, BasketProductModel model)
        {
            var product = _productDal.Get(x => x.Id == model.ProductId);


            if (product == null)
            {
                return CustomResponseModel<bool>.Fail(404, "Product is not found.");
            }

            if (IsAdded(query, model.UserId, model.ProductId))
            {
                return CustomResponseModel<bool>.Fail(404, "Product has already added.");
            }


            var basketProduct = new BasketProduct() { ProductId = model.ProductId, ApplicationUserId = model.UserId };

            _basketProductDal.Add(basketProduct);

            if (basketProduct.Id > 0)
            {
                return CustomResponseModel<bool>.Success(true, 201);
            }

            return CustomResponseModel<bool>.Fail(403, "Product is added.");
        }

        public CustomResponseModel<NoContentModel> DelBasketProductById(QueryParametersModel query, string userId,
            int productId)
        {

            if (IsAdded(query, userId, productId) == false)
            {
                return CustomResponseModel<NoContentModel>.Fail(404, "No basket product found.");
            }

            BasketProduct basketProduct = _basketProductDal.Get(x => x.ProductId == productId && x.ApplicationUserId == userId);
            _basketProductDal.Delete(basketProduct);

            return CustomResponseModel<NoContentModel>.Success(204);
        }

        public CustomResponseModel<bool> IsAddedBasketProductTable(QueryParametersModel query, int productId, string userId)
        {
            if (IsAdded(query, userId, productId))
            {
                return CustomResponseModel<bool>.Success(true, 201);
            }

            return CustomResponseModel<bool>.Fail(404, "Product is not added.");
        }
        
        public bool IsAdded(QueryParametersModel query, string userId, int productId)
        {
            var userList = _basketProductDal.GetList(query, x => x.ApplicationUserId == userId);
            foreach (var user in userList)
            {
                if (user.ProductId == productId)
                {

                    return true;
                }
            }

            return false;
        }

        public decimal GetTotalPrice(string userId)
        {
            decimal totalPrice = 0;
            var results = _basketProductDal.GetListNoPagination( x => x.ApplicationUserId == userId);
            var basketProductIdList = results.Select(x => x.ProductId);
            var basketProductList = _productDal.GetListNoPagination(x => basketProductIdList.Contains(x.Id));
            foreach (var basketProduct in basketProductList)
            {
                totalPrice += basketProduct.Price;
            }
            return totalPrice;
        }
        
    }
}


