using System.Globalization;
using System.Windows.Input;

namespace Barbarossa.Converters
{
    public class BoolToCommandConverter : IValueConverter
    {
        public ICommand TrueCommand { get; set; }
        public ICommand FalseCommand { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && b ? TrueCommand : FalseCommand;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}