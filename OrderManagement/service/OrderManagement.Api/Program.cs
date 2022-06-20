using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using OrderManagement.Application;
using OrderManagement.Application.Contract.Accounts;
using OrderManagement.Application.Contract.Orders;
using OrderManagement.Application.Helpers;
using OrderManagement.Domain.Contracts;
using OrderManagement.Domain.Services;
using OrderManagement.EntityFramework;
using OrderManagement.EntityFramework.Identity;
using System.Text;
using OrderManagement.Application.Contract.AppUsers;
using Serilog.Context;
using Serilog.Enrichers.AspNetCore.HttpContext;
using Serilog.Core.Enrichers;

var builder = WebApplication.CreateBuilder(args);
const string REQUEST_ID = "request-id";
// Add services to the container.

//AppServices
builder.Services.AddScoped<IAccountsAppService, AccountsAppService>();
builder.Services.AddScoped<IOrdersAppService, OrdersAppService>();
builder.Services.AddScoped<IAppUsersAppService, AppUsersAppService>();
builder.Services.AddScoped<ICurrentUserInfo, CurrentUserInfo>();

//Repositories
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IOrdersRepository, OrderRepository>();

//Domain services
builder.Services.AddScoped<IOrdersService, OrdersService>();

//configure and register automapper
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddControllers().AddNewtonsoftJson(jsonOptions =>
{
    jsonOptions.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Manager", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Provide your Bearer Token in the format `Bearer [space] TOKEN`"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, Array.Empty<string>()
                    }
                });

});

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OrderManagementDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, IdentityRole>()
               .AddEntityFrameworkStores<OrderManagementDBContext>()
               .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.RequireHttpsMetadata = false;
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                      ValidAudience = builder.Configuration["JWT:ValidAudience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                  };
              });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyOrigin();
        options.AllowAnyMethod();
    });
});

LoggerConfiguration loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration);
loggerConfiguration.Enrich.FromLogContext();
Log.Logger = loggerConfiguration.CreateLogger();

builder.Services.AddLogging(builder => builder.ClearProviders().AddSerilog(dispose: true));
var app = builder.Build();
app.UseSerilogLogContext(options =>
{
    options.EnrichersForContextFactory = httpContext =>
    {
        var result = new List<PropertyEnricher>();
        if (httpContext?.Request?.Headers != null && httpContext.Request.Headers.ContainsKey(REQUEST_ID))
        {
            var requestId = httpContext.Request.Headers[REQUEST_ID];
            result.Add(new PropertyEnricher("req", $"{REQUEST_ID}: {requestId}"));
        }
        else
        {
            result.Add(new PropertyEnricher("req", ""));
        }
        return result;
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
