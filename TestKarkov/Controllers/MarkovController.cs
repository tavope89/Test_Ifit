using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestKarkov.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestKarkov.Controllers
{
    public class MarkovController : BaseController
    {
        // GET: Markov
        public ActionResult Index()
        {
            var symbolString = JsonFile<string>(symbolStringFile);
            //ApplyMarkov();

            return View(symbolString);
        }

        public ActionResult ResultMarkov()
        {

            var symbolString = ApplyMarkov();

            return View("Index",symbolString);
        }

    }
}