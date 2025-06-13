using System.Globalization;
namespace Barbarossa.Converters;
public class AuthButtonTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "Вы авторизованы" : "Войти/Зарегистрироваться";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}