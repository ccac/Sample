using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightControls
{
    public class myControl : Control
    {
        public string MyStringProperty
        {
            get { return GetValue(MyStringPropertyProperty) as string; }
            set { SetValue(MyStringPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyStringPropertyProperty =
            DependencyProperty.Register("MyStringProperty", typeof(string), typeof(myControl), null);

        public int MyIntProperty
        {
            get { return (int)GetValue(MyIntPropertyProperty); }
            set { SetValue(MyIntPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyIntPropertyProperty =
            DependencyProperty.Register("MyIntProperty", typeof(int), typeof(myControl), null);


        public object MyObjectProperty
        {
            get { return (object)GetValue(MyObjectPropertyProperty); }
            set { SetValue(MyObjectPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyObjectPropertyProperty =
            DependencyProperty.Register("MyObjectProperty", typeof(object), typeof(myControl), null);
    }
}
