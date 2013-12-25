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

using System.Windows.Browser;

namespace Silverlight20.WebPage
{
    /*
     * 脚本化的类必须是 public 的
     * 需要脚本化的属性、方法、事件要标记为 [ScriptableMember]
     * 如果类被标记为 [ScriptableType]，则意味着其属性、方法、事件都是ScriptableMemberAttribute
     */

    /// <summary>
    /// 用于演示脚本化的类
    /// </summary>
    // [ScriptableType]
    public class Scriptable
    {
        /// <summary>
        /// 当前服务端的时间
        /// </summary>
        [ScriptableMember]
        public DateTime CurrentTime { get; set; }

        /// <summary>
        /// Hello 方法
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        [ScriptableMember]
        public string Hello(string name)
        {
            return string.Format("Hello: {0}", name);
        }
        
        /// <summary>
        /// 开始事件
        /// </summary>
        [ScriptableMember]
        public event EventHandler<StartEventArgs> Start;

        /// <summary>
        /// 触发开始事件所调用的方法
        /// </summary>
        /// <param name="dt"></param>
        public void OnStart(DateTime dt)
        {
            if (Start != null)
            {
                Start(this, new StartEventArgs()
                {
                    CurrentTime = dt
                });
            }
        }

    }

    /// <summary>
    /// 开始事件的 EventArgs
    /// </summary>
    public class StartEventArgs : EventArgs
    {
        /// <summary>
        /// 当前服务端的时间
        /// </summary>
        [ScriptableMember]
        public DateTime CurrentTime { get; set; }
    }
}
