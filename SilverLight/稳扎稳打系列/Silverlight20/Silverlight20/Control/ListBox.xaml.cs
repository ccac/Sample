using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Windows.Browser;

namespace Silverlight20.Control
{
    public partial class ListBox : UserControl
    {
        public ListBox()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ListBox.SelectedItem - ListBox中被选中的对象

            var lst = sender as System.Windows.Controls.ListBox;

            MessageBox.Show(
                ((System.Windows.Controls.ListBoxItem)lst.SelectedItem).Content + " 被单击了",
                "提示",
                MessageBoxButton.OK);
        }
    }
}
