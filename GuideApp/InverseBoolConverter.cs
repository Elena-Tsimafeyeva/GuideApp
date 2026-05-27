using System.Globalization;

namespace GuideApp
{
    // Конвертер для инвертирования логического значения
    // Меняет true на false и false на true
    // Используется в XAML при Binding
    public class InverseBoolConverter  : IValueConverter
    {
        // Метод вызывается при передаче данных
        // из ViewModel в интерфейс (UI)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            // Метод инвертирует логическое значение:
            // true -> false
            // false -> true
            => !(bool)value;
        // Метод не используется, но IValueConverter требует реализовать оба метода
        // из интерфейса обратно во ViewModel
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            // Также инвертируем значение
            => !(bool)value;
    }
}
