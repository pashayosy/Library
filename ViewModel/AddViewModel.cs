using Enums.LibraryClasses;
using FileHandler.LibraryData;
using GalaSoft.MvvmLight.Command;
using LibraryClasses.enums;
using LibraryClasses.Models;
using MahApps.Metro.Actions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Library.ViewModel
{
    public class AddViewModel : INotifyPropertyChanged
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

        public string ISSN { 
            get => iSSN;
            set { iSSN = value; OnPropertyChanged(); } 
        }
        public int Volume { 
            get => volume;
            set { volume = value; OnPropertyChanged(); } 
        }
        public int Issue {
            get => issue;
            set { issue = value;  OnPropertyChanged(); }
        }

        public string Editor {
            get => editor;
            set { editor = value; OnPropertyChanged(); }
        }

        public ICommand AddItemCommand {  get; set; }

        private AbstractItemType abstractItemType;
        private Label errorLabel;
        private Window _window;

        public AddViewModel(AbstractItemType type, Label errorLabel, Window window)
        {
            AddItemCommand = new RelayCommand(() => AddItem());
            abstractItemType = type;
            this.errorLabel = errorLabel;
            _window = window;

            GenreInit();
        }

        private void GenreInit()
        {
            LbGenres = new ObservableCollection<GenreViewModel>();

            foreach (Genres genre in Enum.GetValues(typeof(Genres)))
            {
                if (genre != Genres.None)
                {
                    LbGenres.Add(new GenreViewModel { Genre = genre });
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
