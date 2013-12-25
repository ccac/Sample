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
using System.Windows.Data;

namespace Silverlight20.Data
{
    public partial class Conversion : UserControl
    {
        MyColorEnum _myColorEnum;

        public Conversion()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Conversion_Loaded);
        }

        void Conversion_Loaded(object sender, RoutedEventArgs e)
        {
            _myColorEnum = new MyColorEnum() { ColorEnum = ColorEnum.Red };

            // DataContext - FrameworkElement 做数据绑定时的数据上下文
            // 将 MyColorEnum 对象绑定到 Ellipse
            ellipse.DataContext = _myColorEnum;
        }

        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 鼠标按下后修改 MyColorEnum 对象的属性
            if (_myColorEnum.ColorEnum == ColorEnum.Red)
                _myColorEnum.ColorEnum = ColorEnum.Green;
            else
                _myColorEnum.ColorEnum = ColorEnum.Red;
        }
    }

    /*
     * IValueConverter - 值转换接口，将一个类型的值转换为另一个类型的值。它提供了一种将自定义逻辑应用于绑定的方式
     *     Convert - 正向转换器。将值从数据源传给绑定目标时，数据绑定引擎会调用此方法
     *     ConvertBack - 反向转换器。将值从绑定目标传给数据源时，数据绑定引擎会调用此方法
    */
    public class ColorEnumToBrush : IValueConverter
    {
        /// <summary>
        /// 正向转换器。将值从数据源传给绑定目标时，数据绑定引擎会调用此方法
        /// </summary>
        /// <param name="value">转换之前的值</param>
        /// <param name="targetType">转换之后的类型</param>
        /// <param name="parameter">转换器所使用的参数</param>
        /// <param name="culture">转换器所使用的区域信息</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // 将 ColorEnum 类型的值转换为 SolidColorBrush 类型的值

            // parameter == 10

            ColorEnum color = (ColorEnum)value;

            SolidColorBrush brush = new SolidColorBrush(Colors.Red);

            if (color == ColorEnum.Green)
                brush = new SolidColorBrush(Colors.Green);

            return brush;
        }

        /// <summary>
        /// 反向转换器。将值从绑定目标传给数据源时，数据绑定引擎会调用此方法
        /// </summary>
        /// <param name="value">转换之前的值</param>
        /// <param name="targetType">转换之后的类型</param>
        /// <param name="parameter">转换器所使用的参数</param>
        /// <param name="culture">转换器所使用的区域信息</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /*
     * INotifyPropertyChanged - 向客户端发出某一属性值已更改的通知
     * 使用 OneWay 或者 TwoWay 的话必须要实现 INotifyPropertyChanged 接口
    */
    public class MyColorEnum : INotifyPropertyChanged
    {
        private ColorEnum _colorEnum;
        public ColorEnum ColorEnum
        {
            get { return _colorEnum; }
            set
            {
                _colorEnum = value;
                if (PropertyChanged != null)
                {
                    // 触发 PropertyChanged 事件，事件参数为属性名称
                    PropertyChanged(this, new PropertyChangedEventArgs("ColorEnum"));
                }
            }
        }

        // 声明一个 PropertyChanged 事件
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// 自定义的 ColorEnum 枚举
    /// </summary>
    public enum ColorEnum
    {
        Red,
        Green
    }
}
