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

namespace SilverlightDemoTransForm
{
    public partial class MatrixTransform : UserControl
    {
        public MatrixTransform()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            myStoryboard.Resume();
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            myStoryboard.Pause();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin();
            myStoryboard.Pause();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myTransform1.Angle += 15;
        }
    }
}
