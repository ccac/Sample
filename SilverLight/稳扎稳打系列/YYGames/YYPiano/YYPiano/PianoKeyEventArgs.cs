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

namespace YYPiano.Controls
{
    public class PianoKeyEventArgs : EventArgs
    {
        /// <summary>
        /// 键值
        /// </summary>
        public Key Key { get; set; }
    }
}
