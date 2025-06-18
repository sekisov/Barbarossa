using Barbarossa.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Barbarossa.Services
{
    public interface IUserService : INotifyPropertyChanged
    {
        User CurrentUser { get; }
        bool IsAuthenticated { get; }
        Task InitializeAsync();
        Task<bool> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(User user, string password);
        Task UpdateProfileAsync(User updatedUser);
        Task LogoutAsync();
    }

    public class UserService : IUserService, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string UserDataKey = "user_data";
        private readonly ISecureStorage _secureStorage;

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            private set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsAuthenticated));
                }
            }
        }

        public bool IsAuthenticated => CurrentUser != null;

        public UserService(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task InitializeAsync()
        {
            var userData = await _secureStorage.GetAsync(UserDataKey);
            if (!string.IsNullOrEmpty(userData))
            {
                CurrentUser = JsonSerializer.Deserialize<User>(userData);
                CurrentUser.LastLogin = DateTime.UtcNow;
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            CurrentUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Владислав",
                Email = "email@gmail.com",
                Phone = "+79991234567",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                LastLogin = DateTime.UtcNow
            };

            await SaveUserData();
            return true;
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            CurrentUser = user;
            CurrentUser.LastLogin = DateTime.UtcNow;
            await SaveUserData();
            return true;
        }

        public async Task UpdateProfileAsync(User updatedUser)
        {
            CurrentUser.Name = updatedUser.Name;
            CurrentUser.Phone = updatedUser.Phone;
            CurrentUser.Email = updatedUser.Email;
            await SaveUserData();
        }

        public Task LogoutAsync()
        {
            CurrentUser = null;
            _secureStorage.Remove(UserDataKey); // Используем синхронный Remove
            return Task.CompletedTask;
        }

        private async Task SaveUserData()
        {
            var userData = JsonSerializer.Serialize(CurrentUser);
            await _secureStorage.SetAsync(UserDataKey, userData);
        }
    }

}