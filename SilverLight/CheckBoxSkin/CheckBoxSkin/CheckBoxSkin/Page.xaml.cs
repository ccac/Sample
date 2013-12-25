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

namespace CheckBoxSkin
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void UncheckedButton_Click(object sender, RoutedEventArgs e)
        {
            MyCheckBox.IsChecked = false;
        }

        private void IndeterminateButton_Click(object sender, RoutedEventArgs e)
        {
            MyCheckBox.IsChecked = null;
        }

        private void CheckedButton_Click(object sender, RoutedEventArgs e)
        {
            MyCheckBox.IsChecked = true;
        }

        private void DisabledButton_Click(object sender, RoutedEventArgs e)
        {
            MyCheckBox.IsEnabled = false;
        }

        private void EnabledButton_Click(object sender, RoutedEventArgs e)
        {
            MyCheckBox.IsEnabled = true;
        }
    }
}
