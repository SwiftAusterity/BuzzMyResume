using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuzzMyResume.Controllers
{
    public class InterpretationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Submit(String description, String context)
        {
            //1. Find multiword phrases

            //2. Throw out junk words

            //3. Translate everything else

            //4. Submit for moderation everything that can not be translated

            //5. Archive request


            //99. Output results
            return View();
        }

    }
}
