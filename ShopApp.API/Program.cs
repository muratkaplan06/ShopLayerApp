using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.Business.Mapping;
using ShopApp.Core.DataAccess;
using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework;
using ShopApp.DataAccess.DataContext;
using ShopApp.DataAccess.DataContext.Initializer;
using ShopApp.DataAccess.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//addScoped

//builder.Services.AddTransient(typeof(IEntityRepository<>), typeof(EfEntityRepositoryBase<,>));
builder.Services.AddTransient<IProductService, ProductManager>();
builder.Services.AddTransient<ICategoryService, CategoryManager>();
builder.Services.AddTransient<IBasketProductService, BasketProductManager>();
builder.Services.AddTransient<IProductDal, EfProductDal>();
builder.Services.AddTransient<ICategoryDal, EfCategoryDal>();
builder.Services.AddTransient<IBasketProductDal, EfBasketProductDal>();

builder.Services.AddDbContext<ShopAppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection") ?? string.Empty,
        option => 
        { option.MigrationsAssembly(Assembly.GetAssembly(typeof(ShopAppDbContext))?.GetName().Name); });
});



//Add Identy
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ShopAppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(MapProfile));
//Swagger Authentication
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme." +
                      " \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],

    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:Audience"],

    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);

//Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;


});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();


//Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
AppDbInitializer.SeedRolesToDb(app).Wait();
app.Run();

