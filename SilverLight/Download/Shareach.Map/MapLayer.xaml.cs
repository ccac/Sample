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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.IO;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;

namespace Shareach.Map
{
    public partial class MapLayer : UserControl
    {
        private Dictionary<string, object> m_currMapPics = new Dictionary<string, object>();
        //private List<Ents.MapPicItem> m_currImg = new List<Shareach.Map.Ents.MapPicItem>();
        private BitmapImage m_noPic = null;

        private bool m_dragging = false;
        private Point m_orgPoint;

        private Page m_parent = null;
        private string m_mapPath = "http://{3}:{4}/mappic/png{0}/{1}/{2}.jpg";
        private int m_zoom = 4;
        private const int SCALE_X = 256;
        private const int SCALE_Y = 256;
        /// <summary>
        /// 当前放大级别
        /// </summary>
        public int Zoom
        {
            get { return m_zoom; }
            set {
                m_zoom = value;
                MoveTo(m_pointCX, m_pointCY);
            }
        }

        private double m_pointCX = 0;
        private double m_pointCY = 0;
        /// <summary>
        /// 左上角坐标X
        /// </summary>
        private double m_pointTX = 0;
        /// <summary>
        /// 左上角坐标X
        /// </summary>
        public double PointTX
        {
            get { return m_pointTX; }
            set
            {
                m_xt = Convert.ToInt64(Math.Floor(value / SCALE_X));
                m_pointTX = value;
                m_pointCX = (m_pointTX + this.RenderSize.Width / 2) * Math.Pow(2, m_zoom);
            }
        }

        /// <summary>
        /// 左上角坐标Y
        /// </summary>
        private double m_pointTY = 0;
        /// <summary>
        /// 左上角坐标Y
        /// </summary>
        public double PointTY
        {
            get { return m_pointTY; }
            set
            {
                m_yt = Convert.ToInt64(Math.Floor(value / SCALE_Y));
                m_pointTY = value;
                m_pointCY = (m_pointTY + this.RenderSize.Height / 2) * Math.Pow(2, m_zoom);
            }
        }

        private long m_xt = 4;
        private long m_yt = 4;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        public MapLayer(Page parent):base()
        {
            InitializeComponent();
            m_noPic = new BitmapImage(new Uri(string.Format("http://{0}:{1}/mappic/0.jpg", Application.Current.Host.Source.Host, Application.Current.Host.Source.Port)));
            m_parent = parent;
            MouseMove += new MouseEventHandler(Map_MouseMove);
            MouseLeftButtonDown += new MouseButtonEventHandler(Map_MouseLeftButtonDown);
            MouseLeftButtonUp += new MouseButtonEventHandler(Map_MouseLeftButtonUp);
            MouseLeave +=new MouseEventHandler(Map_MouseLeave);
            SizeChanged += new SizeChangedEventHandler(Map_SizeChanged);
        }
        /// <summary>
        /// 装载地图
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        public void LoadMap(int zoom, double tx, double ty)
        {
            m_zoom = zoom;
            DownLoadMapPic(tx / Math.Pow(2, zoom), ty / Math.Pow(2, zoom));
        }
        /// <summary>
        /// 移动到
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(double cx, double cy)
        {
            double tx = (cx / Math.Pow(2, m_zoom)) - this.RenderSize.Width / 2;
            double ty = (cy / Math.Pow(2, m_zoom)) - this.RenderSize.Height / 2;
            DownLoadMapPic(tx, ty);
        }
        /// <summary>
        /// 移动并缩放
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        public void MoveTo(int zoom, double cx, double cy)
        {
            m_zoom = zoom;
            MoveTo(cx, cy);
        }

        /// <summary>
        /// 装载指定区域的地图
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void DownLoadMapPic(double tx, double ty)
        {
            PointTX = tx;
            PointTY = ty;

            this.translateTransform.X = -m_pointTX;
            this.translateTransform.Y = -m_pointTY;

            int w = Convert.ToInt32(Math.Floor(this.RenderSize.Width / SCALE_X));
            int h = Convert.ToInt32(Math.Floor(this.RenderSize.Height / SCALE_X));

            Dictionary<string, object> lst = m_currMapPics;
            m_currMapPics = new Dictionary<string, object>();
            //左右上下多加载一屏
            for (long i = m_xt-1; i < m_xt + w+2; i++)
            {
                for (long j = m_yt-1; j < m_yt + h+2; j++)
                {
                    bool needCreate = true;
                    string sK = string.Format("{0}-{1}", i, j);
                    if (lst.Keys.Contains(sK))
                    {
                        m_currMapPics.Add(sK, lst[sK]);
                        lst[sK] = null;
                        lst.Remove(sK);
                        needCreate = false;
                    }
                    if (!needCreate) continue;
                    WebClient wc = new WebClient();
                    wc.OpenReadCompleted += new OpenReadCompletedEventHandler(Img_OpenReadCompleted);
                    wc.OpenReadAsync(new Uri(string.Format(m_mapPath, m_zoom, i, j, Application.Current.Host.Source.Host, Application.Current.Host.Source.Port)));
                    //System.Diagnostics.Debug.WriteLine(string.Format(m_mapPath, m_zoom, i, j, Application.Current.Host.Source.Host, Application.Current.Host.Source.Port));
                    //Ents.MapPicItem mpiNew = new Shareach.Map.Ents.MapPicItem(i, j, null);
                    m_currMapPics.Add(sK, null);
                }
            }
            //释放资源
            while(lst.Keys.Count>0)
            {
                string sK = lst.Keys.First();
                if (lst[sK] != null)
                {
                    this.LayoutRoot.Children.Remove((Image)lst[sK]);
                    lst[sK] = null;
                }
                lst.Remove(sK);
            }
            GC.Collect();
        }
        /// <summary>
        /// 下载完显示图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Img_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            Stream str = e.Result;
            if (e.Error == null)
            {
                Regex reg = new Regex(@"png(\d+)/([\-0-9]+)/([\-0-9]+).jpg");
                MatchCollection mc = reg.Matches(e.Address.AbsolutePath);
                if (mc.Count>0 && mc[0].Groups.Count==4)
                {
                    Image img = new Image();
                    if (str != null)
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(e.Result as Stream);
                        img.Source = bi;
                    }
                    else
                    {
                        try{ img.Source = new BitmapImage(m_noPic.UriSource); } catch{}
                    }
                    img.Stretch = Stretch.None;
                    //img.RenderTransformOrigin = new Point(double.Parse(mc[0].Groups[2].Value) * 256,double.Parse(mc[0].Groups[3].Value) * 256);
                    TranslateTransform imgTransForm = new TranslateTransform();
                    long x = long.Parse(mc[0].Groups[2].Value);
                    long y = long.Parse(mc[0].Groups[3].Value);

                    string sK = string.Format("{0}-{1}", x, y);
                    if (m_currMapPics.Keys.Contains(sK))
                    {
                        m_currMapPics[sK] = img;
                    }
                    imgTransForm.X = x * 256;
                    imgTransForm.Y = y * 256;
                    img.RenderTransform = imgTransForm;

                    this.LayoutRoot.Children.Add(img);
                    img.Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_dragging)
            {//拖动地图
                var position = e.GetPosition(null);
                //System.Diagnostics.Debug.WriteLine("position:" + position);
                this.translateTransform.X += position.X - m_orgPoint.X;
                this.translateTransform.Y += position.Y - m_orgPoint.Y;
                //System.Diagnostics.Debug.WriteLine("translateTransform:" + translateTransform.X + "," + translateTransform.Y);
                m_orgPoint = position;
            }
        }
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_orgPoint = e.GetPosition(null);
            //System.Diagnostics.Debug.WriteLine("m_orgPoint:" + m_orgPoint);
            //表示开始拖动, 当鼠标松开和离开时表示结束拖动
            m_dragging = true;
        }
        /// <summary>
        /// 鼠标松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Map_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //表示结束拖动
            if (m_dragging) ReloadMap();
            m_dragging = false;
        }
        /// <summary>
        /// 离开了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Map_MouseLeave(object sender, MouseEventArgs e)
        {
            //表示结束拖动
            if (m_dragging) ReloadMap();
            m_dragging = false;
        }
        void Map_SizeChanged(object sender, EventArgs e)
        {//中心点不变
            MoveTo(m_pointCX, m_pointCY);
        }
        /// <summary>
        /// 加载地图
        /// </summary>
        void ReloadMap()
        {
            DownLoadMapPic(-this.translateTransform.X, -this.translateTransform.Y);
        }
    }
}
