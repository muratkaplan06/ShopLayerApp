using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Business.Models;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.Entities;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService
    {
        CustomResponseModel<PaginationModel> GetAllByCategoryId(CategoryQueryParameters categoryQueryParameters);

    }
}
