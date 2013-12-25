using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SilverlightControls.Design.Editos
{
    public static class EditorDictionary
    {
        private static ResourceDictionary dictionary = (ResourceDictionary)Application.LoadComponent(new Uri("SilverlightControls.Design.Editos;component/EditorDictionary.xaml", UriKind.Relative));

        public static DataTemplate ColorsComboBox
        {
            get
            {
                return dictionary["ColorsComboBox"] as DataTemplate;
            }
        }

        public static DataTemplate SimpleTextBox
        {
            get
            {
                return dictionary["SimpleTextBox"] as DataTemplate;
            }
        }

        public static DataTemplate HelloWorldListBox
        {
            get
            {
                return dictionary["HelloWorldListBox"] as DataTemplate;
            }
        }

        public static DataTemplate MyCategoryEditor
        {
            get
            {
                return dictionary["MyCategoryEditor"] as DataTemplate;
            }
        }
    }
}
