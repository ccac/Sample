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

namespace Silverlight20.Control
{
    public partial class ProgressBar : UserControl
    {
        Storyboard _loop = new Storyboard();
        int _count = 0;

        public ProgressBar()
        {
            InitializeComponent();

            ProgressBarDemo();
        }

        void ProgressBarDemo()
        {
            _loop.Duration = TimeSpan.FromMilliseconds(100d);
            _loop.Completed += new EventHandler(_loop_Completed);
            _loop.Begin();
        }

        void _loop_Completed(object sender, EventArgs e)
        {
            progressBar.Value = _count;
            lblPercent.Text = _count.ToString() + "%";

            if (_count > 100)
                _count = 0;
            else
                _count++;

            _loop.Begin();
        }
    }
}
