using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Mvc;

namespace Memic.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Identity()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        [ResourceAuthorize("Read", "ContactDetails")]
        [HandleForbidden]
        public ActionResult Contact()
        {
            ViewBag.Message = "My Contact Information:";

            return View();
        }
        [ResourceAuthorize("Write", "UpdateData")]
        [HandleForbidden]
        public ActionResult UpdateData()
        {

            //if (!HttpContext.CheckAccess("Write", "UpdateData", "more data"))
            //{
            //    // either 401 or 403 based on authentication state
            //    return this.AccessDenied();
            //}

            ViewBag.Message = "Allowed to Updata data";

            return View();
        }
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }

    }
}