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

namespace YYPiano
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// System.Windows.Input.Key 转换为 字符 A-Z
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static char ToChar(this Key key)
        {
            return (char)((int)key + 35);
        }

        /// <summary>
        /// 字符 A-Z 转换为 System.Windows.Input.Key
        /// </summary>
        /// <param name="c">字符 A-Z</param>
        /// <returns></returns>
        public static Key ToKey(this char c)
        {
            var ascii = (int)c;
            if (ascii > 91 || ascii < 65)
                return Key.A;

            return (Key)((int)c - 35);
        }
    }
}
