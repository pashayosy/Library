using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using System.Runtime.CompilerServices;

namespace Library.ViewModel
{
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


        // Commands
        public ICommand ShowLoginCommand { get; set; }
        public ICommand ShowRegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public MainViewModel()
        {
            ShowLoginCommand = new RelayCommand(() => ShowLogin());
            ShowRegisterCommand = new RelayCommand(() => ShowRegister());
            // TODO : Initialize LoginCommand and RegisterCommand with actual execution logic
            LoginCommand = new RelayCommand(() => LoginAction());
            RegisterCommand = new RelayCommand(() => RegisterAction());
        }

        private void ShowLogin()
        {
            LoginVisibility = Visibility.Visible;
            RegistrationVisibility = Visibility.Collapsed;
        }

        private void ShowRegister()
        {
            LoginVisibility = Visibility.Collapsed;
            RegistrationVisibility = Visibility.Visible;
        }

        private void LoginAction()
        {
            // TODO : make logic workin , dont forget make showing the errors
        }

        private void RegisterAction()
        {
            // TODO : make registration workin , dont forget make showing the errors
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
