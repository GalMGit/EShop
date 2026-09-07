using System.Reflection;
using System.Security.Claims;
using System.Text;
using EShop.Shared.Endpoint;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Modules.Identity.Application.Abstractions.Auth;
using Modules.Identity.Features.CreateUser;
using Modules.Identity.Infrastructure.Auth;
using Modules.Identity.Infrastructure.Persistence.Database;
using Modules.Identity.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.RabbitMQ;

namespace Modules.Identity.DI;

public static class DependencyInjection
{
    extension(WolverineOptions options)
    {
        public void AddIdentityMessaging()
        {
            options.Discovery.IncludeAssembly(
                typeof(IdentityModuleMarker).Assembly);

            options.PublishMessage<CreateUserRequest>()
                .ToRabbitQueue("eshop-email");
        }
    }
    
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIdentityModule(
            IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(o =>
            {
                o.UseNpgsql(configuration.GetConnectionString("IdentityDatabase"));
            });
            
            services.AddAuth(configuration);
            
            services.Configure<AdminOptions>(
                configuration.GetSection("Identity:Admin"));
            
            services.AddEndpoints(typeof(IdentityModuleMarker).Assembly);
            services.AddValidatorsFromAssembly(typeof(IdentityModuleMarker).Assembly);
            
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            
            return services;
        }
        
        private void AddAuth(IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
                configuration.GetSection(nameof(JwtOptions)));

            services.AddOptions<JwtOptions>()
                .Validate(o => 
                    !string.IsNullOrEmpty(o.SecretKey), 
                    "SecretKey is required")
                .ValidateOnStart();

            var jwtOptions = configuration
                .GetSection(nameof(JwtOptions))
                .Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions!.SecretKey)),
                        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                        NameClaimType = ClaimTypes.NameIdentifier,
                    };

                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var request = context.HttpContext.Request;
                            
                            var authHeader = request.Headers.Authorization.FirstOrDefault();
                            if (!string.IsNullOrEmpty(authHeader) &&
                                authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authHeader["Bearer ".Length..].Trim();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}

public sealed class IdentityModuleMarker;
