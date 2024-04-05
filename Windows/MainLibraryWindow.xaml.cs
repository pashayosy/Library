using Enums.LibraryClasses;
using Library.ViewModel;
using System.Globalization;
using System;
using System.Windows;
using System.Windows.Controls;
using LibraryClasses.enums;
using Library.View;
using System.Data;
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

            if (userType == UserType.Regular)
                ShowRegular();

            if (userType == UserType.Admin)
                ShowAdmin();

            DataContext = new LibraryViewModel();
        }

        private void ShowRegular()
        {
            AdminOption.Visibility = Visibility.Collapsed;
            UseroptionBuy.Visibility = Visibility.Visible;
            UserOptionBorrow.Visibility = Visibility.Visible;
            DataView dataView = DataView;
            dataView.dgItems.IsReadOnly = true;
        }

        private void ShowAdmin()
        {
            AdminOption.Visibility = Visibility.Visible;
            UseroptionBuy.Visibility = Visibility.Collapsed;
            UserOptionBorrow.Visibility = Visibility.Collapsed;
            DataView dataView = DataView;
            dataView.dgItems.IsReadOnly = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Using the sender parameter
            if (sender is ComboBox comboBox)
            {
                var selectedItem = comboBox.SelectedItem;
                if (DataContext is LibraryViewModel viewModel)
                {
                    viewModel.GenreSelecter(selectedItem.ToString());
                }
            }
            
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox textBox) 
            {
                if (DataContext is LibraryViewModel viewModel)
                {
                    viewModel.Search(textBox.Text);
                }
            }
        }
    }
}
