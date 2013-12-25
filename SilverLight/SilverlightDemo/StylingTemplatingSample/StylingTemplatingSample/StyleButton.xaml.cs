using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace StylingTemplatingSample
{
    public partial class StyleButton : UserControl
    {
        public StyleButton()
        {
            InitializeComponent();

            AllBooks.Add(new Book("4458907683", "Training Your Dog",
                new DateTime(2000, 2, 8), 44.25));
            //Set the data context for the list of books
            MyBooks.DataContext = AllBooks;

        }

        //Create a collection to store data items
        private ObservableCollection<Book> _AllBooks;
        public ObservableCollection<Book> AllBooks
        {
            get
            {
                if (_AllBooks == null)
                {
                    _AllBooks = new ObservableCollection<Book>();
                    _AllBooks.Add(new Book("3390092284", "All About Dogs",
                        new DateTime(2004, 3, 4), 12.99));

                }
                return _AllBooks;
            }
        }
    }

    public class Book
    {
        public Book()
        {
        }

        public Book(string isbn, string title,
            DateTime publishdate, double price)
        {
            this.ISBN = isbn;
            this.Title = title;
            this.PublishDate = publishdate;
            this.Price = price;
        }

        //Define the public properties
        public string ISBN { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public double Price { get; set; }
    }
}
