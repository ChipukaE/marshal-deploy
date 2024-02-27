using marshal_deploy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace marshal_deploy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int activePrecinctsCount = GetActivePrecinctsCount();
            double activePrecinctsPercentage = CalculateActivePrecinctsPercentage();

            ViewBag.ActivePrecinctsCount = activePrecinctsCount;
            ViewBag.ActivePrecinctsPercentage = activePrecinctsPercentage;
            return View();
        }

        //get active precincts
        private int GetActivePrecinctsCount()
        {
            using (var db = new Deploy()) 
            {
                return db.Precincts.Count(p => (bool)p.IsActive);
            }
        }

        //active precincts percentage
        private double CalculateActivePrecinctsPercentage()
        {
            using (var db = new Deploy())
            {
                int totalPrecincts = db.Precincts.Count();
                int activePrecincts = db.Precincts.Count(p => (bool)p.IsActive);

                if (totalPrecincts == 0)
                {
                    return 0;
                }

                double percentage = (double)activePrecincts / totalPrecincts * 100;
                return Math.Round(percentage, 1);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}