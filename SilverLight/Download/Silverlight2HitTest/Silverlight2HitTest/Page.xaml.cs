using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Silverlight2HitTest
{
	public partial class Page : UserControl
	{
		public Page()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        void MouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            Point mousePt = e.GetPosition(LayoutRoot);
            IEnumerable<UIElement> elements = LayoutRoot.HitTest(mousePt);
            string result="";
            foreach (UIElement element in elements)
            {
                FrameworkElement fe = element as FrameworkElement;
                if(fe.Name!=""&&fe.Name!="LayoutRoot")
                result += fe.Name+";";
            }
            tb1.Text = result;
        }
	}
}