using System.Collections.Generic;

namespace CanopusAirlines.Models
{
    public class SearchViewModel
    {
        // Gidiş Uçuşlarının Listesi
        public List<sp_SearchFlights_Result> OutboundFlights { get; set; }

        // Dönüş Uçuşlarının Listesi
        public List<sp_SearchFlights_Result> InboundFlights { get; set; }

        // Gidiş-Dönüş mü seçildi?
        public bool IsRoundTrip { get; set; }
    }
}