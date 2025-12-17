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
        public ActionResult Search(int from_id, int to_id, DateTime flight_date, string flight_class, DateTime? return_date, string trip_type, int passenger_count = 1)
        {
            // 1. Hata Önlemleri
            if (string.IsNullOrEmpty(flight_class)) flight_class = "Economy";
            if (passenger_count < 1) passenger_count = 1;

            // 2. ViewModel Çantasını Hazırlıyoruz
            SearchViewModel model = new SearchViewModel();

            // 3. GİDİŞ UÇUŞLARINI BUL (Standart İşlem)
            model.OutboundFlights = db.sp_SearchFlights(from_id, to_id, flight_date, flight_class).ToList();

            // 4. DÖNÜŞ UÇUŞLARINI BUL (Eğer Round Trip seçildiyse)
            if (trip_type == "round" && return_date != null)
            {
                model.IsRoundTrip = true;

                // DİKKAT: Dönüşte Nereden ve Nereye yer değiştirir! (to_id -> from_id)
                model.InboundFlights = db.sp_SearchFlights(to_id, from_id, return_date.Value, flight_class).ToList();
            }
            else
            {
                model.IsRoundTrip = false;
                model.InboundFlights = new List<sp_SearchFlights_Result>(); // Boş liste
            }

            // Hesaplamalar için ViewBag kullanmaya devam
            ViewBag.PassengerCount = passenger_count;
            ViewBag.SelectedClass = flight_class;

            // Çantayı (modeli) sayfaya gönderiyoruz
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


        // POST: Seçilen uçuşları alıp ödeme/yolcu sayfasına yönlendirir
        // Search sayfasından "Continue" butonuna basınca buraya düşecek
        [HttpPost]
        public ActionResult PassengerDetails(int selected_outbound_id, int? selected_inbound_id, int passenger_count)
        {
            BookingViewModel model = new BookingViewModel();
            decimal totalPrice = 0;

            // 1. GİDİŞ UÇUŞUNU BUL VE FİYATI EKLE
            var outbound = db.Flights.Find(selected_outbound_id);
            model.OutboundFlight = outbound;
            model.OutboundFlightId = selected_outbound_id;

            if (outbound.price != null)
            {
                totalPrice += (decimal)outbound.price * passenger_count;
            }

            // 2. DÖNÜŞ UÇUŞU VARSA BUL VE FİYATI EKLE (DÜZELTME BURADA)
            if (selected_inbound_id != null)
            {
                var inbound = db.Flights.Find(selected_inbound_id);
                model.InboundFlight = inbound;
                model.InboundFlightId = selected_inbound_id;

                if (inbound.price != null)
                {
                    // Dönüş fiyatını da toplama ekliyoruz
                    totalPrice += (decimal)inbound.price * passenger_count;
                }
            }

            // 3. TOPLAM FİYATI MODELE YAZ
            model.TotalPrice = totalPrice;

            return View(model);
        }

        [HttpPost]
        public ActionResult SeatSelection(BookingViewModel model)
        {
            // Modeli tekrar doldurmamız lazım çünkü sayfalar arası sadece ID taşınır genelde
            // Ama HiddenFor kullandığımız için veriler modelde dolu gelecek.

            // Uçuş detayını tekrar çekelim (ekranda göstermek için)
            var flight = db.Flights.Find(model.OutboundFlightId);
            model.OutboundFlight = flight;

            return View(model);
        }

        [HttpPost]
        public ActionResult CompleteBooking(BookingViewModel model)
        {
            // 1. YOLCUYU KAYDET
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

            // --- YENİ EKLENEN KISIM (BAŞLANGIÇ) ---
            // Kullanıcı giriş yapmış mı kontrol ediyoruz.
            // Eğer yapmışsa ID'sini alıyoruz, yapmamışsa NULL (boş) bırakıyoruz.
            int? loggedInUserId = null;
            if (Session["UserID"] != null)
            {
                loggedInUserId = Convert.ToInt32(Session["UserID"]);
            }
            // --- YENİ EKLENEN KISIM (BİTİŞ) ---

            // --- TICKET VIEW MODEL HAZIRLIĞI ---
            TicketViewModel ticketModel = new TicketViewModel();
            ticketModel.PassengerName = model.FirstName.ToUpper() + " " + model.LastName.ToUpper();
            ticketModel.PnrCode = pnr;
            ticketModel.IssueDate = DateTime.Now;
            ticketModel.TotalAmount = model.TotalPrice;
            ticketModel.Flights = new List<FlightDetail>();

            // 2. GİDİŞ BİLETİNİ KAYDET
            Tickets ticketOut = new Tickets();
            ticketOut.flight_id = model.OutboundFlightId;
            ticketOut.passenger_id = newPassenger.passenger_id;
            ticketOut.seat_number = model.SelectedSeat;
            ticketOut.total_price = model.TotalPrice;
            ticketOut.pnr_code = pnr;
            ticketOut.booking_date = DateTime.Now;

            // --- YENİ EKLENEN KISIM: Kullanıcı ID'sini bilete yazıyoruz ---
            ticketOut.user_id = loggedInUserId;

            db.Tickets.Add(ticketOut);

            // --- HATA DÜZELTME KISMI (GİDİŞ) ---
            var outFlightDb = db.Flights.Find(model.OutboundFlightId);
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

            // 3. EĞER DÖNÜŞ VARSA KAYDET VE MODELE EKLE
            if (model.InboundFlightId != null)
            {
                Tickets ticketIn = new Tickets();
                ticketIn.flight_id = model.InboundFlightId.Value;
                ticketIn.passenger_id = newPassenger.passenger_id;
                ticketIn.seat_number = model.SelectedSeat;
                ticketIn.total_price = 0;
                ticketIn.pnr_code = pnr;
                ticketIn.booking_date = DateTime.Now;

                // --- YENİ EKLENEN KISIM: Dönüş biletine de Kullanıcı ID'sini yazıyoruz ---
                ticketIn.user_id = loggedInUserId;

                db.Tickets.Add(ticketIn);

                // --- HATA DÜZELTME KISMI (DÖNÜŞ) ---
                var inFlightDb = db.Flights.Find(model.InboundFlightId);
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

            // 4. TICKET SAYFASINI AÇ
            return View("TicketConfirmation", ticketModel);
        }

        // Yardımcı PNR Fonksiyonu
        private string GeneratePNR()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}