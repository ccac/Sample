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

namespace YYPiano.Controls
{
    public partial class Hits : UserControl
    {
        public Hits()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 总连击数
        /// </summary>
        public int HitsCount
        {
            get { return lblHitsCount.Text == "" ? 0 : int.Parse(lblHitsCount.Text); }
            set { lblHitsCount.Text = value.ToString(); }
        }

        /// <summary>
        /// 当前连击总数
        /// </summary>
        public int HitCounts
        {
            get { return lblHitsCount.Text == "" ? 0 : int.Parse(lblHitsCount.Text); }
        }

        /// <summary>
        /// 增加连击数
        /// </summary>
        public void AddHits()
        {
            if (lblHitsCount.Text == "")
                lblHitsCount.Text = "1";
            else
                lblHitsCount.Text = (int.Parse(lblHitsCount.Text) + 1).ToString();

            // 当连击数大于 2 时则显示相关的连击动画
            if (HitsCount > 2)
            {
                ani.Stop();
                ani.Begin();
            }
        }

        /// <summary>
        /// 连击数清零
        /// </summary>
        public void ClearHits()
        {
            lblHitsCount.Text = "0";
        }
    }
}
