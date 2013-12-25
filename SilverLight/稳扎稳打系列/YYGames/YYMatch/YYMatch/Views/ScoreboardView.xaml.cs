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

using YYMatch.Controls;
using YYMatch.ViewModels;
using YYMatch.Models;

namespace YYMatch.Views
{
    public partial class ScoreboardView : UserControl
    {
        ScoreboardViewModel _model = null;

        public ScoreboardView()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(ScoreboardView_Loaded);
        }

        void ScoreboardView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = Resources["scoreboardViewModel"] as ScoreboardViewModel;
        }

        public ScoreboardViewModel Model
        {
            get { return _model; }
        }

        public event EventHandler Replace;
        public void OnReplace()
        {
            if (Replace != null)
                Replace(this, EventArgs.Empty);
        }

        private void replace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnReplace();
        }
    }
}
