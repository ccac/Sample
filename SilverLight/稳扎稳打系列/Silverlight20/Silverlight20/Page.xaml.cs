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

namespace Silverlight20
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }
       
        private void btnSize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as Microsoft.Windows.Controls.TreeView;

            var tag = ((Microsoft.Windows.Controls.TreeViewItem)treeView.SelectedItem).Tag;

            if (tag == null)
                return;

            Type type = typeof(Page).Assembly.GetType("Silverlight20." + tag, false);
            UIElement element = Activator.CreateInstance(type) as UIElement;

            this.pnl.Children.Clear();
            this.pnl.Children.Add(element);

            pageScroll.ScrollToVerticalOffset(0);
        }
    }
}
