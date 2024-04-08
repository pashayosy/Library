using LibraryClasses.Models;
using System.Windows.Controls;
using System.Windows;

namespace Library.ViewModel
{
    /// <summary>
    /// Custom DataTemplateSelector for dynamically selecting the appropriate DataTemplate
    /// based on the type of library item being displayed. This allows for different
    /// visual representations in UI elements like ListViews or DataGrids for various
    /// types of library items such as books and journals.
    /// </summary>
    public class LibraryItemTemplateViewModel: DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the DataTemplate to be used for Book objects.
        /// </summary>
        public DataTemplate BookView { get; set; }

        /// <summary>
        /// Gets or sets the DataTemplate to be used for Journal objects.
        /// </summary>
        public DataTemplate JournalView { get; set; }


        /// <summary>
        /// Overrides the SelectTemplate method to choose between BookView and JournalView
        /// based on the type of the item being displayed.
        /// </summary>
        /// <param name="item">The item for which to select the template.</param>
        /// <param name="container">The container in which the item will be displayed.</param>
        /// <returns>A DataTemplate for the item based on its type, or the base implementation if no match is found.</returns>

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
