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

using System.ComponentModel;

namespace Silverlight20.Data
{
    public partial class NotifyProperty : UserControl
    {
        MyColor _myColor;

        public NotifyProperty()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(NotifyProperty_Loaded);
        }

        void NotifyProperty_Loaded(object sender, RoutedEventArgs e)
        {
            _myColor = new MyColor { Brush = new SolidColorBrush(Colors.Red) };

            // DataContext - FrameworkElement 做数据绑定时的数据上下文
            // 将 MyColor 对象绑定到 Ellipse
            ellipse.DataContext = _myColor;
        }

        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 鼠标按下后修改 MyColor 对象的属性
            // 如果MyColor实现了INotifyPropertyChanged接口，并且绑定目标的BindingMode为OneWay或者TwoWay的话则会自动通知到绑定目标
            if (_myColor.Brush.Color == Colors.Red)
                _myColor.Brush = new SolidColorBrush(Colors.Green);
            else
                _myColor.Brush = new SolidColorBrush(Colors.Red);
        }
    }

    /*
     * INotifyPropertyChanged - 向客户端发出某一属性值已更改的通知
     * 使用 OneWay 或者 TwoWay 的话必须要实现 INotifyPropertyChanged 接口
    */
    public class MyColor : INotifyPropertyChanged
    {
        private SolidColorBrush _brush;
        public SolidColorBrush Brush
        {
            get { return _brush; }
            set
            {
                _brush = value;
                if (PropertyChanged != null)
                {
                    // 触发 PropertyChanged 事件，事件参数为属性名称
                    PropertyChanged(this, new PropertyChangedEventArgs("Brush"));
                }
            }
        }

        // 声明一个 PropertyChanged 事件
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
