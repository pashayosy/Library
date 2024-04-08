using Enums.LibraryClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModel
{
    /// <summary>
    /// Represents a ViewModel for genres, implementing the INotifyPropertyChanged interface.
    /// This class facilitates notification when a property value changes, which is crucial for data binding in MVVM applications.
    /// </summary>
    public class GenreViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Holds the genre type.
        /// </summary>
        public Genres Genre { get; set; }

        private bool _isSelected;

        /// <summary>
        /// Gets or sets a value indicating whether the genre is selected.
        /// Changes to this property raise the PropertyChanged event.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

