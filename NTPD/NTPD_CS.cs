using Microsoft.Win32; // Для работы с диалоговыми окнами (SaveFileDialog, OpenFileDialog)
using System; // Базовые классы и структуры данных
using System.IO; // Для работы с файлами (FileStream, File)
using System.Windows; // Для работы с WPF (MessageBox, MessageBoxButton)
using System.Windows.Controls; // Для работы с элементами управления (RichTextBox)
using System.Windows.Documents; // Для работы с текстовыми документами (TextRange)

namespace NTPD
{
    class NTPD_CS
    {
        string nameFile; // Хранит имя текущего файла
        RichTextBox fieldEdit; // Поле редактирования текста
        public bool Modified { get; set; } // Флаг, указывающий, был ли текст изменен

        public string NameFile // Свойство для получения имени файла
        {
            get { return nameFile; }
        }

        // Конструктор класса
        public NTPD_CS(RichTextBox fieldEdit)
        {
            nameFile = ""; // Инициализация имени файла пустой строкой
            this.fieldEdit = fieldEdit; // Привязка поля редактирования
            Modified = false; // Сброс флага изменений
        }

        // Метод сохранения блокнота
        public bool ASaveNotepad()
        {
            SaveFileDialog sd = new SaveFileDialog(); // Создание диалогового окна для сохранения файла
            sd.DefaultExt = "rtf"; // Установка расширения по умолчанию
            sd.Filter = "Текстовый файл (.rtf)|*.rtf|Все файлы (*.*)|*.*"; // Фильтр для выбора типа файла

            if (nameFile == "") // Если имя файла не задано
            {
                if (sd.ShowDialog() == true) // Отображение диалога и проверка, что пользователь выбрал файл
                    nameFile = sd.FileName; // Сохранение выбранного имени файла
                else
                    return false; // Если пользователь отменил сохранение, возвращаем false
            }

            try
            {
                TextRange doc = new TextRange(fieldEdit.Document.ContentStart, fieldEdit.Document.ContentEnd); // Получение содержимого текстового поля
                using (FileStream fs = File.Create(nameFile)) // Создание потока для записи в файл
                {
                    doc.Save(fs, DataFormats.Rtf); // Сохранение текста в формате RTF
                }
                Modified = false; // Сброс флага изменений после сохранения
                return true; // Возвращаем true, если сохранение прошло успешно
            }
            catch (Exception ex) // Обработка возможных ошибок при сохранении
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Метод создания нового документа
        public void Create()
        {
            if (Modified) // Если документ был изменен
            {
                MessageBoxResult result = MessageBox.Show("Вы хотите сохранить изменения в файле?", "Notepad", MessageBoxButton.YesNoCancel); // Запрос на сохранение изменений

                if (result == MessageBoxResult.Yes) // Если пользователь выбрал "Да"
                {
                    if (!ASaveNotepad()) return; // Пытаемся сохранить файл, если не удалось, прерываем действие
                }
                else if (result == MessageBoxResult.Cancel) // Если пользователь выбрал "Отмена"
                {
                    return; // Прерываем создание нового документа
                }
            }

            fieldEdit.Document.Blocks.Clear(); // Очищаем содержимое текстового поля
            nameFile = ""; // Сбрасываем имя файла
            Modified = false; // Сбрасываем флаг изменений
        }

        // Метод загрузки текста из файла
        public void Load()
        {
            OpenFileDialog od = new OpenFileDialog(); // Создание диалогового окна для открытия файла
            od.DefaultExt = "rtf"; // Установка расширения по умолчанию
            od.Filter = "Текстовый файл (.rtf)|*.rtf|Все файлы (*.*)|*.*"; // Фильтр для выбора типа файла

            if (od.ShowDialog() == true) // Отображение диалога и проверка, что пользователь выбрал файл
            {
                try
                {
                    TextRange doc = new TextRange(fieldEdit.Document.ContentStart, fieldEdit.Document.ContentEnd); // Получение содержимого текстового поля
                    using (FileStream fs = File.OpenRead(od.FileName)) // Открытие потока для чтения файла
                    {
                        doc.Load(fs, DataFormats.Rtf); // Загрузка текста из файла в формате RTF
                    }
                    nameFile = od.FileName; // Сохранение имени загруженного файла
                    Modified = false; // Сброс флага изменений
                }
                catch (Exception ex) // Обработка возможных ошибок при загрузке
                {
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Метод сохранения с новым именем
        public void SaveAs()
        {
            string oldName = nameFile; // Сохраняем старое имя файла
            nameFile = ""; // Сбрасываем имя файла для вызова диалога сохранения
            if (!ASaveNotepad()) // Пытаемся сохранить файл под новым именем
            {
                nameFile = oldName; // Если сохранение отменено, восстанавливаем старое имя
            }
        }

        // Метод завершения работы программы
        public bool Exit()
        {
            if (Modified) // Если документ был изменен
            {
                MessageBoxResult result = MessageBox.Show("Вы хотите сохранить изменения в файле?", "Notepad", MessageBoxButton.YesNoCancel); // Запрос на сохранение изменений

                if (result == MessageBoxResult.Yes) // Если пользователь выбрал "Да"
                {
                    if (!ASaveNotepad()) return false; // Пытаемся сохранить файл, если не удалось, прерываем выход
                }
                else if (result == MessageBoxResult.Cancel) // Если пользователь выбрал "Отмена"
                {
                    return false; // Прерываем выход
                }
            }

            return true; // Разрешаем выход из программы
        }

        // Метод копирования текста в буфер обмена
        public void Copy()
        {
            fieldEdit.Copy(); // Копирование выделенного текста в буфер обмена
        }

        // Метод вырезания текста в буфер обмена
        public void Cut()
        {
            fieldEdit.Cut(); // Вырезание выделенного текста в буфер обмена
            Modified = true; // Устанавливаем флаг изменений
        }

        // Метод вставки текста из буфера обмена
        public void Paste()
        {
            fieldEdit.Paste(); // Вставка текста из буфера обмена
            Modified = true; // Устанавливаем флаг изменений
        }

        // Метод отмены последнего действия
        public void Undo()
        {
            if (fieldEdit.CanUndo) // Проверка возможности отмены действия
            {
                fieldEdit.Undo(); // Отмена последнего действия
                Modified = true; // Устанавливаем флаг изменений
            }
        }
        
    }
}