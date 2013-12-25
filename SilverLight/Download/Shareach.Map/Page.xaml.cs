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
    public partial class Page : UserControl
    {
        private MapLayer m_mapLayer = null;
        private MapNavi m_mapNav = null;
        private bool m_loade = false;
        public Page()
        {
            InitializeComponent();
            m_mapLayer = new MapLayer(this);
            this.LayoutRoot.Children.Add(m_mapLayer);

            m_mapNav = new MapNavi(m_mapLayer);
            LayoutRoot.Children.Add(m_mapNav);

            this.SizeChanged +=new SizeChangedEventHandler(Page_SizeChanged);

        }
        void Page_SizeChanged(object sender, EventArgs e)
        {
            if (!m_loade)
            {
                m_loade = true;
                m_mapLayer.MoveTo(3, 55000, 63000);
            }
            m_mapNav.ChangePos();
        }
    }
}
