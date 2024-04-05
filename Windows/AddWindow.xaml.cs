using Library.View;
using Library.ViewModel;
using LibraryClasses.enums;
using LibraryClasses.Models;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;

namespace Library.Windows
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
            var dialog = new CustomDialog(Enum.GetNames(typeof(AbstractItemType)));

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                var value = dialog.SelectedOption;
                switch (value)
                {
                    case "Book":
                        BookView.Visibility = Visibility.Visible;
                        break;
                    case "Journal":
                        JournalView.Visibility = Visibility.Visible;
                        break;
                    default:
                        BookView.Visibility = Visibility.Visible;
                        break;
                }

                DataContext = new AddViewModel((AbstractItemType)Enum.Parse(typeof(AbstractItemType), value), ItemAddError, this);
                AbstractItemView.dpPublicationDate.DisplayDateEnd = DateTime.Now;
                AbstractItemView.dpPublicationDate.DisplayDateStart = DateTime.Now.AddYears(-100);
                AbstractItemView.dpPublicationDate.SelectedDate = DateTime.Now;
            }
            else
            {
                Close();
            }
            
        }
    }
}
