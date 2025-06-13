using Barbarossa.Models;
using System.Text.Json;

namespace Barbarossa.Services
{
    public interface IUserService
    {
        User CurrentUser { get; }
        bool IsAuthenticated { get; }
        Task InitializeAsync();
        Task<bool> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(User user, string password);
        Task UpdateProfileAsync(User updatedUser);
        Task LogoutAsync();
    }

    public class UserService : IUserService
    {
        private const string UserDataKey = "user_data";
        private readonly ISecureStorage _secureStorage;

        public User CurrentUser { get; private set; }
        public bool IsAuthenticated => CurrentUser != null;

        public UserService(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
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
            // 1. Валидация входных данных
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            // 2. Заглушка - создаем тестового пользователя
            CurrentUser = new User
            {
                Id = Guid.NewGuid(), // Уникальный идентификатор
                Name = "Владислав", // Пример имени
                Email = "vasekisov@gmail.com", // Используем введенный email
                Phone = "+79991234567", // Тестовый телефон
                CreatedAt = DateTime.UtcNow.AddDays(-10), // Дата регистрации
                LastLogin = DateTime.UtcNow // Текущая дата входа
            };

            // 3. Сохраняем данные в SecureStorage
            await SaveUserData();

            // 4. Логируем успешный вход (для отладки)
            Console.WriteLine($"User logged in: {CurrentUser}");

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

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            _secureStorage.Remove(UserDataKey); // Исправлено на Remove вместо RemoveAsync
        }

        private async Task SaveUserData()
        {
            var userData = JsonSerializer.Serialize(CurrentUser);
            await _secureStorage.SetAsync(UserDataKey, userData);
        }
    }
}