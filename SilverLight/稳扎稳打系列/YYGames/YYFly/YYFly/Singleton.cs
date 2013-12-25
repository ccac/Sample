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

namespace YYFly
{
    /// <summary>
    /// 单例模式的实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T>
        where T : new()
    {
        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }

        class SingletonCreator
        {
            internal static readonly T instance = new T();
        }
    }
}
