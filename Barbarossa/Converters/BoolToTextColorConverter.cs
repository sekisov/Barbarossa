using System.Globalization;

namespace Barbarossa.Converters
{
    /// <summary>
    /// Конвертер для изменения цвета основного текста при выборе
    /// </summary>
    public class BoolToTextColorConverter : IValueConverter
    {
        // Цвет когда элемент выбран (активный)
        public Color TrueColor { get; set; } = Color.FromArgb("#4A2E27"); // Ярко-синий

        // Цвет когда элемент не выбран (неактивный)
        public Color FalseColor { get; set; } = Color.FromArgb("#4A2E27"); // Тёмно-коричневый

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? TrueColor : FalseColor;
            }
            return FalseColor; // По умолчанию
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
