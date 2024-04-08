using Enums.LibraryClasses;
using FileHandler.LibraryData;
using GalaSoft.MvvmLight.CommandWpf;
using Library.Windows;
using LibraryClasses.Models; // Adjust the namespace to where your models are located
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Library.ViewModel
{
    /// <summary>
    /// The LibraryViewModel is designed to manage the library's inventory, including adding, removing, updating,
    /// buying, and borrowing items. It supports interaction with the user interface for managing library items,
    /// such as books and journals, by utilizing commands bound to UI actions. This ViewModel also handles filtering
    /// items based on search criteria and selected genres, providing dynamic updates to the UI through property
    /// notifications.
    /// </summary>
    public class LibraryViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Genres> _allGenre;

        public IEnumerable<Genres> AllGenre
        {
            get { return _allGenre; }
            set { _allGenre = value; OnPropertyChanged(); }
        }

        public ICommand AddWindowOpenCommand { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public ICommand UpdateItemCommand { get; set; }
        public ICommand BuyCommand { get; set; }
        public ICommand BorrowCommand { get; set; }
        public ICommand ShowBoughtItemCommand { get; set; }
        public ICommand ShowItemCommand { get; set; }
        public ICommand ShowBorrowedItemCommand { get; set; }
        public ICommand ReturnCommand { get; set; }


        public ObservableCollection<AbstractItem> Items { get; set; } = new ObservableCollection<AbstractItem>();

        private AbstractItem _userSelectedItem;

        public AbstractItem UserSelectedItem
        {
            get => _userSelectedItem;
            set
            {
                if (_userSelectedItem != value)
                {
                    _userSelectedItem = value;
                    OnPropertyChanged(nameof(UserSelectedItem));
                }
            }
        }

        private string _tbSearch;

        public string TbSearch
        {
            get => _tbSearch;
            set
            {
                _tbSearch = value;
                OnPropertyChanged(nameof(TbSearch));
                Search(value);
            }
        }

        private string _cbGenre;
        public string CbGenre
        {
            get => _cbGenre;
            set
            {
                _cbGenre = value;
                OnPropertyChanged(nameof(_cbGenre));
                GenreSelecter(value);
            }
        }

        private Visibility _buyButtonVisibility = Visibility.Visible; // Default state

        public Visibility BuyButtonVisibility
        {
            get { return _buyButtonVisibility; }
            set
            {
                _buyButtonVisibility = value;
                OnPropertyChanged(nameof(BuyButtonVisibility)); // Notify the UI of the change
            }
        }

        private Visibility _borrowButtonVisibility = Visibility.Visible; // Default state

        public Visibility BorrowButtonVisibility
        {
            get { return _borrowButtonVisibility; }
            set
            {
                _borrowButtonVisibility = value;
                OnPropertyChanged(nameof(BorrowButtonVisibility)); // Notify the UI of the change
            }
        }

        private Visibility _returnButtonVisibility = Visibility.Collapsed; // Default state

        public Visibility ReturnButtonVisibility
        {
            get { return _returnButtonVisibility; }
            set
            {
                _returnButtonVisibility = value;
                OnPropertyChanged(nameof(ReturnButtonVisibility)); // Notify the UI of the change
            }
        }


        private string _savedKeyWord = "";
        private string _savedGenre = "";
        private string _filename = "Items";
        private Guid userId;
        private DataGridTextColumn _quantityColomn;

        private readonly string ItemsPath;
        private readonly string BoughtPath;
        private readonly string BorrowPath;

        /// <summary>
        /// Constructor for initializing commands and setting up the ViewModel.
        /// </summary>
        /// <param name="id">The user's identifier for customizing user-specific actions, like showing borrowed items.</param>
        /// <param name="quantityColumn">A reference to the DataGrid's quantity column for visibility control.</param>
        public LibraryViewModel(Guid id, DataGridTextColumn quantityColomn)
        {
            AddWindowOpenCommand = new RelayCommand(() => OpenAddWindow());
            RemoveItemCommand = new RelayCommand(() => RemoveItem());
            UpdateItemCommand = new RelayCommand(() => UpdateItem());
            BuyCommand = new RelayCommand<AbstractItem>((item) => BuyItem(item));
            BorrowCommand = new RelayCommand<AbstractItem>((item) => BorrowItem(item));
            ShowBoughtItemCommand = new RelayCommand<string>((filename) => ShowBoughtItem(filename));
            ShowItemCommand = new RelayCommand<string>((filename) => ShowItem(filename));
            ShowBorrowedItemCommand = new RelayCommand<string>((filename) => ShowBorrowedItem(filename));
            ReturnCommand = new RelayCommand<AbstractItem>((item) => ReturnItem(item));
            userId = id;
            _quantityColomn = quantityColomn;

            ItemsPath = "Items";
            BoughtPath = Path.Combine("BuyFolder", userId.ToString());
            BorrowPath = Path.Combine("BorrowFolder", userId.ToString());

            LoadItems();
            AllGenre = Enum.GetValues(typeof(Genres)).Cast<Genres>();
        }


        /// <summary>
        /// Handles the logic for removing a selected item from the library, including confirmation and error handling.
        /// </summary>
        private void ReturnItem(AbstractItem item)
        {
            if (DataManager.DeleteData(item.Id, BorrowPath)) 
            {
                MessageBox.Show("You returned the item back");
                item.QuantityInStock++;
                DataManager.UpdateData(item.Id, item, ItemsPath);
            }
            else
                MessageBox.Show("Opss, Error happend , try to return later");
            LoadItems();
        }

        private void ShowUserItem(string fileName)
        {
            _quantityColomn.Visibility = Visibility.Collapsed;
            _filename = $"{fileName}/{userId}";
            LoadItems();
        }

        private void ShowBoughtItem(string fileName)
        {
            BuyButtonVisibility = Visibility.Collapsed;
            BorrowButtonVisibility = Visibility.Collapsed;
            ReturnButtonVisibility = Visibility.Collapsed;
            ShowUserItem(fileName);
        }
        private void ShowBorrowedItem(string fileName)
        {
            BuyButtonVisibility = Visibility.Collapsed;
            BorrowButtonVisibility = Visibility.Collapsed;
            ReturnButtonVisibility = Visibility.Visible;
            ShowUserItem(fileName);
        }

        private void ShowItem(string fileName)
        {
            _quantityColomn.Visibility = Visibility.Visible;
            BuyButtonVisibility = Visibility.Visible;
            BorrowButtonVisibility = Visibility.Visible;
            ReturnButtonVisibility = Visibility.Collapsed;
            _filename = $"{fileName}";
            LoadItems();
        }

        private void BuyItem(AbstractItem item)
        {
            if (item.QuantityInStock > 0)
            {
                item.QuantityInStock--;
                DataManager.UpdateData(item.Id, item, ItemsPath);
                if (DataManager.SaveData(item, BoughtPath))
                    MessageBox.Show($"You Buy the item {item.Title}");
                else
                    MessageBox.Show($"Some error happend, Try later to buy => {item.Title}");
            }
            else
                MessageBox.Show("You can't buy the item because it out of stock");
            LoadItems();
        }

        private void BorrowItem(AbstractItem item)
        {
            if (item.QuantityInStock > 0)
            {
                item.QuantityInStock--;
                DataManager.UpdateData(item.Id, item, ItemsPath);
                if (DataManager.SaveData(item, BorrowPath))
                    MessageBox.Show($"You Borrow the item {item.Title}");
                else
                    MessageBox.Show($"Some error happend, Try later to borrow => {item.Title}");
            }
            else
                MessageBox.Show("You can't borrow the item because it out of stock");
            LoadItems();
        }

        private async Task AddReloadAfterClose(Window window)
        {
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


        /// <summary>
        /// Updates the details of the selected item in the library, refreshing the item list upon updating.
        /// </summary>
        private async void UpdateItem()
        {
            if (_userSelectedItem != null)
            {
                UpdateWindow window = new UpdateWindow(_userSelectedItem);
                await AddReloadAfterClose(window);
            }
        }

        private void RemoveItem()
        {
            if (_userSelectedItem != null)
            {
                if (DataManager.DeleteData(_userSelectedItem.Id, "Items"))
                {
                    MessageBox.Show($"Item {_userSelectedItem.Id} Deleted");
                    LoadItems();
                }
                else
                    MessageBox.Show($"Item {_userSelectedItem.Id} can't be Deleted");
            }
        }


        /// <summary>
        /// Opens the window for adding new items to the library, refreshing the item list upon closing the window.
        /// </summary>
        private async void OpenAddWindow()
        {
            AddWindow window = new AddWindow();
            await AddReloadAfterClose(window);
        }

        private void LoadItems()
        {
            Items.Clear();

            LibCollection libCollection = DataManager.LoadData(_filename);

            foreach (var item in libCollection)
            {
                Items.Add(item);
            }
            OnPropertyChanged(nameof(Items));
        }


        /// <summary>
        /// Filters the displayed items based on the selected genre.
        /// </summary>
        /// <param name="genre">The genre used for filtering items.</param>
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


        /// <summary>
        /// Filters the displayed items based on the entered search keyword.
        /// </summary>
        /// <param name="keyWord">The search keyword used for filtering items.</param>
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
