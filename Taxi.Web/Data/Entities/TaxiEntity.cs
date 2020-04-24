using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taxi.Web.Data.Entities
{
    public class TaxiEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory")]
        //[RegularExpression(@"^([A-Aa-a]{2}\d{3})$", ErrorMessage = "The field {0} must have a valid format")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "The field {0} must have more than {1} characters")]
        public string Plaque { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

        public User User { get; set; }
    }
}
