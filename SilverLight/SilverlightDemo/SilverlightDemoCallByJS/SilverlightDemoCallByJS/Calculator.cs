using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Windows.Browser;

namespace SilverlightDemoCallByJS
{
    [ScriptableType]
    public class Calculator
    {
        [ScriptableMember]
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
