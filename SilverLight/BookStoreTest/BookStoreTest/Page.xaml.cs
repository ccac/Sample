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
using System.Windows.Data;
using System.ComponentModel;

namespace BookStoreTest
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            AllBooks.Add(new Book("4458907683", "Training Your Dog",
                new DateTime(2000, 2, 8), 44.25));
            //Set the data context for the list of books
            MyBooks.DataContext = AllBooks;

            BookDetails.DataContext = AllBooks[0];
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

        private void MyBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            BookDetails.DataContext = lb.SelectedItem;

            DetailView.DataContext = lb.SelectedItem;
            DetailView.Visibility = Visibility.Visible;
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

    public class DateToString : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            DateTime date = (DateTime)value;
            string s = date.Month.ToString() + "/" + date.Year.ToString();
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();

        }

        #endregion
    }

    public class ToCurrency : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType,
             object parameter, System.Globalization.CultureInfo culture)
        {
            double amount = (double)value;
            string s = culture.NumberFormat.CurrencySymbol +
                amount.ToString();
            return s;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
