using Barbarossa.Models;
using Barbarossa.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private bool _availableTimesEmpty;

        [ObservableProperty]
        private bool _availableTimesNotEmpty;

        [ObservableProperty]
        private Master? _selectedMaster;

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Today;

        [ObservableProperty]
        private AvailableSlot? _selectedTime;

        [ObservableProperty]
        private ObservableCollection<Master> _availableMasters = [];

        [ObservableProperty]
        private ObservableCollection<DateTime> _availableDates = [];

        [ObservableProperty]
        private ObservableCollection<AvailableSlot> _availableTimes = [];

        [ObservableProperty]
        private decimal _totalPrice;

        public bool IsNotBusy => !IsBusy;
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

        public ICommand ItemSelectedCommand => new Command<Service>(item =>
        {
            item.IsSelected = !item.IsSelected;
            UpdateSelectedServices();
            UpdateAvailableMasters();
        });

        partial void OnSelectedDateChanged(DateTime value)
        {
            _ = HandleDateChangeAsync(value);
        }

        partial void OnAvailableTimesChanged(ObservableCollection<AvailableSlot> value)
        {
            UpdateTimeSlotsVisibility();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsBusy = true;

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
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadDatesAsync()
        {
            if (SelectedMaster == null)
                return;

            try
            {
                IsBusy = true;

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
            finally
            {
                IsBusy = false;
            }
        }

        private async Task HandleDateChangeAsync(DateTime newDate)
        {
            if (SelectedMaster == null || SelectedDate == default)
                return;

            try
            {
                IsBusy = true;

                var dateKey = SelectedDate.ToString("dd.MM.yyyy");

                Debug.WriteLine($"Checking slots for date: {dateKey}");

                if (SelectedMaster.AvailableSlots.TryGetValue(dateKey, out var times))
                {
                    AvailableTimes = [.. times.Distinct().OrderBy(t => t.Time)];

                    Debug.WriteLine($"Found {AvailableTimes.Count} available times");
                }
                else
                {
                    AvailableTimes.Clear();
                    Debug.WriteLine("No available times found");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                UpdateTimeSlotsVisibility();
            }
        }

        private void UpdateTimeSlotsVisibility()
        {
            AvailableTimesEmpty = AvailableTimes.Count == 0;
            AvailableTimesNotEmpty = !AvailableTimesEmpty;

            Debug.WriteLine($"Time slots visibility - Empty: {AvailableTimesEmpty}, NotEmpty: {AvailableTimesNotEmpty}");
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
                            master.Services.Any(s => s.Id == serviceId)))];
            }
            else
            {
                AvailableMasters.Clear();
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
                SelectedDate == default || SelectedTime == null)
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    "Пожалуйста, заполните все поля", "OK");
                return;
            }

            var confirmationMessage = BuildConfirmationMessage();
            await Shell.Current.DisplayAlert("Подтверждение брони",
                confirmationMessage, "OK");
        }

        private string BuildConfirmationMessage()
        {
            var sb = new StringBuilder()
                .AppendLine("Подтверждение бронирования")
                .AppendLine("----------------------------")
                .AppendLine($"Мастер: {SelectedMaster?.Name}")
                .AppendLine($"Дата: {SelectedDate:dd.MM.yyyy}")
                .AppendLine($"Время: {SelectedTime?.Time}")
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

        public ICommand ShowDurationInfoCommand => new Command<string>(async (duration) =>
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FFD0C2"),
                TextColor = Color.FromArgb("#4A2E27"),
                CornerRadius = new CornerRadius(10),
                Font = Microsoft.Maui.Font.SystemFontOfSize(14)
            };

            await Shell.Current.DisplaySnackbar(
                message: $"Продолжительность выполнения услуги: {duration}",
                actionButtonText: "OK",
                duration: TimeSpan.FromSeconds(3),
                visualOptions: snackbarOptions);
        });
    }
}