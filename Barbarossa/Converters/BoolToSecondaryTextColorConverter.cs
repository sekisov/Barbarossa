using System.Globalization;

namespace Barbarossa.Converters
{
    /// <summary>
    /// Конвертер для изменения вторичного цвета текста (длительность, доп. информация)
    /// </summary>
    public class BoolToSecondaryTextColorConverter : IValueConverter
    {
        // Цвет когда элемент выбран
        public Color TrueColor { get; set; } = Color.FromArgb("#FFD0C2"); // Светло-синий

        // Цвет когда элемент не выбран
        public Color FalseColor { get; set; } = Color.FromArgb("#694237"); // Средне-коричневый

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? TrueColor : FalseColor;
            }
            return FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return color == TrueColor;
            }
            return false;
        }
    }
}
