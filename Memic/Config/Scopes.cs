using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Memic.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
        {
            new Scope
            {
                Enabled = true,
                Name = "roles",
                Type = ScopeType.Identity,
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim("role")
                }
            },
            
            new Scope
            {
                Enabled = true,
                DisplayName = "Memic API",
                Name = "memicApi",
                Description = "Access to the Memic API",
                Type = ScopeType.Resource
            }
        };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }




}