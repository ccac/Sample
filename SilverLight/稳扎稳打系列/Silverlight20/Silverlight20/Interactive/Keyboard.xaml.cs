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
    public partial class Keyboard : UserControl
    {
        public Keyboard()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Keyboard_Loaded);

            // 为 UserControl 注册 KeyUp 事件
            userControl.KeyUp += new KeyEventHandler(userControl_KeyUp);
        }

        void Keyboard_Loaded(object sender, RoutedEventArgs e)
        {
            // 让 UserControl 获得焦点，这样该 UserControl 内的元素才能监听到键盘事件
            userControl.Focus();
        }

        private void userControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 获取 textBox 对象的相对于 Canvas 的 x坐标 和 y坐标
            double x = (double)textBox.GetValue(Canvas.LeftProperty);
            double y = (double)textBox.GetValue(Canvas.TopProperty);

            // KeyEventArgs.Key - 与事件相关的键盘的按键 [System.Windows.Input.Key枚举]
            switch (e.Key)
            {
                // 按 Up 键后 textBox 对象向 上 移动 1 个像素
                // Up 键所对应的 e.PlatformKeyCode == 38 
                // 当获得的 e.Key == Key.Unknown 时，可以使用 e.PlatformKeyCode 来确定用户所按的键
                case Key.Up:
                    textBox.SetValue(Canvas.TopProperty, y - 1);
                    break;

                // 按 Down 键后 textBox 对象向 下 移动 1 个像素
                // Down 键所对应的 e.PlatformKeyCode == 40
                case Key.Down:
                    textBox.SetValue(Canvas.TopProperty, y + 1);
                    break;

                // 按 Left 键后 textBox 对象向 左 移动 1 个像素
                // Left 键所对应的 e.PlatformKeyCode == 37
                case Key.Left:
                    textBox.SetValue(Canvas.LeftProperty, x - 1);
                    break;

                // 按 Right 键后 textBox 对象向 右 移动 1 个像素
                // Right 键所对应的 e.PlatformKeyCode == 39 
                case Key.Right:
                    textBox.SetValue(Canvas.LeftProperty, x + 1);
                    break;

                default:
                    break;
            }

            // 同上：Key.W - 向上移动； Key.S - 向下移动； Key.A - 向左移动； Key.D - 向右移动
            switch (e.Key)
            {
                // KeyEventArgs.Handled - 是否处理过此事件

                // 如果在文本框内敲 W ，那么文本框会向上移动，而且文本框内也会被输入 W
                // 如果只想移动文本框，而不输入 W ，那么可以设置 KeyEventArgs.Handled = true 告知此事件已经被处理完毕
                case Key.W:
                    textBox.SetValue(Canvas.TopProperty, y - 1);
                    e.Handled = true;
                    break;
                case Key.S:
                    textBox.SetValue(Canvas.TopProperty, y + 1);
                    e.Handled = true;
                    break;
                case Key.A:
                    textBox.SetValue(Canvas.LeftProperty, x - 1);
                    e.Handled = true;
                    break;
                case Key.D:
                    textBox.SetValue(Canvas.LeftProperty, x + 1);
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void userControl_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            System.Windows.Input.Keyboard.Modifiers - 当前按下的辅助键 [System.Windows.Input.ModifierKeys枚举]
                ModifierKeys.None - 无
                ModifierKeys.Alt - Alt 键
                ModifierKeys.Control - Ctrl 键
                ModifierKeys.Shift - Shift 键
                ModifierKeys.Windows - Windows 键
                ModifierKeys.Apple - Apple 键（苹果电脑）
            */

            // 按 Ctrl + M 则将 textBox 的位置设置为其初始位置
            if (System.Windows.Input.Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.M)
            {
                textBox.SetValue(Canvas.LeftProperty, 0d);
                textBox.SetValue(Canvas.TopProperty, 0d);
            }
        }
    }
}
