using System.Text;
using AccounteeApi.Endpoints;
using AccounteeApi.Middleware;
using AccounteeCommon.HttpContexts;
using AccounteeCommon.Options;
using AccounteeCQRS;
using AccounteeCQRS.Mapping;
using AccounteeDomain.Contexts;
using AccounteeService.Repositories;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services;
using AccounteeService.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace AccounteeApi.Extensions;

public static class StartupExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        });
        
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(new CamelCaseNamingStrategy()),
            },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "AccounteeAPI",
                    Version = "v1"
                });

            x.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT without Bearer into field",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.Http
                });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        builder.Services.AddAutoMapper(typeof(EntityToResponseProfile), typeof(RequestToEntityProfile));

        var jwtOptions = new JwtOptions();
        var pwdOptions = new PwdOptions();
        
        builder.Configuration.Bind("Jwt", jwtOptions);
        builder.Configuration.Bind("PwdOptions", pwdOptions);

        builder.Services.AddSingleton(jwtOptions);
        builder.Services.AddSingleton(pwdOptions);
        
        builder.Services.AddDbContext<AccounteeContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"), 
                x => x.MigrationsAssembly("AccounteeApi"));
        });

        builder.Services.AddSwaggerGenNewtonsoftSupport();
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
            .AddJwtBearer(options =>    
            {    
                options.TokenValidationParameters = new TokenValidationParameters    
                {    
                    ValidateIssuer = true,    
                    ValidateAudience = true,    
                    ValidateLifetime = true,    
                    ValidateIssuerSigningKey = true,    
                    ValidIssuer = jwtOptions.Issuer,    
                    ValidAudience = jwtOptions.Audience,    
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))    
                };    
            });

        builder.Services.AddMediatR(typeof(MediatorAssemblyReference));

        builder.AddServices();
    }

    public static void SetupPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapEndpoints();
    }
    
    private static void MapEndpoints(this WebApplication app)
    {
        app.MapSwagger();
        
        app.MapAuthEndpoints();
        app.MapCategoryEndpoints();
        app.MapCompanyEndpoints();
        app.MapUserEndpoints();
        app.MapRoleEndpoints();
        app.MapProductEndpoints();
        app.MapIncomeEndpoints();
        app.MapServiceEndpoints();
    }

    private static void AddServices(this WebApplicationBuilder builder)
    {
        var accessor = new HttpContextAccessor();
        builder.Services.AddSingleton<IHttpContextAccessor>(accessor);
        GlobalHttpContext.HttpContextAccessor = accessor;

        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
        builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
        builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
    }
}