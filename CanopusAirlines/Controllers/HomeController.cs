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

        
        public ActionResult Search(int from_id, int to_id, DateTime flight_date, string flight_class, DateTime? return_date, string trip_type, int passenger_count = 1)
        {
           
            if (string.IsNullOrEmpty(flight_class)) flight_class = "Economy";
            if (passenger_count < 1) passenger_count = 1;

           
            SearchViewModel model = new SearchViewModel();

           
            model.OutboundFlights = db.sp_SearchFlights(from_id, to_id, flight_date, flight_class).ToList();

           
            if (trip_type == "round" && return_date != null)
            {
                model.IsRoundTrip = true;

               
                model.InboundFlights = db.sp_SearchFlights(to_id, from_id, return_date.Value, flight_class).ToList();
            }
            else
            {
                model.IsRoundTrip = false;
                model.InboundFlights = new List<sp_SearchFlights_Result>(); 
            }

            
            ViewBag.PassengerCount = passenger_count;
            ViewBag.SelectedClass = flight_class;

            
            return View(model);
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


        
        [HttpPost]
        public ActionResult PassengerDetails(int selected_outbound_id, int? selected_inbound_id, int passenger_count)
        {
            BookingViewModel model = new BookingViewModel();
            decimal totalPrice = 0;

            
            var outbound = db.Flights.Find(selected_outbound_id);
            model.OutboundFlight = outbound;
            model.OutboundFlightId = selected_outbound_id;

            if (outbound.price != null)
            {
                totalPrice += (decimal)outbound.price * passenger_count;
            }

            
            if (selected_inbound_id != null)
            {
                var inbound = db.Flights.Find(selected_inbound_id);
                model.InboundFlight = inbound;
                model.InboundFlightId = selected_inbound_id;

                if (inbound.price != null)
                {
                    
                    totalPrice += (decimal)inbound.price * passenger_count;
                }
            }

            
            model.TotalPrice = totalPrice;

            return View(model);
        }

        [HttpPost]
        public ActionResult SeatSelection(BookingViewModel model)
        {
            
            var flight = db.Flights.Find(model.OutboundFlightId);
            model.OutboundFlight = flight;

            
            var depAirport = db.Airports.Find(flight.departure_id);
            var arrAirport = db.Airports.Find(flight.arrival_id);

            
            model.FromCity = depAirport.city; 
            model.ToCity = arrAirport.city;   

            return View(model);
        }

        [HttpPost]
        public ActionResult CompleteBooking(BookingViewModel model)
        {
            
            Passengers newPassenger = new Passengers();
            newPassenger.first_name = model.FirstName;
            newPassenger.last_name = model.LastName;
            newPassenger.email = model.Email;
            newPassenger.phone = model.Phone;
            newPassenger.gender = model.Gender;
            newPassenger.birth_date = model.BirthDate;

            db.Passengers.Add(newPassenger);
            db.SaveChanges();

            string pnr = GeneratePNR();

            
            int? loggedInUserId = null;
            if (Session["UserID"] != null)
            {
                loggedInUserId = Convert.ToInt32(Session["UserID"]);
            }

            
            TicketViewModel ticketModel = new TicketViewModel();
            ticketModel.PassengerName = model.FirstName.ToUpper() + " " + model.LastName.ToUpper();
            ticketModel.PnrCode = pnr;
            ticketModel.IssueDate = DateTime.Now;
            ticketModel.TotalAmount = model.TotalPrice;
            ticketModel.Flights = new List<FlightDetail>();

            
            Tickets ticketOut = new Tickets();
            ticketOut.flight_id = model.OutboundFlightId;
            ticketOut.passenger_id = newPassenger.passenger_id;
            ticketOut.seat_number = model.SelectedSeat;
            ticketOut.total_price = model.TotalPrice;
            ticketOut.pnr_code = pnr;
            ticketOut.booking_date = DateTime.Now;

            
            ticketOut.user_id = loggedInUserId;

            db.Tickets.Add(ticketOut);

            
            var outFlightDb = db.Flights.Find(model.OutboundFlightId);

            
            if (outFlightDb.available_seats > 0)
            {
                outFlightDb.available_seats -= 1;
            }
            

            var outDepAirport = db.Airports.Find(outFlightDb.departure_id);
            var outArrAirport = db.Airports.Find(outFlightDb.arrival_id);

            ticketModel.Flights.Add(new FlightDetail
            {
                From = outDepAirport.iata,
                To = outArrAirport.iata,
                FlightNo = outFlightDb.flight_number,
                Seat = model.SelectedSeat,
                Date = (DateTime)outFlightDb.flight_date
            });

            
            if (model.InboundFlightId != null)
            {
                Tickets ticketIn = new Tickets();
                ticketIn.flight_id = model.InboundFlightId.Value;
                ticketIn.passenger_id = newPassenger.passenger_id;
                ticketIn.seat_number = model.SelectedSeat;
                ticketIn.total_price = 0;
                ticketIn.pnr_code = pnr;
                ticketIn.booking_date = DateTime.Now;

                
                ticketIn.user_id = loggedInUserId;

                db.Tickets.Add(ticketIn);

                
                var inFlightDb = db.Flights.Find(model.InboundFlightId);

                
                if (inFlightDb.available_seats > 0)
                {
                    inFlightDb.available_seats -= 1;
                }
                

                var inDepAirport = db.Airports.Find(inFlightDb.departure_id);
                var inArrAirport = db.Airports.Find(inFlightDb.arrival_id);

                ticketModel.Flights.Add(new FlightDetail
                {
                    From = inDepAirport.iata,
                    To = inArrAirport.iata,
                    FlightNo = inFlightDb.flight_number,
                    Seat = model.SelectedSeat,
                    Date = (DateTime)inFlightDb.flight_date
                });
            }

            db.SaveChanges(); 

          
            return View("TicketConfirmation", ticketModel);
        }

        public ActionResult CheckIn()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult GetTicketCheckIn(string pnr, string lastname)
        {
           
            var ticket = db.Tickets.FirstOrDefault(t =>
                t.pnr_code == pnr &&
                t.Passengers.last_name.ToLower() == lastname.ToLower());

            if (ticket != null)
            {
                
                var flight = db.Flights.Find(ticket.flight_id);
                var depAirport = db.Airports.Find(flight.departure_id);
                var arrAirport = db.Airports.Find(flight.arrival_id);

                
                var result = new
                {
                    success = true,
                    passengerName = ticket.Passengers.first_name.ToUpper() + " " + ticket.Passengers.last_name.ToUpper(),
                    flightNo = flight.flight_number,
                    originCode = depAirport.iata,     
                    originCity = depAirport.city,     
                    destCode = arrAirport.iata,       
                    destCity = arrAirport.city,       
                    date = flight.flight_date.Value.ToString("dd MMM yyyy"), 
                    time = flight.flight_date.Value.ToString("HH:mm"),     
                    seat = ticket.seat_number,
                    gate = "A" + new Random().Next(1, 20) 
                };

                return Json(result);
            }
            else
            {
                return Json(new { success = false, message = "Bilet bulunamadı! PNR kodunu ve soyadınızı kontrol edin." });
            }
        }

        
        public ActionResult MyReservations()
        {
            
            if (Session["UserID"] == null)
            {
                
                return RedirectToAction("Login", "Auth");
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            
            var userTickets = db.Tickets.Where(t => t.user_id == userId).ToList();

            List<ReservationViewModel> model = new List<ReservationViewModel>();

            
            foreach (var t in userTickets)
            {
                
                var flight = db.Flights.Find(t.flight_id);
                var depAirport = db.Airports.Find(flight.departure_id);
                var arrAirport = db.Airports.Find(flight.arrival_id);
                var passenger = db.Passengers.Find(t.passenger_id);

                ReservationViewModel vm = new ReservationViewModel();
                vm.PNR = t.pnr_code;
                vm.FlightNo = flight.flight_number;
                vm.FromCode = depAirport.iata;
                vm.FromCity = depAirport.city;
                vm.ToCode = arrAirport.iata;
                vm.ToCity = arrAirport.city;
                vm.DepTime = (DateTime)flight.flight_date;

                
                vm.ArrTime = vm.DepTime.AddHours(3);

                vm.Seat = t.seat_number;
                vm.PassengerName = passenger.first_name + " " + passenger.last_name;
                vm.Price = (decimal)t.total_price;

                
                if (vm.DepTime < DateTime.Now)
                    vm.Status = "Completed";
                else
                    vm.Status = "Upcoming";

                model.Add(vm);
            }

            
            return View(model.OrderByDescending(x => x.DepTime).ToList());
        }

        
        private string GeneratePNR()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}