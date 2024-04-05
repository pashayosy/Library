using System.Windows;

namespace Library.Windows
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        public string SelectedOption { get; private set; }

        public CustomDialog(string[] options)
        {
            InitializeComponent();
            OptionsComboBox.ItemsSource = options;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedOption = OptionsComboBox.SelectedItem as string;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

    
}
