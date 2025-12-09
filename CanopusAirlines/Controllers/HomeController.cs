using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CanopusAirlines.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult AirportServices()
        {
            return View();
        }

        public ActionResult Cancellation()
        {
            return View();
        }

        public ActionResult Baggage()
        {
            return View();
        }

        public ActionResult EuropeDestinations()
        {
            return View();
        }

        public ActionResult AsiaDestinations()
        {
            return View();
        }

        public ActionResult AmericaDestinations()
        {
            return View();
        }

    }
}