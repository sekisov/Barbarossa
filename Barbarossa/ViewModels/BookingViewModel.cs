using Barbarossa.Models;
using Barbarossa.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Barbarossa.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        private IEnumerable<Master> _allMasters = [];

        [ObservableProperty]
        private ObservableCollection<Service> _allServices = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowNextSteps))]
        [NotifyPropertyChangedFor(nameof(SelectedServicesText))]
        private ObservableCollection<Service> _selectedServices = [];

        [ObservableProperty]
        private Master? _selectedMaster;

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Today;

        [ObservableProperty]
        private string? _selectedTime;

        [ObservableProperty]
        private ObservableCollection<Master> _availableMasters = [];

        [ObservableProperty]
        private ObservableCollection<DateTime> _availableDates = [];

        [ObservableProperty]
        private ObservableCollection<string> _availableTimes = [];

        [ObservableProperty]
        private decimal _totalPrice;

        public bool ShowNextSteps => SelectedServices.Any();
        public string SelectedServicesText => SelectedServices.Any()
            ? $"Выбрано услуг: {SelectedServices.Count}"
            : "Выберите хотя бы одну услугу";

        public BookingViewModel(IApiService apiService)
        {
            _apiService = apiService;
            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            LoadDataCommand.Execute(null);
        }

        public IAsyncRelayCommand LoadDataCommand { get; }
        public IAsyncRelayCommand LoadDatesCommand => new AsyncRelayCommand(LoadDatesAsync);
        public IAsyncRelayCommand LoadTimesCommand => new AsyncRelayCommand(LoadTimesAsync);

        // Команда для обработки выбора элемента
        public ICommand ItemSelectedCommand => new Command<Service>(item =>
        {
            item.IsSelected = !item.IsSelected;
            
            UpdateSelectedServices();
            UpdateAvailableMasters();
        });


        private async Task LoadDataAsync()
        {
            try
            {
                _allMasters = await _apiService.GetMastersAsync();

                var services = _allMasters
                    .SelectMany(m => m.Services)
                    .DistinctBy(s => s.Id)
                    .OrderBy(s => s.Name);

                AllServices = [.. services];
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private void UpdateSelectedServices()
        {
            SelectedServices = [.. AllServices.Where(s => s.IsSelected)];
            CalculateTotal();
        }

        private void UpdateAvailableMasters()
        {
            if (SelectedServices.Any())
            {
                var selectedServiceIds = SelectedServices.Select(s => s.Id).ToList();
                AvailableMasters = [.. _allMasters.Where(master =>
                        selectedServiceIds.All(serviceId =>
                            master.Services.Any(s => s.Id == serviceId))
                    )];
            }
            else
            {
                AvailableMasters.Clear();
            }
        }

        private async Task LoadDatesAsync()
        {
            if (SelectedMaster == null) return;

            try
            {
                var dates = SelectedMaster.AvailableSlots.Keys
                    .Select(DateTime.Parse)
                    .Where(d => d >= DateTime.Today)
                    .OrderBy(d => d);

                AvailableDates = [.. dates];
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async Task LoadTimesAsync()
        {
            if (SelectedMaster == null || SelectedDate == default) return;

            try
            {
                var dateKey = SelectedDate.ToString("dd.MM.yyyy");
                if (SelectedMaster.AvailableSlots.TryGetValue(dateKey, out var times))
                {
                    AvailableTimes = [.. times.Distinct().OrderBy(t => t)];
                }
                else
                {
                    AvailableTimes.Clear();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private void CalculateTotal()
        {
            TotalPrice = SelectedServices.Sum(s => s.Price);
        }

        [RelayCommand]
        private async Task BookAppointment()
        {
            if (!SelectedServices.Any() || SelectedMaster == null ||
                SelectedDate == default || string.IsNullOrEmpty(SelectedTime))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            var confirmationMessage = BuildConfirmationMessage();
            await Shell.Current.DisplayAlert("Подтверждение брони", confirmationMessage, "OK");
        }

        private string BuildConfirmationMessage()
        {
            var sb = new StringBuilder()
                .AppendLine("Подтверждение бронирования")
                .AppendLine("----------------------------")
                .AppendLine($"Мастер: {SelectedMaster?.Name}")
                .AppendLine($"Дата: {SelectedDate:dd.MM.yyyy}")
                .AppendLine($"Время: {SelectedTime}")
                .AppendLine()
                .AppendLine("Выбранные услуги:");

            foreach (var service in SelectedServices)
            {
                sb.AppendLine($"- {service.Name} ({service.Price} руб.)");
            }

            sb.AppendLine()
              .AppendLine($"Итого: {TotalPrice} руб.");

            return sb.ToString();
        }
    }
}