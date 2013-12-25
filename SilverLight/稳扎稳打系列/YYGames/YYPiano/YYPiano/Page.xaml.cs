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

using YYPiano.Controls;
using System.Windows.Resources;
using System.Xml.Linq;

namespace YYPiano
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            keyboard.Click += new EventHandler<PianoKeyEventArgs>(keyboard_Click);
            book.Score += new EventHandler<EventArgs>(book_Score);
            book.Lost += new EventHandler<EventArgs>(book_Lost);

            // 加载乐谱编码 XML
            StreamResourceInfo sri =
                Application.GetResourceStream(new Uri("MusicBook.xml", UriKind.Relative));
            var xmlMenus = from p in XElement.Load(sri.Stream).Elements("MusicBook")
                    select new { Name = ((string)p.Element("Name")).Trim(), Code = ((string)p.Element("Code")).Trim() };

            // 转换乐谱编码的 XML 为一个 MenuModel 集合 
            List<MenuModel> menus = new List<MenuModel>();
            int i = 1;
            foreach (var xmlMenu in xmlMenus)
            {
                menus.Add(new MenuModel() { Name = i.ToString() + "、" + xmlMenu.Name, Code = xmlMenu.Code });
                i++;
            }

            // 绑定乐谱到 ListBox
            menu.ItemsSource = menus;
        }

        /// <summary>
        /// 更换乐谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            score.ClearScore();
            hits.ClearHits();
            var result = book.Start((e.AddedItems[0] as MenuModel).Code);

            if (!result)
                MessageBox.Show("解析乐谱失败，请确保乐谱格式正确");
        }

        /// <summary>
        /// 按键敲击错误或未及时敲击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void book_Lost(object sender, EventArgs e)
        {
            hits.ClearHits();
        }

        /// <summary>
        /// 按键敲击正确，计分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void book_Score(object sender, EventArgs e)
        {
            hits.AddHits();
            score.AddScore(13 * hits.HitsCount);
        }

        /// <summary>
        /// 敲击鼠标按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void keyboard_Click(object sender, PianoKeyEventArgs e)
        {
            keyboard.Play(e.Key);
            book.Play(e.Key);
        }

        /// <summary>
        /// 敲击键盘按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            keyboard.Play(e.Key);
            book.Play(e.Key);
        }

        private void btnMyBook_Click(object sender, RoutedEventArgs e)
        {
            score.ClearScore();
            hits.ClearHits();
            var result = book.Start(txtMyBook.Text.Trim());

            if (!result)
                MessageBox.Show("解析乐谱失败，请确保乐谱格式正确");
        }
    }
}
