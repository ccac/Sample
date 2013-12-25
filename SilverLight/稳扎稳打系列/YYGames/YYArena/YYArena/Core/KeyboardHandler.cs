using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Collections.Generic;

namespace YYArena.Core
{
    /// <summary>
    /// 键盘事件处理器
    /// </summary>
    public class KeyboardHandler
    {
        private bool[] _isPressed = new bool[256];
        private Control _targetControl;

        /// <summary>
        /// 在指定的控件上监测键盘事件
        /// </summary>
        public KeyboardHandler(Control targetControl)
        {
            _targetControl = targetControl;
        }

        /// <summary>
        /// 清除所有按键信息
        /// </summary>
        public void ClearKeyPresses()
        {
            for (int i = 0; i < 255; i++)
            {
                _isPressed[i] = false;
            }
        }

        /// <summary>
        /// 开始监测
        /// </summary>
        public void Attach()
        {
            ClearKeyPresses();

            _targetControl.KeyDown += new KeyEventHandler(_targetControl_KeyDown);
            _targetControl.KeyUp += new KeyEventHandler(_targetControl_KeyUp);
            _targetControl.LostFocus += new RoutedEventHandler(_targetControl_LostFocus);
        }

        /// <summary>
        /// 取消监测
        /// </summary>
        public void Detach()
        {
            _targetControl.KeyDown -= new KeyEventHandler(_targetControl_KeyDown);
            _targetControl.KeyUp -= new KeyEventHandler(_targetControl_KeyUp);
            _targetControl.LostFocus -= new RoutedEventHandler(_targetControl_LostFocus);

            ClearKeyPresses();
        }

        void _targetControl_KeyDown(object sender, KeyEventArgs e)
        {
            _isPressed[(int)e.Key] = true;
        }

        void _targetControl_KeyUp(object sender, KeyEventArgs e)
        {
            _isPressed[(int)e.Key] = false;
        }

        void _targetControl_LostFocus(object sender, EventArgs e)
        {
            ClearKeyPresses();            
        }

        /// <summary>
        /// 是否正在按指定的某个键
        /// </summary>
        public bool IsKeyPressed(Key key)
        {
            int i = (int)key;

            if (i < 0 || i > 82) 
                return false;

            return _isPressed[i];
        }

        /// <summary>
        /// 是否正在按指定的某些键
        /// </summary>
        public bool IsKeyPressed(List<Key> keys)
        {
            if (keys == null || keys.Count == 0)
                return false;

            foreach (var key in keys)
            {
                if (!IsKeyPressed(key))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否正在按指定的按键集合中的任意键
        /// </summary>
        public bool AnyKeyPressed(List<Key> keys)
        {
            if (keys == null || keys.Count == 0)
                return false;

            foreach (var key in keys)
            {
                if (IsKeyPressed(key))
                    return true;
            }

            return false;
        }
    }
}
