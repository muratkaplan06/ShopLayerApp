using Microsoft.AspNetCore.Identity;

namespace ShopApp.DataAccess.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<BasketProduct> BasketProducts { get; set; }
    }

    
}
