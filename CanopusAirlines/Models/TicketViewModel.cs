using System;
using System.Collections.Generic;

namespace CanopusAirlines.Models
{
    public class TicketViewModel
    {
        public string PassengerName { get; set; }
        public string PnrCode { get; set; } 
        public DateTime IssueDate { get; set; }
        public decimal TotalAmount { get; set; }

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