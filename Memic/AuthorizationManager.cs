using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Memic
{
    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            switch (context.Resource.First().Value)
            {
                case "ContactDetails":
                    return AuthorizeContactDetails(context);
                case "UpdateData":
                    return AuthorizeUpdataData(context);
                default:
                    return Nok();
            }
        }

        private Task<bool> AuthorizeContactDetails(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                case "Read":
                    return Eval(context.Principal.HasClaim("role", "User"));
                case "Write":
                    return Eval(context.Principal.HasClaim("role", "Manager"));
                default:
                    return Nok();
            }
        }
        private Task<bool> AuthorizeUpdataData(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                case "Read":
                    return Eval(context.Principal.HasClaim("role", "User"));
                case "Write":
                    return Eval(context.Principal.HasClaim("role", "Writer"));
                    //return Eval(context.Principal.HasClaim("role", "Manager"));
                default:
                    return Nok();
            }
        }
    }


}