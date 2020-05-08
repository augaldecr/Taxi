namespace Taxi.Web.Data.Entities
{
    public class UserGroupDetailEntity
    {
        public int Id { get; set; }

        public User User { get; set; }

        public UserGroup UserGroup { get; set; }
    }
}
