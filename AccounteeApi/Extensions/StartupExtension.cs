using System.Text;
using AccounteeApi.Middleware;
using AccounteeCommon.Options;
using AccounteeDomain.Contexts;
using AccounteeService.Mapping;
using AccounteeService.PrivateServices;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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
        builder.Services.AddAutoMapper(typeof(MapperProfile));
        
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

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(new CamelCaseNamingStrategy()),
            },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

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
        app.MapControllers();
    }

    private static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddScoped<IAuthPrivateService, AuthPrivateService>();
        builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();
        builder.Services.AddScoped<ICurrentUserPrivateService, CurrentUserPrivateService>();
        
        builder.Services.AddScoped<IAuthPublicService, AuthPublicService>();
        builder.Services.AddScoped<ICompanyPublicService, CompanyPublicService>();
        builder.Services.AddScoped<IUserPublicService, UserPublicService>();
        builder.Services.AddScoped<IRolePublicService, RolePublicService>();
        builder.Services.AddScoped<IIncomePublicService, IncomePublicService>();
    }
}