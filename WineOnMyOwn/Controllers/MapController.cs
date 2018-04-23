using System.Web.Mvc;
using System.Xml.Linq;
using WineOnMyOwn.Models;
using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace WineOnMyOwn.Controllers
{
    public class MapController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View(new SampleViewModel());
        }

        [HttpPost]
        public JsonResult GetAnswer(string question)
        {
            int index = _rnd.Next(_db.Count);
            var answer = _db[index];
            return Json(answer);
        }

        private static Random _rnd = new Random();
        private static List<string> _db = new List<string> { "Yes", "No", "Definitely, yes", "I don't know", "Looks like, yes" };
    }



    //public ActionResult GetRadiusMap()
    //    {
    //        winePermitsList = MapRepository.GetTTBWinePermits();
    //        return View("RadiusMap", winePermitsList);
    //    }

}
