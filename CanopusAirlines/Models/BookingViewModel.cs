using System;

namespace CanopusAirlines.Models
{
    public class BookingViewModel
    {
        // --- 1. Gidiş Uçuş Bilgileri ---
        public Flights OutboundFlight { get; set; }
        public int OutboundFlightId { get; set; }

        // --- YENİ EKLENEN KISIM: Dönüş Uçuş Bilgileri ---
        // Soru işareti (?) koyduk çünkü Tek Yön (One Way) seçilirse burası boş kalacak.
        public Flights InboundFlight { get; set; }
        public int? InboundFlightId { get; set; }
        // ------------------------------------------------

        // --- 2. Yolcu Bilgileri ---
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // --- 3. Koltuk ve Fiyat Bilgisi ---
        public string SelectedSeat { get; set; }
        public decimal TotalPrice { get; set; }

        public string FromCity { get; set; }
        public string ToCity { get; set; }
    }
}