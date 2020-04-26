using Prism.Commands;
using Prism.Navigation;
using System;
using System.Text.RegularExpressions;
using Taxi.Common.Models;
using Taxi.Common.Services;

namespace Taxi.Prism.ViewModels
{
    public class TaxiHistoryPageViewModel : ViewModelBase
    {
        private readonly IAPIService _aPIService;
        private TaxiResponse _taxi;
        private bool _isRunning;
        private DelegateCommand _checkPlaqueCommand;

        public TaxiHistoryPageViewModel(INavigationService navigationService,
            IAPIService aPIService)
            : base(navigationService)
        {
            Title = "Taxi history";
            _aPIService = aPIService;
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public TaxiResponse Taxi
        {
            get => _taxi;
            set => SetProperty(ref _taxi, value);
        }

        public string Plaque { get; set; }

        public DelegateCommand CheckPlaqueCommand => _checkPlaqueCommand ?? (_checkPlaqueCommand = new DelegateCommand(CheckPlaqueAsync));

        private async void CheckPlaqueAsync()
        {
            if (string.IsNullOrEmpty(Plaque))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a plaque",
                    "OK"
                    );
                return;
            }

            Regex regex = new Regex(@"^([A-Za-z]{3}\d{3})$");

            if (!regex.IsMatch(Plaque))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "The plaque must start with three letters and end with three numbers.",
                    "Accept");
                return;
            }

            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _aPIService.GetTaxiAsync(Plaque, url, "api", "/Taxis");
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Taxi = (TaxiResponse)response.Result;
        }
    }
}