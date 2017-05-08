using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Memic.Config
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
        {
            new InMemoryUser
            {
                Username = "Larry",
                Password = "secret",
                Subject = "1",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Lawrence"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Thompson"),
                    new Claim(Constants.ClaimTypes.Role, "Manager"),
                    new Claim(Constants.ClaimTypes.Role, "User")                  
                }
            }
        };
        }
    }


}