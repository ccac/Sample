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
    public partial class GameView : UserControl
    {
        GameViewModel _model = null;

        public GameView()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(GameView_Loaded);
        }

        void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = Resources["gameViewModel"] as GameViewModel;
        }

        private void cardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _model.CheckedCard(sender as CardControl);
        }

        public GameViewModel Model
        {
            get { return _model; }
        }
    }
}