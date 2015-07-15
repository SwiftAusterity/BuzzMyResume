using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuzzMyResume.Controllers
{
    public class CurationController : Controller
    {
        public ActionResult Synonyms()
        {
            return View();
        }

        public ActionResult RejectedWords()
        {
            return View();
        }

        public ActionResult Moderation()
        {
            return View();
        }

        //Actions
        public ActionResult CreateSynonym(String keyword, String description)
        {
            return View();
        }

        public ActionResult CreateRejectedWord(String keyword, String description)
        {
            return View();
        }

        public ActionResult RemoveSynonym(Guid id)
        {
            return View();
        }

        public ActionResult RemoveRejectedWord(Guid id)
        {
            return View();
        }

        public ActionResult EditSynonym(Guid id, String description)
        {
            return View();
        }

    }
}
