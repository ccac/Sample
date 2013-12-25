using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightDemoWebClient
{
    public class Book
    {
        public Book(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
