using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sample.WebApiRestful.Authentication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Infrastucture.Configuration
{
    public static class ConfigurationTokenBear
    {
        public static void RegisterTokenBear(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["TokenBear:Issuer"], // người cấp phát token, thường truyền đường link name project hiện tại
                        ValidateIssuer = false,
                        ValidAudience = configuration["TokenBear:Audience"], // người phát hành
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenBear:Key"])), // secretkey
                        ValidateIssuerSigningKey = true, // check key
                        ValidateLifetime = true, // giới hạn tg token
                        ClockSkew = TimeSpan.Zero,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context => // step 2 if have authorize
                        {
                            var tokenHandler = context.HttpContext.RequestServices.GetRequiredService<ITokenHandler>();

                            return tokenHandler.ValidateToken(context);
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("Authentication failed: " + context.Exception.Message);
                            Console.WriteLine(context.Exception.StackTrace);
                            return Task.CompletedTask;

                        },
                        OnMessageReceived = context => // step 1
                        {
                            var token = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (string.IsNullOrEmpty(token))
                            {
                                Console.WriteLine("No token received");
                            }
                            else
                            {
                                Console.WriteLine($"Token received: {token}");
                            }
                            return Task.CompletedTask;

                        },
                        OnChallenge = context => // step 3
                        {
                            return Task.CompletedTask;

                        }
                    };
                });
        }
    }
}
