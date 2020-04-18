using System.Collections.Generic;

namespace Taxi.Web.Data.Entities
{
    public class UserGroup
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
