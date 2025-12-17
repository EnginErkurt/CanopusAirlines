using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CanopusAirlines.Models;

namespace CanopusAirlines.Controllers
{
    public class AdminController : Controller
    {
        CanopusAirportsEntities db = new CanopusAirportsEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            // SADECE 'Admin' ROLÜNE SAHİP KULLANICIYI GETİR
            var user = db.Users.FirstOrDefault(u => u.Email == username && u.Password == password);

            if (user != null)
            {
                // EKSTRA GÜVENLİK KONTROLÜ
                if (user.Role == "Admin")
                {
                    return Json(new { success = true, name = user.FirstName + " " + user.LastName, role = user.Role });
                }
                else
                {
                    // Kullanıcı var ama Admin değilse hata ver
                    return Json(new { success = false, message = "Unauthorized access! Only administrators may enter." });
                }
            }
            else
            {
                return Json(new { success = false, message = "The username or password is incorrect." });
            }
        }

        // --- HATA VEREN KISIM BURASIYDI, MANUEL JOIN İLE DÜZELTİLDİ ---
        [HttpGet]
        public JsonResult GetAllData()
        {
            // A) UÇUŞLARI ÇEKERKEN HAVALİMANLARINI MANUEL BAĞLIYORUZ (JOIN)
            var flightsQuery = from f in db.Flights
                               join dep in db.Airports on f.departure_id equals dep.airport_id
                               join arr in db.Airports on f.arrival_id equals arr.airport_id
                               select new
                               {
                                   f.flight_number,
                                   f.flight_date,
                                   f.price,
                                   f.capacity,
                                   f.status,
                                   DepCity = dep.city,
                                   DepCode = dep.iata,
                                   ArrCity = arr.city,
                                   ArrCode = arr.iata
                               };

            var flights = flightsQuery.ToList().Select(f => new {
                no = f.flight_number,
                from = f.DepCity + " (" + f.DepCode + ")",
                to = f.ArrCity + " (" + f.ArrCode + ")",
                date = f.flight_date.HasValue ? f.flight_date.Value.ToString("yyyy-MM-dd HH:mm") : "",
                price = f.price,
                seats = f.capacity ?? 150,
                status = f.status ?? "On Time"
            }).ToList();

            // B) BİLETLERİ ÇEKERKEN DE MANUEL BAĞLIYORUZ
            var bookingsQuery = from t in db.Tickets
                                join p in db.Passengers on t.passenger_id equals p.passenger_id
                                join f in db.Flights on t.flight_id equals f.flight_id
                                select new
                                {
                                    t.pnr_code,
                                    t.seat_number,
                                    PassengerName = p.first_name + " " + p.last_name,
                                    FlightNo = f.flight_number
                                };

            var bookings = bookingsQuery.ToList().Select(t => new {
                pnr = t.pnr_code,
                name = t.PassengerName,
                flight = t.FlightNo,
                seat = t.seat_number,
                flight_class = "Economy"
            }).ToList();

            return Json(new { flights = flights, bookings = bookings }, JsonRequestBehavior.AllowGet);
        }

        // 4. YENİ UÇUŞ EKLEME
        [HttpPost]
        public JsonResult AddFlight(string no, string fromIata, string toIata, decimal price, int seats, string date, string status)
        {
            try
            {
                Flights f = new Flights();
                f.flight_number = no;

                var depAirport = db.Airports.FirstOrDefault(a => a.iata == fromIata);
                var arrAirport = db.Airports.FirstOrDefault(a => a.iata == toIata);

                if (depAirport == null || arrAirport == null)
                    return Json(new { success = false, message = "Havalimanı kodu hatalı! (Örn: IST, ESB)" });

                f.departure_id = depAirport.airport_id;
                f.arrival_id = arrAirport.airport_id;
                f.price = price;
                f.capacity = seats;
                f.available_seats = seats;
                f.flight_date = DateTime.Parse(date);
                f.status = status;
                f.price_business = price * 1.5m;

                db.Flights.Add(f);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // 5. UÇUŞ SİLME
        [HttpPost]
        public JsonResult DeleteFlight(string flightNo)
        {
            var flight = db.Flights.FirstOrDefault(f => f.flight_number == flightNo);
            if (flight != null)
            {
                var tickets = db.Tickets.Where(t => t.flight_id == flight.flight_id).ToList();
                db.Tickets.RemoveRange(tickets);
                db.Flights.Remove(flight);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // 6. UÇUŞ GÜNCELLEME
        [HttpPost]
        public JsonResult EditFlight(string no, string from, string to, string date, string status)
        {
            var flight = db.Flights.FirstOrDefault(f => f.flight_number == no);
            if (flight != null)
            {
                flight.status = status;
                flight.flight_date = DateTime.Parse(date);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // 7. BİLET İPTAL
        [HttpPost]
        public JsonResult CancelTicket(string pnr)
        {
            var ticket = db.Tickets.FirstOrDefault(t => t.pnr_code == pnr);
            if (ticket != null)
            {
                db.Tickets.Remove(ticket);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}