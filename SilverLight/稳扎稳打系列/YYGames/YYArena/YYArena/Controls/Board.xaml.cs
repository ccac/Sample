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

namespace YYArena.Controls
{
    public partial class Board : UserControl
    {
        private int _fps;
        private DateTime _prevDateTime;

        public Board()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Board_Loaded);
        }

        void Board_Loaded(object sender, RoutedEventArgs e)
        {
            root.Width = App.AreaWidth - 10;

            _fps = 0;
            _prevDateTime = DateTime.Now;

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        /// <summary>
        /// 计算 fps
        /// </summary>
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            _fps++;

            if ((DateTime.Now - _prevDateTime).TotalSeconds >= 1)
            {
                lblFps.Text = _fps.ToString();
                _fps = 0;
                _prevDateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            HP = 1d;
            Level = 1;
            Score = 0;
        }

        /// <summary>
        /// 生命力 0 - 1
        /// </summary>
        public double HP
        {
            get { return rg.Rect.Width / 120; }
            set { rg.Rect = new Rect(0, 0, value * 120, 6); }
        }

        /// <summary>
        /// 级别
        /// </summary>
        public int Level
        {
            get { return int.Parse(lblLevel.Text); }
            set { lblLevel.Text = value.ToString(); }
        }

        /// <summary>
        /// 得分
        /// </summary>
        public int Score
        {
            get { return int.Parse(lblScore.Text); }
            set { lblScore.Text = value.ToString(); }
        }
    }
}
