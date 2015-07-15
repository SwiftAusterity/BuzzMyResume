using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuzzMyResume.Controllers
{
    public class ArchiveController : Controller
    {
        public ActionResult Index(String keywords)
        {
            return View();
        }

    }
}
