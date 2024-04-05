using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Enums.LibraryClasses;
using FileHandler.LibraryData;
using GalaSoft.MvvmLight.CommandWpf;
using Library.Windows;
using LibraryClasses.Models; // Adjust the namespace to where your models are located

namespace Library.ViewModel
{
    public class LibraryViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Genres> _allGenre;

        public IEnumerable<Genres> AllGenre
        {
            get { return _allGenre; }
            set { _allGenre = value; OnPropertyChanged(); }
        }

        public ICommand AddWindowOpenCommand { get; set; }


        public ObservableCollection<AbstractItem> Items { get; set; } = new ObservableCollection<AbstractItem>();

        private string _savedKeyWord = "";
        private string _savedGenre = "";

        public LibraryViewModel()
        {
            AddWindowOpenCommand = new RelayCommand(() => OpenAddWindow());
            LoadItems();
            AllGenre = Enum.GetValues(typeof(Genres)).Cast<Genres>();
        }

        private async Task OpenAddWindow()
        {
            AddWindow window = new AddWindow();
            var tcs = new TaskCompletionSource<bool>();

            // Event handler to capture the close event
            void dialog_Closed(object sender, EventArgs e)
            {
                window.Closed -= dialog_Closed; // Unsubscribe from the event
                tcs.SetResult(true); // Signal completion
            }

            window.Closed += dialog_Closed; // Subscribe to the Closed event
            window.Show(); // Non-blocking call to show the window

            await tcs.Task;

            LoadItems();
        }

        private void LoadItems()
        {
            Items.Clear();

            LibCollection libCollection = DataManager.LoadData("Items");

            foreach(var item in libCollection) 
            {
                Items.Add(item);
            }
            OnPropertyChanged(nameof(Items));
        }

        public void GenreSelecter(string genre) 
        {
            _savedGenre = genre;
            LoadItems();

            if (genre.Length > 0 && genre != Genres.None.ToString())
                Items = SortGenres();

            if (_savedKeyWord.Length > 0)
                Items = SortKeyWord();

            OnPropertyChanged(nameof(Items));
        }

        public void Search(string keyWord)
        {
            _savedKeyWord = keyWord;
            LoadItems();

            if (keyWord != "")
                Items = SortKeyWord();

            if (_savedGenre.Length > 0 && _savedGenre != Genres.None.ToString())
                Items = SortGenres();

            OnPropertyChanged(nameof(Items));
        }

        public ObservableCollection<AbstractItem> SortGenres() => new ObservableCollection<AbstractItem>(Items.Where((item) => item.Genres.ToString().Contains(_savedGenre)));
        public ObservableCollection<AbstractItem> SortKeyWord() => new ObservableCollection<AbstractItem>(Items.Where((item) => item.Title.ToLower().StartsWith(_savedKeyWord.ToLower())));

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
