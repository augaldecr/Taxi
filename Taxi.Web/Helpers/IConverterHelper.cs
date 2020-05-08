using System.Collections.Generic;
using Taxi.Common.Models;
using Taxi.Web.Data.Entities;

namespace Taxi.Web.Helpers
{
    public interface IConverterHelper
    {
        TaxiResponse ToTaxiResponse(TaxiEntity taxi);

        TripResponse ToTripResponse(Trip trip);

        UserResponse ToUserResponse(User user);

        List<TripResponseWithTaxi> ToTripResponse(List<Trip> tripEntities);

        List<UserGroupDetailResponse> ToUserGroupResponse(List<UserGroupDetailEntity> users);
    }
}
