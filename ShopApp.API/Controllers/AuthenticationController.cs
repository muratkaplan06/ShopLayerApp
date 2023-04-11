using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApp.Business.Models.ViewModels;
using ShopApp.DataAccess.DataContext;
using ShopApp.DataAccess.Entities;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.DataContext.Initializer;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using Microsoft.AspNetCore.Authorization;
using ShopApp.Business.Models;

namespace ShopApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ShopAppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ShopAppDbContext context,
            IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please,provide all the required fields");
            }

            var userExist = await _userManager.FindByEmailAsync(registerVm.Email);
            if (userExist != null)
            {
                return BadRequest($"User {registerVm.Email} already exist");
            }

            var newUser = new ApplicationUser()
            {
                FirstName = registerVm.FirstName,
                LastName = registerVm.LastName,
                Email = registerVm.Email,
                UserName = registerVm.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, registerVm.Password);

            if (result.Succeeded)
            {
                switch (registerVm.Role)
                {
                    case UserRoles.Admin:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                        break;
                    case UserRoles.Customer:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Customer);
                        break;
                    default:
                        break;

                }
                var tokenValue = await GenerateJwtTokenAsync(newUser, null);

                var roles = await _userManager.GetRolesAsync(newUser);

                var registerResult = new RegisterResultVm()
                {
                    User = new UserVm()
                    {
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Id = newUser.Id,
                        Role = string.Join(',', roles),
                    },
                    AccessToken = tokenValue.AccessToken,
                    RefreshToken = tokenValue.RefreshToken,
                    ExpiresAt = tokenValue.ExpiresAt
                };

                return Ok(CustomResponseModel<RegisterResultVm>.Success(registerResult, 201));
            }
            return BadRequest("User could not be crate");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please,provide all required fields");
            }

            var userExist = await _userManager.FindByEmailAsync(loginVm.Email);


            if (userExist != null && await _userManager.CheckPasswordAsync(userExist, loginVm.Password))
            {
                var tokenValue = await GenerateJwtTokenAsync(userExist, null);
                var loginResult = new LoginResultVm()
                {
                    User = new UserVm()
                    {
                        FirstName = userExist.FirstName,
                        LastName = userExist.LastName,
                        Id = userExist.Id,

                    },
                    AccessToken = tokenValue.AccessToken,
                    RefreshToken = tokenValue.RefreshToken,
                    ExpiresAt = tokenValue.ExpiresAt
                };

                return Ok(CustomResponseModel<LoginResultVm>.Success(loginResult, 201));

            }


            return Unauthorized();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(TokenRequestVm tokenRequestVm)
        {
            if (tokenRequestVm.RefreshToken == null)
            {
                return BadRequest();
            }

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x =>
                x.Token == tokenRequestVm.RefreshToken);


            var removedEntity = _context.Entry(storedToken);
            removedEntity.State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok("log out");


        }

        [Authorize, HttpGet("Me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var appUser = await _userManager.FindByIdAsync(userId);

            var roles = await _userManager.GetRolesAsync(appUser);

            var userVm = new UserVm()
            {
                FirstName =appUser.FirstName,
                LastName = appUser.LastName,
                Id = appUser.Id,
                Role = string.Join(',', roles),
                Email = appUser.Email,
            };

            return Ok(CustomResponseModel<UserVm>.Success(userVm, 200));

        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVm tokenRequestVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please,Provide all required field");
            }

            var result = await VerifyAndGenerateTokenAsync(tokenRequestVm);

            return Ok(result);

        }

        private async Task<AuthResultVm> GenerateJwtTokenAsync(ApplicationUser user, RefreshToken rToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //add User Role Claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if (rToken != null)
            {
                var rTokenResponse = new AuthResultVm()
                {
                    AccessToken = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.Now,
                DateExpire = DateTime.Now.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResultVm()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
            return response;
        }

        private async Task<AuthResultVm> VerifyAndGenerateTokenAsync(TokenRequestVm tokenRequestVm)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x =>
                x.Token == tokenRequestVm.RefreshToken
            );
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

            if (storedToken.DateExpire >= DateTime.Now)
            {
                return await GenerateJwtTokenAsync(dbUser, storedToken);
            }
            else
            {
                return await GenerateJwtTokenAsync(dbUser, null);
            }


        }
    }
}
