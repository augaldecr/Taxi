using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Taxi.Common.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Taxi.Prism.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        private double _distance;
        private Position _position;

        public HomePage(IGeolocatorService geolocatorService)
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;
            _distance = .2;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MoveMapToCurrentPositionAsync();
        }

        private async void MoveMapToCurrentPositionAsync()
        {
            bool isLocationPermision = await CheckLocationPermisionsAsync();

            if (isLocationPermision)
            {
                MyMap.IsShowingUser = true;

                await _geolocatorService.GetLocationAsync();
                if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
                {
                    _position = new Position(_geolocatorService.Latitude, _geolocatorService.Longitude);
                    MoveMap();
                }
            }
        }

        private void MoveMap()
        {
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(_position, Distance.FromKilometers(_distance)));
        }

        private async Task<bool> CheckLocationPermisionsAsync()
        {
            PermissionStatus permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
            PermissionStatus permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationAlwaysPermission>();
            PermissionStatus permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationWhenInUsePermission>();
            bool isLocationEnabled = permissionLocation == PermissionStatus.Granted ||
                                     permissionLocationAlways == PermissionStatus.Granted ||
                                     permissionLocationWhenInUse == PermissionStatus.Granted;
            if (isLocationEnabled)
            {
                return true;
            }

            await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();

            permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
            permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationAlwaysPermission>();
            permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationWhenInUsePermission>();
            return permissionLocation == PermissionStatus.Granted ||
                   permissionLocationAlways == PermissionStatus.Granted ||
                   permissionLocationWhenInUse == PermissionStatus.Granted;
        }

        private void MySlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _distance = e.NewValue;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(_position, Distance.FromKilometers(_distance)));
        }
    }
}