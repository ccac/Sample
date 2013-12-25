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
using System.Text;

namespace InputEventHandler
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            Canvas2.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas1_MouseLeftButtonDown);
        }

        private void Canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            StatusText.Text = "";
            Canvas c = sender as Canvas;
            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(255, 200, 77, 0));
            c.Background = b;
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                FrameworkElement fe = e.Source as FrameworkElement;
                StringBuilder sb = new StringBuilder();
                sb.Append("source: " + fe.Name + "\n");
                sb.Append("relative x/y to source: " + e.GetPosition(fe) + "\n");
                sb.Append("Silverlight content area x/y : " + e.GetPosition(null));
                StatusText.Text = sb.ToString();
            }
        }

        private void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    StatusText.Text = "Beep!";
                }
            }
        }
    }
}
