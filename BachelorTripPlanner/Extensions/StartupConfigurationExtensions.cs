using Domain.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BachelorTripPlanner.Extensions
{
    public static class StartupConfigurationExtensions
    {
        public static void AddSwaggerConfigurations(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo { Title = "TripPlanner API", Version = "v1" });
                //options.AddSecurityDefinition("Bearer",
                //new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey
                //});
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //{
                //    new OpenApiSecurityScheme
                //    {
                //    Reference = new OpenApiReference
                //        {
                //        Type = ReferenceType.SecurityScheme,
                //        Id = "Bearer"
                //        },
                //        Scheme = "oauth2",
                //        Name = "Bearer",
                //        In = ParameterLocation.Header,
                //    },
                //    new List<string>()
                //    }
                //});
            });
        }

        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    //ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GlobalConstants.JwtSecretKey)),
                    //ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = false
                };
            });
        }
    }
}