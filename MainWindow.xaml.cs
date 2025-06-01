using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using Microsoft.VisualBasic;




namespace NotepadQodirov
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string currentFilePath = string.Empty;
        private bool isTextChanged = false;

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveChanges())
            {
                MainTextBox.Clear();
                currentFilePath = string.Empty;
                isTextChanged = false;
                Title = "Новый документ - Блокнот";
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (!ConfirmSaveChanges()) return;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                currentFilePath = openFileDialog.FileName;
                MainTextBox.Text = File.ReadAllText(currentFilePath);
                isTextChanged = false;
                Title = System.IO.Path.GetFileName(currentFilePath) + " - Блокнот";
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileAs_Click(sender, e);
            }
            else
            {
                File.WriteAllText(currentFilePath, MainTextBox.Text);
                isTextChanged = false;
            }
        }

        private void SaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                currentFilePath = saveFileDialog.FileName;
                File.WriteAllText(currentFilePath, MainTextBox.Text);
                Title = System.IO.Path.GetFileName(currentFilePath) + " - Блокнот";
                isTextChanged = false;
            }
        }

        private bool ConfirmSaveChanges()
        {
            if (!isTextChanged) return true;

            MessageBoxResult result = System.Windows.MessageBox.Show(
                "Сохранить изменения в текущем документе?",
                "Подтверждение",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                SaveFile_Click(null, null);
                return true;
            }

            return result == MessageBoxResult.No;
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextChanged = true;
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Cut();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Paste();
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.SelectAll();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveChanges())
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Простой блокнот, разработан на WPF.\nАвтор: Qodirov\n2025 год", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ChangeFont_Click(object sender, RoutedEventArgs e)
        {
            var fontDialog = new FontDialog();

            // Текущий шрифт устанавливается по умолчанию
            fontDialog.Font = new System.Drawing.Font(
                MainTextBox.FontFamily.Source,
                (float)(MainTextBox.FontSize * 72.0 / 96.0) // перевод пикселей в пункты
            );

            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainTextBox.FontFamily = new System.Windows.Media.FontFamily(fontDialog.Font.Name);
                MainTextBox.FontSize = fontDialog.Font.Size * 96.0 / 72.0; // обратно: пункты → пиксели
                MainTextBox.FontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                MainTextBox.FontStyle = fontDialog.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void FindText_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Введите текст для поиска:", "Поиск");

            if (!string.IsNullOrEmpty(input))
            {
                int index = MainTextBox.Text.IndexOf(input, StringComparison.OrdinalIgnoreCase);

                if (index >= 0)
                {
                    MainTextBox.Focus();
                    MainTextBox.Select(index, input.Length);
                }
                else
                {
                    System.Windows.MessageBox.Show("Текст не найден.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


    }
}