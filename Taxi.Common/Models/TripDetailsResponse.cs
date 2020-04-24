using System;

namespace Taxi.Common.Models
{
    public class TripDetailsResponse
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateLocal => Date.ToLocalTime();

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
