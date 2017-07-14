using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace TestKarkov.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var symbolString = ApplyMarkov();
            ViewBag.symbolString = new JavaScriptSerializer().Serialize(ConvertToArray(symbolString));
            ViewBag.ListWords = JsonFile<string>(wordsFile);
            return View("Index", symbolString);
        }

    }
}