﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Models;
using Taxi.Web.Data.Entities;

namespace Taxi.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public TaxiResponse ToTaxiResponse(TaxiEntity taxi)
        {
            return new TaxiResponse
            {
                Id = taxi.Id,
                Plaque = taxi.Plaque,
                Trips = taxi.Trips?.Select(t => new TripResponse
                {
                   EndDate = t.EndDate,
                   Id = t.Id,
                   Qualification = t.Qualification,
                   Remarks = t.Remarks,
                   Source = t.Source,
                   SourceLatitude = t.SourceLatitude,
                   SourceLongitude = t.SourceLongitude,
                   StartDate = t.StartDate,
                   Target = t.Target,
                   TargetLatitude = t.TargetLatitude,
                   TargetLongitude = t.TargetLongitude,
                   TripDetails = t.TripDetails?.Select(td => new TripDetailsResponse
                   {
                       Date = td.Date,
                       Id = td.Id,
                       Latitude = td.Latitude,
                       Longitude = td.Longitude
                   }).ToList(),
                   User = ToUserResponse(t.User)
                }).ToList(),
                User = ToUserResponse(taxi.User)
            };
        }

        private UserResponse ToUserResponse(User user)
        {
            if (user == null)
            {
                return null;
            }
            return new UserResponse
            {
              Address = user.Address,
              Document = user.Document,
              Email = user.Email,
              FirstName = user.FirstName,
              Id = user.Id,
              LastName = user.LastName,
              PhoneNumber = user.PhoneNumber,
              PicturePath = user.PicturePath,
              UserType = user.UserType
            };
        }
    }
}