using System.Globalization;

namespace Barbarossa.Converters
{
    /// <summary>
    /// Конвертер bool в Color с настраиваемыми цветами для true/false состояний
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        // Цвет для true значения (можно задать через XAML)
        public Color TrueColor { get; set; } = Color.FromArgb("#FFD0C2"); // Светло-голубой по умолчанию

        // Цвет для false значения (можно задать через XAML)
        public Color FalseColor { get; set; } = Colors.Transparent; // Прозрачный по умолчанию

        /// <summary>
        /// Преобразует bool значение в Color
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем входное значение
            if (value is bool isSelected)
            {
                return isSelected ? TrueColor : FalseColor;
            }

            // Если значение не bool, возвращаем цвет по умолчанию (FalseColor)
            return FalseColor;
        }

        /// <summary>
        /// Обратное преобразование (обычно не используется)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если нужно преобразовывать обратно (например, для TwoWay binding)
            if (value is Color color)
            {
                return color == TrueColor;
            }

            return false;
        }
    }
}