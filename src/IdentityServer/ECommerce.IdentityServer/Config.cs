// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace ECommerce.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResource => new ApiResource[]
        {
            new ApiResource("resource_catalog"){ Scopes = {"catalog_fullpermission"} },
            new ApiResource("photo_stock_catalog"){ Scopes = {"photo_stock_fullpermission"} },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource()
                       {
                           Name = "roles", 
                           DisplayName = "Roles", 
                           Description = "Kullanıcı Rolleri", 
                           UserClaims = new [] { "role" }
                       }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission", "full access for Catalog API"),
                new ApiScope("photo_stock_fullpermission", "full access for Photo Stock API"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "ASP.Net Core MVC",
                    ClientId = "WebMvcClient",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = { GrantType.ClientCredentials },
                    AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientName = "ASP.Net Core MVC",
                    ClientId = "WebMvcClientForUser",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email, 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles"
                    },
                    AccessTokenLifetime = 60 * 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int) (DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse
                }
            };
    }
}
