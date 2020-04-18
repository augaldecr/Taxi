using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Enums;

namespace Taxi.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Document")]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        [MaxLength(20, ErrorMessage = "The field {0} must have more than {1} characters")]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        [MaxLength(50, ErrorMessage = "The field {0} must have more than {1} characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        [MaxLength(50, ErrorMessage = "The field {0} must have more than {1} characters")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} must have more than {1} characters")]
        public string Address { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }

        [Display(Name = "User type")]
        public UserType UserType { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FullName} - {Document}";

        public ICollection<TaxiEntity> Taxis { get; set; }

        public ICollection<Trip> Trips { get; set; }
    }
}
