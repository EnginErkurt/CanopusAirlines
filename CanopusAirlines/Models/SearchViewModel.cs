using System.Collections.Generic;

namespace CanopusAirlines.Models
{
    public class SearchViewModel
    {
        // Gidiş Uçuşları
        public List<sp_SearchFlights_Result> OutboundFlights { get; set; }

        // Dönüş Uçuşları
        public List<sp_SearchFlights_Result> InboundFlights { get; set; }

        public bool IsRoundTrip { get; set; }
    }
}