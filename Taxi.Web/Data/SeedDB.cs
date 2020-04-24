using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Enums;
using Taxi.Web.Data.Entities;
using Taxi.Web.Helpers;

namespace Taxi.Web.Data
{
    public class SeedDB
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public SeedDB(DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();

            await CheckRolesAsync();

            var admin = await CheckUserAsync("701570344", "Alonso", "Ugalde", 
                "augaldecr@gmail.com", "85090266", "Coopevigua 2", UserType.Admin);

            var driver = await CheckUserAsync("1010", "Taxista", "Genérico",
                "taxista@gmail.com", "88888888", "Coopevigua 2", UserType.Driver);

            var user1 = await CheckUserAsync("2222", "Sharon", "Ugalde",
                "sharon@gmail.com", "11111111", "Coopevigua 2", UserType.User);

            var user2 = await CheckUserAsync("2223", "Brandon", "Ugalde",
                "brandon@gmail.com", "22222222", "Coopevigua 2", UserType.User);

            await CheckTaxisAsync(driver, user1, user2);
        }

        private async Task<User> CheckUserAsync(string document, string firstName,
            string lastName, string email, string phoneNumber, string address,
            UserType userType)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    UserType = userType,
                    UserName = email,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Driver.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckTaxisAsync(User driver, User user1, User user2)
        {
            if (_dataContext.Taxis.Any())
            {
                return;
            }

            await _dataContext.Taxis.AddAsync(new TaxiEntity
            {
                Plaque = "TLC777",
                User = driver,
                Trips = new List<Trip>
                   {
                       new Trip
                       {
                           StartDate = DateTime.UtcNow,
                           EndDate = DateTime.UtcNow.AddMinutes(30),
                           Qualification = 4.5f,
                           Source = "Hospital de Guápiles",
                           Target = "Universidad Latina",
                           Remarks = "Excelente servicio",
                           User = user1,
                       },
                       new Trip
                       {
                           StartDate = DateTime.UtcNow,
                           EndDate = DateTime.UtcNow.AddMinutes(30),
                           Qualification = 5f,
                           Source = "Universidad Latina",
                           Target = "Hospital de Guápiles",
                           Remarks = "Impecable servicio",
                           User = user1,
                       },
                   }
            });

            await _dataContext.Taxis.AddAsync(new TaxiEntity
            {
                Plaque = "TLC888",
                User = driver,
                Trips = new List<Trip>
                   {
                       new Trip
                       {
                           StartDate = DateTime.UtcNow,
                           EndDate = DateTime.UtcNow.AddMinutes(20),
                           Qualification = 4.5f,
                           Source = "Terminal de Guápiles",
                           Target = "Universidad San José",
                           Remarks = "Excelente servicio",
                           User = user2,
                       },
                       new Trip
                       {
                           StartDate = DateTime.UtcNow,
                           EndDate = DateTime.UtcNow.AddMinutes(20),
                           Qualification = 5f,
                           Source = "Universidad San José",
                           Target = "Terminal de Guápiles",
                           Remarks = "Impecable servicio",
                           User = user2,
                       },
                   }
            });

            await _dataContext.SaveChangesAsync();
        }
    }
}
