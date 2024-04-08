using Enums.LibraryClasses;
using FileHandler.LibraryData;
using GalaSoft.MvvmLight.Command;
using LibraryClasses.enums;
using LibraryClasses.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Library.ViewModel
{
    /// <summary>
    /// ViewModel supporting the addition and updating of library items such as books and journals.
    /// It provides properties for editing item details, commands for saving changes, and manages the selection of genres.
    /// Implements INotifyPropertyChanged for data binding with UI elements.
    /// </summary>
    public class ItemUpdateAndAddViewModel : INotifyPropertyChanged
    {
        // Abstract item
        private string title;
        private string publisher;
        private DateTime publicationDate;
        private int quantityInStock;
        private string description;
        private double price;

        // Book
        private string iSBN;
        private string author;
        private int edition;

        // Journal
        private string iSSN;
        private int volume;
        private int issue;
        private string editor;

        public string Title
        {
            get => title;
            set { title = value; OnPropertyChanged(); }
        }

        public string Publisher
        {
            get => publisher;
            set { publisher = value; OnPropertyChanged(); }
        }

        public DateTime PublicationDate
        {
            get => publicationDate;
            set { publicationDate = value; OnPropertyChanged(); }
        }

        public int QuantityInStock
        {
            get => quantityInStock;
            set { quantityInStock = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => description;
            set { description = value; OnPropertyChanged(); }
        }

        public ObservableCollection<GenreViewModel> LbGenres { get; set; }

        public double Price
        {
            get => price;
            set { price = value; OnPropertyChanged(); }
        }
        public string ISBN
        {
            get => iSBN; set
            {
                iSBN = value;
                OnPropertyChanged();
            }
        }

        public string Author
        {
            get => author; set
            {
                author = value;
                OnPropertyChanged();
            }
        }

        public int Edition
        {
            get => edition; set
            {
                edition = value;
                OnPropertyChanged();
            }
        }

        public string ISSN
        {
            get => iSSN;
            set { iSSN = value; OnPropertyChanged(); }
        }
        public int Volume
        {
            get => volume;
            set { volume = value; OnPropertyChanged(); }
        }
        public int Issue
        {
            get => issue;
            set { issue = value; OnPropertyChanged(); }
        }

        public string Editor
        {
            get => editor;
            set { editor = value; OnPropertyChanged(); }
        }

        private AbstractItem itemToUpdate;

        public ICommand AddItemCommand { get; set; }
        public ICommand UpdateItemCommand { get; set; }

        private AbstractItemType abstractItemType;
        private Label errorLabel;
        private Window _window;


        /// <summary>
        /// Initializes a new instance for adding a new item, setting up commands and initializing genre selections.
        /// </summary>
        /// <param name="type">The type of the item to add (Book or Journal).</param>
        /// <param name="errorLabel">Label to display error messages.</param>
        /// <param name="window">The window instance for closing upon completion.</param>
        public ItemUpdateAndAddViewModel(AbstractItemType type, Label errorLabel, Window window)
        {
            AddItemCommand = new RelayCommand(() => AddItem());
            UpdateItemCommand = new RelayCommand(() => UpdateItem());
            abstractItemType = type;
            this.errorLabel = errorLabel;
            _window = window;

            GenreInit();
        }


        /// <summary>
        /// Initializes a new instance for updating an existing item, loading its details into the properties.
        /// </summary>
        /// <param name="errorLabel">Label to display error messages.</param>
        /// <param name="window">The window instance for closing upon completion.</param>
        /// <param name="toUpdate">The item to update.</param>
        public ItemUpdateAndAddViewModel(Label errorLabel, Window window, AbstractItem toUpdate)
        {
            AddItemCommand = new RelayCommand(() => AddItem());
            UpdateItemCommand = new RelayCommand(() => UpdateItem());
            this.errorLabel = errorLabel;
            _window = window;
            itemToUpdate = toUpdate;

            if (itemToUpdate != null)
            {
                Title = itemToUpdate.Title;
                Publisher = itemToUpdate.Publisher;
                PublicationDate = itemToUpdate.PublicationDate;
                QuantityInStock = itemToUpdate.QuantityInStock;
                Description = itemToUpdate.Description;
                Price = itemToUpdate.Price;

                switch (itemToUpdate)
                {
                    case Book _:
                        Book book = itemToUpdate as Book;
                        ISBN = book.ISBN;
                        Author = book.Author;
                        Edition = book.Edition;
                        break;
                    case Journal _:
                        Journal journal = itemToUpdate as Journal;
                        ISSN = journal.ISSN;
                        Volume = journal.Volume;
                        Issue = journal.Issue;
                        Editor = journal.Editor;
                        break;
                }
            }
            else
            {
                _window.Close();
            }


            GenreInit();
        }

        private void GenreInit()
        {
            LbGenres = new ObservableCollection<GenreViewModel>();

            foreach (Genres genre in Enum.GetValues(typeof(Genres)))
            {
                if (genre != Genres.None)
                {
                    LbGenres.Add(new GenreViewModel { Genre = genre});
                }
            }

            if(itemToUpdate != null)
            {
                foreach(GenreViewModel genreViewModel in  LbGenres) 
                {
                    genreViewModel.IsSelected = HasGenre(itemToUpdate.Genres, genreViewModel.Genre);
                }
            }
        }

        private bool HasGenre(Genres allGenres, Genres genreToCheck) => (allGenres & genreToCheck) == genreToCheck;

        public Genres SumUpAllTheGenres() => LbGenres.Where((element) => element.IsSelected).Aggregate(Genres.None, (current, genre) => current | genre.Genre);

        private void AddItem()
        {
            try
            {
                AbstractItem item = null;
                switch (abstractItemType)
                {
                    case AbstractItemType.Book:
                        item = new Book(ISBN, Author, Edition, Title, Publisher, PublicationDate, QuantityInStock, Description, SumUpAllTheGenres(), Price);
                        break;
                    case AbstractItemType.Journal:
                        item = new Journal(ISSN, Volume, Issue, Editor, Title, Publisher, PublicationDate, QuantityInStock, Description, SumUpAllTheGenres(), Price);
                        break;
                }
                bool respond = DataManager.SaveData(item, "Items");
                if (!respond)
                {
                    throw new Exception("Ops... Something went wrong ,try to change email to other one");
                }

                _window.Close();
            }
            catch (Exception e)
            {
                MainViewModel.ShowErrorMessageAsync(e.Message, errorLabel);
            }
        }

        private void UpdateItem()
        {
            try
            {
                Book book = null;
                Journal journal = null;
                itemToUpdate.Title = Title;
                itemToUpdate.Publisher = Publisher;
                itemToUpdate.PublicationDate = PublicationDate;
                itemToUpdate.QuantityInStock = QuantityInStock;
                itemToUpdate.Description = Description;
                itemToUpdate.Price = Price;
                itemToUpdate.Genres = SumUpAllTheGenres();

                switch (itemToUpdate)
                {
                    case Book _:
                        book = itemToUpdate as Book;
                        book.ISBN = ISBN;
                        book.Author = Author;
                        book.Edition = Edition;
                        break;
                    case Journal _:
                        journal = itemToUpdate as Journal;
                        journal.Issue = Issue;
                        journal.Volume = Volume;
                        journal.Issue = Issue;
                        journal.Editor = Editor;
                        break;
                }

                bool respond = DataManager.UpdateData(itemToUpdate.Id, itemToUpdate, "Items");
                if (!respond)
                {
                    throw new Exception("Ops... Something went wrong ,try to change email to other one");
                }

                _window.Close();
            }
            catch (Exception e)
            {
                MainViewModel.ShowErrorMessageAsync(e.Message, errorLabel);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
