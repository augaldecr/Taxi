using System;
using Taxi.Common.Enums;

namespace Taxi.Web.Data.Entities
{
    public class UserGroupRequestEntity
    {
        public int Id { get; set; }

        public User ProposalUser { get; set; }

        public User RequiredUser { get; set; }

        public UserGroupStatus Status { get; set; }

        public Guid Token { get; set; }
    }
}