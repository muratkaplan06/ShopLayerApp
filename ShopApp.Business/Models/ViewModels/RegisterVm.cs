using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.DataAccess.DataContext.Initializer;

namespace ShopApp.Business.Models.ViewModels
{
    public class RegisterVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = UserRoles.Customer;
    }
}
