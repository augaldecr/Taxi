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

            User user3 = await CheckUserAsync("6060", "Sandra", "Usuga",
                "sandra@yopmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            User user4 = await CheckUserAsync("7070", "Lisa", "Marin",
                "luisa@yopmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);


            await CheckTaxisAsync(driver, user1, user2);
            await CheckUserGroups(user1, user2, user3, user4);
        }

        private async Task<User> CheckUserAsync(string document, string firstName,
            string lastName, string email, string phoneNumber, string address,
            UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);

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

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
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
                           Remarks = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus varius turpis lectus, non euismod quam fermentum eu. Quisque eu lorem nec quam egestas condimentum. Praesent turpis metus, porta nec aliquet id, laoreet ac sapien. Mauris sit amet neque dolor. Nullam id varius enim. Integer tempus bibendum facilisis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Pellentesque nunc tortor, facilisis vel lorem et, porttitor congue lorem.",
                           User = user1,
                       },
                       new Trip
                       {
                           StartDate = DateTime.UtcNow,
                           EndDate = DateTime.UtcNow.AddMinutes(30),
                           Qualification = 5f,
                           Source = "Universidad Latina",
                           Target = "Hospital de Guápiles",
                           Remarks = "Sed erat nibh, faucibus tincidunt commodo et, varius in diam. Sed dui elit, blandit in ullamcorper ut, ullamcorper a elit. Sed et sapien a nunc cursus pretium. Sed efficitur risus et nisl convallis mattis. Sed euismod, erat sit amet feugiat venenatis, ipsum diam volutpat leo, quis luctus dolor arcu eget nisi. Morbi volutpat nibh nec auctor ullamcorper. Fusce congue eu ligula id dictum. Proin posuere, nisi a auctor auctor, odio enim tristique diam, eget molestie mauris nisi ullamcorper risus. Vivamus sed nibh bibendum, ullamcorper odio vitae, fermentum nulla.",
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

        private async Task CheckUserGroups(User user1, User user2, User user3, User user4)
        {
            if (!_dataContext.UserGroups.Any())
            {
                _dataContext.UserGroups.Add(new UserGroup
                {
                    User = user1,
                    Users = new List<UserGroupDetailEntity>
            {
                new UserGroupDetailEntity { User = user2 },
                new UserGroupDetailEntity { User = user3 },
                new UserGroupDetailEntity { User = user4 }
            }
                });

                _dataContext.UserGroups.Add(new UserGroup
                {
                    User = user2,
                    Users = new List<UserGroupDetailEntity>
            {
                new UserGroupDetailEntity { User = user1 },
                new UserGroupDetailEntity { User = user3 },
                new UserGroupDetailEntity { User = user4 }
            }
                });

                await _dataContext.SaveChangesAsync();
            }

        }
    }
}
