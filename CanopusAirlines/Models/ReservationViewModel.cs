using System;

namespace CanopusAirlines.Models
{
    public class ReservationViewModel
    {
        public string PNR { get; set; }
        public string FlightNo { get; set; }

        // Kalkış Bilgileri
        public string FromCode { get; set; }
        public string FromCity { get; set; } 
        public DateTime DepTime { get; set; }

        // Varış Bilgileri
        public string ToCode { get; set; }
        public string ToCity { get; set; } 
        public DateTime ArrTime { get; set; }

        public string Seat { get; set; }
        public string PassengerName { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}