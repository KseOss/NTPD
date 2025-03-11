using System.Windows; // Для работы с WPF (Window, RoutedEventArgs)
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
        notepad.Cut(); // Вызов метода вырезания текста
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
    //Действие Справка
    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Выполнила программу: Сухомяткина Ксения Игоревна\nГруппа: ИСП-34\n\nЗадание:\nРазработать блокнот. Она должна обеспечивать след, действия:\nВвод и редактирование текста\nСохранение текста. Название сохраненного текста должно выводиться в строке заголовок. При повторном сохранении, имя файла не запрашивать.\nСохранение с новым именем.\nЗагрузка текста из файла\nЗавершение работы с программой. При завершении если текст не сохранен, запросить сообщение.\nСоздание нового документа.\nСправка о программе\nРеализовать действие с буфера обмена\nРеализовать отмену последнего действия\nРеализовать контекстное меню для действий с буфером обмена","Справка о программе",MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
