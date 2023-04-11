using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Core.DataAccess;
using ShopApp.DataAccess.Entities;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICategoryDal:IEntityRepository<Category>
    {
    }
}
