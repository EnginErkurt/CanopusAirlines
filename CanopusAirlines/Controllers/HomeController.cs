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
        public ActionResult Search(int from_id, int to_id, DateTime flight_date, string flight_class, int passenger_count = 1)
        {
            // Olası hatalara karşı güvenlik (Eğer sayı gelmezse 1 kabul et)
            if (string.IsNullOrEmpty(flight_class)) flight_class = "Economy";
            if (passenger_count < 1) passenger_count = 1;

            // 4. Parametre (flight_class) ile veriyi çekiyoruz
            var results = db.sp_SearchFlights(from_id, to_id, flight_date, flight_class).ToList();

            // --- YENİ KISIM ---
            // Yolcu sayısını sayfada hesaplama yapmak için ViewBag'e atıyoruz
            ViewBag.PassengerCount = passenger_count;

            // Seçilen sınıf bilgisini de gönderiyoruz (Opsiyonel)
            ViewBag.SelectedClass = flight_class;

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