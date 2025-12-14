using System;
using System.Collections.Generic;

namespace CanopusAirlines.Models
{
    public class TicketViewModel
    {
        // Bilet Başlık Bilgileri
        public string PassengerName { get; set; }
        public string PnrCode { get; set; } // Ticket Number yerine PNR kullanacağız
        public DateTime IssueDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Uçuş Detayları (Gidiş ve varsa Dönüş)
        public List<FlightDetail> Flights { get; set; }
    }

    public class FlightDetail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string FlightNo { get; set; }
        public string Seat { get; set; }
        public DateTime Date { get; set; }
    }
}