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

namespace Silverlight20.Interactive
{
    public partial class Mouse : UserControl
    {
        public Mouse()
        {
            InitializeComponent();
        }

        void ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            ellipse.Fill = new SolidColorBrush(Colors.Yellow);
        }

        void ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            ellipse.Fill = new SolidColorBrush(Colors.Red);
        }

        private void ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ellipse.Fill = new SolidColorBrush(Colors.Yellow);
        }

        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ellipse.Fill = new SolidColorBrush(Colors.Blue);

            // MouseButtonEventArgs.Handled - 此事件是否已被处理
            //     false - 未被处理，事件的路由为向上冒泡
            //     true - 已被处理，事件的路由为不再冒泡
            e.Handled = true;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果鼠标单击 rectangle 对象，则 会 执行到此句
            // 如果鼠标单击 ellipse 对象，则 不会 执行到此句，因为之前 ellipse 对象的 MouseLeftButtonDown 事件中已经设置 e.Handled = true ，所以事件不会冒泡至此
            ellipse.Fill = new SolidColorBrush(Colors.Black);
        }
        


        // 是否正在捕获鼠标
        private bool _isMouseCaptured;

        // 鼠标垂直方向上的坐标
        private double _mouseY;

        // 鼠标水平方向上的坐标
        private double _mouseX;

        private void rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // MouseButtonEventArgs.GetPosition() - 鼠标相对于指定元素的坐标
            _mouseY = e.GetPosition(null).Y;
            _mouseX = e.GetPosition(null).X;

            // CaptureMouse() - 在指定的元素上捕获鼠标
            rectangle.CaptureMouse();
            _isMouseCaptured = true;
        }

        public void rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseCaptured)
            {
                // 移动前和移动后的鼠标 垂直方向 和 水平方向 的位置的差值
                double v = e.GetPosition(null).Y - _mouseY;
                double h = e.GetPosition(null).X - _mouseX;

                // 移动后的 rectangle 对象相对于 Canvas 的坐标
                double newTop = v + (double)rectangle.GetValue(Canvas.TopProperty);
                double newLeft = h + (double)rectangle.GetValue(Canvas.LeftProperty);

                // 设置 rectangle 对象的位置为新的坐标.
                rectangle.SetValue(Canvas.TopProperty, newTop);
                rectangle.SetValue(Canvas.LeftProperty, newLeft);

                // 更新鼠标的当前坐标
                _mouseY = e.GetPosition(null).Y;
                _mouseX = e.GetPosition(null).X;
            }
        }

        private void rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // ReleaseMouseCapture() - 如果指定的元素具有鼠标捕获，则释放该捕获
            rectangle.ReleaseMouseCapture();
            _isMouseCaptured = false;
        }
    }
}
