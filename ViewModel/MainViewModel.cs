using FileHandler.UserData;
using GalaSoft.MvvmLight.CommandWpf;
using Library.Windows;
using LibraryClasses.enums;
using LibraryClasses.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Library.ViewModel
{
    /// <summary>
    /// ViewModel responsible for controlling the login and registration flow within the application.
    /// It handles user interactions for logging in and registering, toggling between the login and
    /// registration views, and executing login and registration logic.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private Visibility _loginVisibility = Visibility.Visible;
        public Visibility LoginVisibility
        {
            get => _loginVisibility;
            set { _loginVisibility = value; OnPropertyChanged(); }
        }

        private Visibility _registrationVisibility = Visibility.Collapsed;

        public Visibility RegistrationVisibility
        {
            get => _registrationVisibility;
            set { _registrationVisibility = value; OnPropertyChanged(); }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _email;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private MainWindow _window;


        // Commands
        public ICommand ShowLoginCommand { get; set; }
        public ICommand ShowRegisterCommand { get; set; }


        /// <summary>
        /// Constructor that initializes commands and sets the initial view to the login screen.
        /// </summary>
        /// <param name="window">Reference to the main window for navigation purposes.</param>
        public MainViewModel(MainWindow window)
        {
            ShowLoginCommand = new RelayCommand(() => ShowLogin());
            ShowRegisterCommand = new RelayCommand(() => ShowRegister());
            _window = window;
        }


        /// <summary>
        /// Switches the view to the login screen.
        /// </summary>
        private void ShowLogin()
        {
            LoginVisibility = Visibility.Visible;
            RegistrationVisibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Switches the view to the registration screen.
        /// </summary>
        private void ShowRegister()
        {
            LoginVisibility = Visibility.Collapsed;
            RegistrationVisibility = Visibility.Visible;
        }


        /// <summary>
        /// Executes the login action, including validation against stored user credentials.
        /// </summary>
        /// <param name="password">The password input control from the UI.</param>
        /// <param name="errorLabel">Label for displaying error messages.</param>
        public void LoginAction(PasswordBox password, Label errorLable)
        {
            UserType userType = UserType.None;
            List<BasicUser> users = UserManager.LoadData();
            Guid id = new Guid();
            
            if (users.Any((user) => user.Password.Equals(password.Password) && user.Email.Equals(Email))) 
            {
                id = users.FirstOrDefault((user) => user.Password.Equals(password.Password) && user.Email.Equals(Email)).Id;
                userType = UserType.Regular;
            }


            if (Email == "Admin" && password.Password == "admin")
                userType = UserType.Admin;

            if (userType == UserType.None)
                _ = ShowErrorMessageAsync("The email or password dont exist !!!", errorLable);
            else 
            { 
                password.Password = "";
                Email = "";
                MainLibraryWindow mainLibraryWindow = new MainLibraryWindow(userType, id);
                mainLibraryWindow.Show();
                _window.Close();
            }
        }


        /// <summary>
        /// Executes the registration action, including validation of input and creation of a new user record.
        /// </summary>
        /// <param name="password">The password input control from the UI.</param>
        /// <param name="rePassword">The repeated password input control from the UI for confirmation.</param>
        /// <param name="errorLabel">Label for displaying error messages.</param>
        public void RegisterAction(PasswordBox password, PasswordBox rePassword, Label errorLabel)
        {
            if (password.Password.Equals(rePassword.Password))
            {
                try
                {
                    User u = new User(Name, Email, password.Password);
                    bool respond = UserManager.SaveData(u);
                    if (!respond)
                    {
                        throw new Exception("Ops... Something went wrong ,try to change email to other one");
                    }
                    Name = "";
                    Email = "";
                    password.Password = "";
                    rePassword.Password = "";
                    ShowLogin();
                }
                catch (Exception e)
                {
                    ShowErrorMessageAsync(e.Message, errorLabel);
                }
            }
            else
            {
                ShowErrorMessageAsync("The passwords are not the same", errorLabel);
            }

        }


        /// <summary>
        /// Displays an error message asynchronously, hiding it after a delay.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        /// <param name="errorLabel">Label for displaying the error message.</param>
        public static async Task ShowErrorMessageAsync(string message, Label errorLabel)
        {
            errorLabel.Content = message; // Use the passed message instead of a fixed string
            errorLabel.Visibility = Visibility.Visible;

            await Task.Delay(4000); // Asynchronously wait without blocking the UI thread

            // Ensure the visibility change is performed on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                errorLabel.Visibility = Visibility.Collapsed;
            });
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
