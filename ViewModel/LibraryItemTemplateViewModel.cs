using LibraryClasses.Models;
using System.Windows.Controls;
using System.Windows;

namespace Library.ViewModel
{
    public class LibraryItemTemplateViewModel: DataTemplateSelector
    {
        public DataTemplate BookView { get; set; }
        public DataTemplate JournalView { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case Book _:
                    return BookView;
                case Journal _:
                    return JournalView;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}
