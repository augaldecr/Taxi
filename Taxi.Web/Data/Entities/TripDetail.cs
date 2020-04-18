using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.Web.Data.Entities
{
    public class TripDetail
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime DateLocal => Date.ToLocalTime();

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Trip Trip { get; set; }
    }
}
