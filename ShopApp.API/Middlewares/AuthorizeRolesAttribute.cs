using Microsoft.AspNetCore.Authorization;

namespace ShopApp.API.Middlewares
{
    public class AuthorizeRolesAttribute:AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = string.Join(',', roles);
        }
    }
}
