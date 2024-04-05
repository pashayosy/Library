using Library.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Library.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.LoginAction(LoginPassword, LogError);
            }
        }
    }
}
