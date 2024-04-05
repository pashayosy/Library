using Library.ViewModel;
using System.Windows.Controls;

namespace Library.View
{
    /// <summary>
    /// Interaction logic for RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : UserControl
    {
        public RegistrationView()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.RegisterAction(RegisterPassword, SecondRegisterPassword, RegError);
            }
        }
    }
}
