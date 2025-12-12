using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CanopusAirlines.Models;

namespace CanopusAirlines.Controllers
{
    public class HomeController : Controller
    {

        CanopusAirportsEntities db = new CanopusAirportsEntities();
        public ActionResult Index()
        {
            ViewBag.Airports = db.Airports.ToList();
            return View();
        }

        // 3. EKLEME: Arama butonuna basılınca çalışacak yeni metot
        public ActionResult Search(int from_id, int to_id, DateTime flight_date)
        {
            // Stored Procedure'ü çağırıyoruz
            var results = db.sp_SearchFlights(from_id, to_id, flight_date).ToList();

            // Sonuçları Search.cshtml sayfasına gönderiyoruz
            return View(results);
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