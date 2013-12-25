/********************************************************************************
 * 地图层, 用于地图操作
 * 作者: Shareach
 * Email:yinpengxiang@hotmail.com
 * 网址: http://sps.shareach.com
 * 欢迎访问 http://www.fangtoo.com , Flex版的地图,
 * 这个是Silverlight 2 Bate 2 版本编译开发的, 此代码仅仅作为技术交流使用, 
 * 请不要能够用于商业用途, 著名出处, 欢迎讨论指教
 * 还有很多东西不会用, 刚看了半天就开干的, 纯粹使用C# 的知识 和 SDK文档
 * *............................................................................*/
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

namespace Shareach.Map
{
    public partial class MapNavi : UserControl
    {
        //↑↓←→⊙＋－
        private MapLayer m_parent;
        public MapNavi(MapLayer parent)
        {
            InitializeComponent();
            m_parent = parent;
        }

        public void ChangePos()
        {
            this.Transform.X = -m_parent.RenderSize.Width / 2 + this.RenderSize.Width/2;
            this.Transform.Y = -m_parent.RenderSize.Height / 2 + this.RenderSize.Height/2;
        }

        void UpMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic( m_parent.PointTX, m_parent.PointTY - 100);
        }
        void UpLeftMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic(m_parent.PointTX-100, m_parent.PointTY - 100);
        }
        void UpRightMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic(m_parent.PointTX+100, m_parent.PointTY - 100);
        }
        void DownMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic(m_parent.PointTX, m_parent.PointTY + 100);
        }
        void DownLeftMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic(m_parent.PointTX-100, m_parent.PointTY + 100);
        }
        void DownRightMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic(m_parent.PointTX+100, m_parent.PointTY + 100);
        }
        void LeftMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic(m_parent.PointTX - 100, m_parent.PointTY);
        }
        void RightMove(object sender, RoutedEventArgs e)
        {
            m_parent.DownLoadMapPic( m_parent.PointTX + 100, m_parent.PointTY);
        }
        void ZoomIn(object sender, RoutedEventArgs e)
        {
            if (m_parent.Zoom <= 1) return;
            this.SliderMap.Value = m_parent.Zoom - 1;
        }
        void ZoomOut(object sender, RoutedEventArgs e)
        {
            if (m_parent.Zoom >= 4) return;
            this.SliderMap.Value = m_parent.Zoom + 1;
        }

        private void SliderMapValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_parent == null) return;
            m_parent.Zoom = Convert.ToInt32(e.NewValue);
        }
    }
}
