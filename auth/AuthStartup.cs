﻿using app.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace auth
{
    public static class AuthStartup
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthSwagger(configuration);

            services.Configure<AuthConfig>(configuration.GetSection("Auth"));

            services.AddSingleton<AuthService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var configuracaoAutenticaco = configuration.GetSection("Auth");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuracaoAutenticaco["Issuer"],
                    ValidAudience = configuracaoAutenticaco["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracaoAutenticaco["Key"]!)),
                    ValidateIssuer = bool.Parse(configuracaoAutenticaco["ValidateIssuer"] ?? "false"),
                    ValidateAudience = bool.Parse(configuracaoAutenticaco["ValidateAudience"] ?? "false"),
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = bool.Parse(configuracaoAutenticaco["ValidateIssuerSigningKey"] ?? "false")
                };
            });

            services.AddAuthorization();

            services.AddControllers(o => o.Filters.Add(typeof(AuthExceptionHandler)));
        }

        public static void AddAuthSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}