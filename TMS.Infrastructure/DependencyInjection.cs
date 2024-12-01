﻿using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using TMS.Application.Common.Interfaces;
using TMS.Persistence.Identity;
using TMS.Persistence;
using TMS.Infrastructure.Services;

namespace TMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IWebHostEnvironment environment)
        {

            if (environment.IsEnvironment("Test"))
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<AppUser, ApplicationDbContext>(options =>
                    {
                        options.Clients.Add(new Client
                        {
                            ClientId = "TMS.IntegrationTests",
                            AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            AllowedScopes = { "TMS.WebUIAPI", "openid", "profile" }
                        });
                    })
                    .AddTestUsers(new List<TestUser>
                    {
                        new TestUser
                        {
                            SubjectId = "4c7c3792-0d6f-44e4-8d35-9d0df37178f3",
                            Username = "admin@tms",
                            Password = "Tms!",
                            Claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Email, "admin@tms")
                            }
                        }
                    });
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<AppUser, ApplicationDbContext>();

                services.AddTransient<IDateTime, DateTimeService>();
                services.AddTransient<IIdentityService, IdentityService>();
            }

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}
