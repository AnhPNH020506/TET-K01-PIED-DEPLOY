using Microsoft.EntityFrameworkCore;
using Quartz;
using DotNetEnv;
using TET.API.Extensions;
using TET.Api.Middlewares;
//using TET.API.Middlewares;
using Tet.Repository;
using Tet.Service.Category;
using Tet.Service.JwtService;
using Tet.Service.Seller;
using Tet.Service.User;
using IdentityService = Tet.Service.Identity.Service;
using IndentityIService = Tet.Service.Identity.IService;
using ProductService = Tet.Service.Product;
using MailService = Tet.Service.MailService;
using CartService = Tet.Service.Cart;
//using ProductService = TET.Service.Product
using OrderService = Tet.Service.Order;
Env.Load();

var aspnetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", aspnetCoreEnv);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//LÀ nơi khai báo, đăng kí sử dụng các đồ cho phuc vụ việc code của mình
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        )
);
builder.Services.AddJwtServices(builder.Configuration);
builder.Services.AddSwaggerServices  ();

builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IndentityIService, IdentityService>();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ProductService.IService, ProductService.Service>();
builder.Services.AddScoped<MailService.IService, MailService.Service>();
builder.Services.AddScoped<CartService.IService, CartService.Service>();
builder.Services.AddScoped<OrderService.IService, OrderService.Service>();
builder.Services.AddQuartz(options =>
{
    var jobKey = new JobKey(nameof(ProcessTransactionPendingJob));

    options
        .AddJob<ProcessTransactionPendingJob>(jobKey)
        .AddTrigger(trigger =>
            trigger
                .ForJob(jobKey)
                .WithSimpleSchedule(schedule => schedule
                    .WithIntervalInMinutes(2)
                    .RepeatForever()
                )
        );
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});
//Viết cho [PIEDTEAM - STAGE 2] - BACKEND CLASS 🔥
//test, try again nha tời
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();//dòng này mới lưu ý nha. Lưu ý vì sao dòng này nằm đây
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();