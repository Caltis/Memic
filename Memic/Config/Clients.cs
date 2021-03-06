﻿using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memic.Config
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
            new Client
            {
                Enabled = true,
                ClientName = "MVC Client",
                ClientId = "mvc",
                Flow = Flows.Implicit,

                RedirectUris = new List<string>
                {
                    IdentityServerConfig.Endpoint.IdentityServerBaseUri
                    
                },
                PostLogoutRedirectUris = new List<string>
                {
                    IdentityServerConfig.Endpoint.IdentityServerBaseUri
                    
                },

                AllowAccessToAllScopes = true
            },
            new Client
            {
                ClientName = "MVC Client (service communication)",
                ClientId = "mvc_service",
                Flow = Flows.ClientCredentials,

                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = new List<string>
                {
                    "memicApi"
                }
            }

        };
        }
    }


}