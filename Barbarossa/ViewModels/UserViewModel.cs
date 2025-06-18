using Barbarossa.Models;
using Barbarossa.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Barbarossa.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WelcomeMessage))]
        private bool _isAuthenticated;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _phone;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        public string WelcomeMessage => IsAuthenticated ? $"Добро пожаловать, {Name}!" : "Пожалуйста, войдите";

        public UserViewModel(IUserService userService)
        {
            _userService = userService;
            _userService.PropertyChanged += OnUserServicePropertyChanged;
            Initialize();
        }

        private void OnUserServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUserService.CurrentUser))
                UpdateUserData();
        }

        private async void Initialize()
        {
            await _userService.InitializeAsync();
            UpdateUserData();
        }

        public void UpdateUserData()
        {
            CurrentUser = _userService.CurrentUser;
            IsAuthenticated = _userService.IsAuthenticated;

            if (CurrentUser != null)
            {
                Name = CurrentUser.Name;
                Phone = CurrentUser.Phone;
                Email = CurrentUser.Email;
            }
            else
            {
                Name = string.Empty;
                Phone = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
            }
        }

        [RelayCommand]
        private async Task Login()
        {
            if (await _userService.LoginAsync(Email, Password))
            {
                UpdateUserData();
                await Shell.Current.DisplayAlert("Успех", "Вход выполнен", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", "Неверные данные", "OK");
            }
        }

        [RelayCommand]
        private async Task Register()
        {
            var newUser = new User
            {
                Name = Name,
                Phone = Phone,
                Email = Email
            };

            if (await _userService.RegisterAsync(newUser, Password))
            {
                UpdateUserData();
                await Shell.Current.DisplayAlert("Успех", "Регистрация завершена", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", "Регистрация не удалась", "OK");
            }
        }

        [RelayCommand]
        private async Task UpdateProfile()
        {
            var updatedUser = new User
            {
                Name = Name,
                Phone = Phone,
                Email = Email
            };

            await _userService.UpdateProfileAsync(updatedUser);
            UpdateUserData();
            await Shell.Current.DisplayAlert("Успех", "Профиль обновлен", "OK");
        }

        [RelayCommand]
        private async Task Logout()
        {
            await _userService.LogoutAsync();
            UpdateUserData();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}