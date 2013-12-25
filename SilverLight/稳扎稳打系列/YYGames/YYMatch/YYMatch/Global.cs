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

namespace YYMatch
{
    public static class Global
    {
        public static int ContainerRows
        {
            get { return 12; }
        }

        public static int ContainerColumns
        {
            get { return 16; }
        }

        public static string EmptyImageName
        {
            get { return "00"; }
        }

        public static int ImageCount
        {
            get { return 18; }
        }
    }
}
