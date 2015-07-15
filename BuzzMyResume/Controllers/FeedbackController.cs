using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuzzMyResume.Controllers
{
    public class FeedbackController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Submit(String name, String email, String body)
        {
            return View();
        }
    }
}
