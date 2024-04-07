using Library.ViewModel;
using LibraryClasses.enums;
using LibraryClasses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow(AbstractItem item)
        {
            InitializeComponent();
            if (item != null)
            {
                switch (item)
                {
                    case Book _:
                        BookView.Visibility = Visibility.Visible;
                        windowTitleLable.Content = "Add Book";
                        break;
                    case Journal _:
                        JournalView.Visibility = Visibility.Visible;
                        windowTitleLable.Content = "Add Journal";
                        break;
                    default:
                        BookView.Visibility = Visibility.Visible;
                        break;
                }

                DataContext = new ItemUpdateAndAddViewModel(ItemUpdateError, this, item);
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
