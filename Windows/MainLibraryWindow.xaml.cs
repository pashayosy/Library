using Library.ViewModel;
using System;
using System.Windows;
using LibraryClasses.enums;
using DataView = Library.View.DataView;

namespace Library.Windows
{
    /// <summary>
    /// Interaction logic for MainLibraryWindow.xaml
    /// </summary>
    public partial class MainLibraryWindow : Window
    {
        public MainLibraryWindow(UserType userType, Guid id)
        {
            InitializeComponent();
            DataContext = new LibraryViewModel(id, DataView.quantityColomn);

            if (userType == UserType.Regular)
                ShowRegular();

            if (userType == UserType.Admin)
                ShowAdmin();
        }

        private void ShowRegular()
        {
            AdminOption.Visibility = Visibility.Collapsed;
            UserOptionBuy.Visibility = Visibility.Visible;
            UserOptionBorrow.Visibility = Visibility.Visible;
            UserOptionShow.Visibility = Visibility.Visible;
            DataView.IdColomn.Visibility = Visibility.Collapsed;
        }

        private void ShowAdmin()
        {
            AdminOption.Visibility = Visibility.Visible;
            UserOptionBuy.Visibility = Visibility.Collapsed;
            UserOptionBorrow.Visibility = Visibility.Collapsed;
            UserOptionShow.Visibility = Visibility.Collapsed;
            DataView.ActionColomn.Visibility = Visibility.Collapsed;
        }
    }
}
