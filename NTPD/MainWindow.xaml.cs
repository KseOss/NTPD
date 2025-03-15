using System.Windows; // Для работы с WPF (Window, RoutedEventArgs)
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media; // Для работы с командами (ICommand)

namespace NTPD;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    NTPD_CS notepad; // Экземпляр класса блокнота
    public MainWindow()
    {
        InitializeComponent();
        notepad = new NTPD_CS(fieldEdit); // Инициализация блокнота с привязкой к полю редактирования
    }
    // Действие "Создать"
    private void Создать_Click(object sender, RoutedEventArgs e)
    {
        notepad.Create(); // Вызов метода создания нового документа
        this.Title = notepad.NameFile; // Обновление заголовка окна
    }

    // Действие "Открыть"
    private void Открыть_Click(object sender, RoutedEventArgs e)
    {
        notepad.Load(); // Вызов метода загрузки файла
        this.Title = notepad.NameFile; // Обновление заголовка окна
    }

    // Действие "Сохранить"
    private void Сохранить_Click(object sender, RoutedEventArgs e)
    {
        notepad.ASaveNotepad(); // Вызов метода сохранения файла
        this.Title = notepad.NameFile; // Обновление заголовка окна
    }

    // Действие "Сохранить как"
    private void СохранитьКак_Click(object sender, RoutedEventArgs e)
    {
        notepad.SaveAs(); // Вызов метода сохранения с новым именем
        this.Title = notepad.NameFile; // Обновление заголовка окна
    }

    // Действие "Выход"
    private void Выход_Click(object sender, RoutedEventArgs e)
    {
        if (notepad.Exit()) // Вызов метода завершения работы
        {
            Application.Current.Shutdown(); // Завершение программы
        }
    }

    // Действие "Копировать"
    private void Копировать_Click(object sender, RoutedEventArgs e)
    {
        notepad.Copy(); // Вызов метода копирования текста
    }

    // Действие "Вырезать"
    private void Вырезать_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Проверяем, есть ли выделенный текст.
            if (!string.IsNullOrEmpty(fieldEdit.Selection.Text))
            {
                // Вырезаем текст (он автоматически помещается в буфер обмена и исчезает из RichTextBox).
                fieldEdit.Cut();
            }
            else
            {
                // Если текст не выделен, показываем сообщение.
                MessageBox.Show("Нет выделенного текста для вырезания.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            // Обработка возможных ошибок.
            MessageBox.Show("Ошибка при вырезании текста: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Действие "Вставить"
    private void Вставить_Click(object sender, RoutedEventArgs e)
    {
        notepad.Paste(); // Вызов метода вставки текста
    }

    // Действие "Отменить"
    private void Отменить_Click(object sender, RoutedEventArgs e)
    {
        notepad.Undo(); // Вызов метода отмены последнего действия
    }

    // Обработка нажатия клавиш для контекстного меню
    private void FieldEdit_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) // Ctrl + Z
        {
            notepad.Undo(); // Отмена последнего действия
        }
    }
    private void FontSize_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Получаем значение выбранного размера шрифта из заголовка пункта меню.
            string selectedFontSize = (sender as MenuItem).Header.ToString();

            // Преобразуем строку в число.
            double fontSize = double.Parse(selectedFontSize);

            // Применяем выбранный размер шрифта к выделенному тексту.
            fieldEdit.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при изменении размера шрифта: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void Font_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Получаем название выбранного шрифта из заголовка пункта меню.
            string selectedFont = (sender as MenuItem).Header.ToString();

            // Применяем выбранный шрифт к выделенному тексту.
            fieldEdit.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new System.Windows.Media.FontFamily(selectedFont));
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при изменении шрифта: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void Color_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Получаем название выбранного цвета из заголовка пункта меню.
            string selectedColorName = (sender as MenuItem).Header.ToString();

            // Преобразуем название цвета в объект SolidColorBrush.
            System.Windows.Media.Brush brush = null;

            switch (selectedColorName)
            {
                case "Синий":
                    brush = Brushes.Blue;
                    break;
                case "Зелёный":
                    brush = Brushes.Green;
                    break;
                case "Красный":
                    brush = Brushes.Red;
                    break;
                case "Жёлтый":
                    brush = Brushes.Yellow;
                    break;
                case "Голубой":
                    brush = Brushes.Cyan;
                    break;
                case "Фиолетовый":
                    brush = Brushes.Purple;
                    break;
                case "Розовый":
                    brush = Brushes.Pink;
                    break;
                case "Чёрный":
                    brush = Brushes.Black;
                    break;
                case "Белый":
                    brush = Brushes.White;
                    break;
            }

            if (brush != null)
            {
                // Применяем выбранный цвет к выделенному тексту.
                fieldEdit.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при изменении цвета текста: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    //Действие Справка
    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Выполнила программу: Сухомяткина Ксения Игоревна\nГруппа: ИСП-34\n\nЗадание:\nРазработать блокнот. Она должна обеспечивать след, действия:\nВвод и редактирование текста\nСохранение текста. Название сохраненного текста должно выводиться в строке заголовок. При повторном сохранении, имя файла не запрашивать.\nСохранение с новым именем.\nЗагрузка текста из файла\nЗавершение работы с программой. При завершении если текст не сохранен, запросить сообщение.\nСоздание нового документа.\nСправка о программе\nРеализовать действие с буфера обмена\nРеализовать отмену последнего действия\nРеализовать контекстное меню для действий с буфером обмена","Справка о программе",MessageBoxButton.OK, MessageBoxImage.Information);
    }
   
}
