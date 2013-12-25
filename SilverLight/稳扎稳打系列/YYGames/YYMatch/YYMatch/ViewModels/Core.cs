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

using YYMatch.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace YYMatch.ViewModels
{
    /// <summary>
    /// 连连看核心模块
    /// </summary>
    public class Core : INotifyPropertyChanged
    {
        ObservableCollection<CardModel> _cards = null;
        int _rows = 0;
        int _columns = 0;
        SynchronizationContext _syncContext = null;

        public Core()
        {
            // 在容器上布满空的卡片
            _cards = new ObservableCollection<CardModel>();
            for (int i = 0; i < Global.ContainerColumns * Global.ContainerRows; i++)
            {
                _cards.Add(new CardModel("00", i));
            }

            _syncContext = SynchronizationContext.Current;
        }

        public void Start(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;

            InitCard();
        }

        private ObservableCollection<CardModel> InitCard()
        {
            Random r = new Random();
            // 卡片集合在容器内的范围
            int minX = (Global.ContainerColumns - _columns) / 2;
            int maxX = minX + _columns - 1;
            int minY = (Global.ContainerRows - _rows) / 2;
            int maxY = minY + _rows - 1;

            for (int x = 0; x < Global.ContainerColumns; x++)
            {
                for (int y = 0; y < Global.ContainerRows; y++)
                {
                    // 18 张图随机排列
                    string imageName = r.Next(1, Global.ImageCount + 1).ToString().PadLeft(2, '0');
                    var cardPoint = new CardPoint(x, y);

                    if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                    {
                        _cards[cardPoint.Position] = new CardModel(imageName, cardPoint.Position);
                    }
                }
            }

            // 相同的卡片个数为奇数的集合
            var oddImages = _cards.Where(p => p.ImageName != Global.EmptyImageName)
                .GroupBy(p => p.ImageName)
                .Select(p => new { ImageName = p.Key, Count = p.Count() })
                .Where(p => p.Count % 2 > 0)
                .ToList();

            // 如果 oddImages 集合的成员为奇数个（保证容器容量为偶数个则不可能出现这种情况）
            if (oddImages.Count() % 2 > 0)
            {
                throw new Exception("无法初始化程序");
            }
            else
            {
                // 在集合中将所有的个数为奇数的卡片各自取出一个放到 temp 中
                // 将 temp 一刀切，使其右半部分的卡片的 ImageName 依次赋值为左半部分的卡片的 ImageName
                // 由此保证相同的卡片均为偶数个
                List<CardModel> tempCards = new List<CardModel>();
                for (int i = 0; i < oddImages.Count(); i++)
                {
                    if (i < oddImages.Count() / 2)
                    {
                        var tempCard = _cards.Last(p => p.ImageName == oddImages.ElementAt(i).ImageName);
                        tempCards.Add(tempCard);
                    }
                    else
                    {
                        var tempCard = _cards.Last(p => p.ImageName == oddImages.ElementAt(i).ImageName);
                        _cards[tempCard.Position].ImageName = tempCards[i - oddImages.Count() / 2].ImageName;
                    }
                }
            }

            if (!IsActive())
                Replace();

            return _cards;
        }

        /// <summary>
        /// 判断两卡片是否可消
        /// </summary>
        public bool Match(CardModel c1, CardModel c2, bool removeCard)
        {
            bool result = false;

            if (c1.ImageName != c2.ImageName
                || c1.ImageName == Global.EmptyImageName
                || c2.ImageName == Global.EmptyImageName
                || c1.Position == c2.Position)
                return false;

            // 如果可消的话，则 point1, point2, point3, point4 会组成消去两卡片的路径（共4个点）
            CardPoint point1 = new CardPoint(0);
            CardPoint point2 = new CardPoint(0);
            CardPoint point3 = new CardPoint(0);
            CardPoint point4 = new CardPoint(0);
            // 最小路径长度
            int minLength = int.MaxValue;

            CardPoint p1 = new CardPoint(c1.Position);
            CardPoint p2 = new CardPoint(c2.Position);

            var p1xs = GetXPositions(p1);
            var p1ys = GetYPositions(p1);
            var p2xs = GetXPositions(p2);
            var p2ys = GetYPositions(p2);

            // 在两点各自的 X 轴方向上找可消点（两个可消点的 X 坐标相等）
            var pxs = from p1x in p1xs
                      join p2x in p2xs
                      on p1x.X equals p2x.X
                      select new { p1x, p2x };
            foreach (var px in pxs)
            {
                if (MatchLine(p1, px.p1x) && MatchLine(px.p1x, px.p2x) && MatchLine(px.p2x, p2))
                {
                    int length = Math.Abs(p1.X - px.p1x.X) + Math.Abs(px.p1x.Y - px.p2x.Y) + Math.Abs(px.p2x.X - p2.X);

                    // 查找最短连接路径
                    if (length < minLength)
                    {
                        minLength = length;
                        point1 = p1;
                        point2 = px.p1x;
                        point3 = px.p2x;
                        point4 = p2;
                    }
                    result = true;
                }
            }

            // 在两点各自的 Y 轴方向上找可消点（两个可消点的 Y 坐标相等）
            var pys = from p1y in p1ys
                      join p2y in p2ys
                      on p1y.Y equals p2y.Y
                      select new { p1y, p2y };
            foreach (var py in pys)
            {
                if (MatchLine(p1, py.p1y) && MatchLine(py.p1y, py.p2y) && MatchLine(py.p2y, p2))
                {
                    int length = Math.Abs(p1.Y - py.p1y.Y) + Math.Abs(py.p1y.X - py.p2y.X) + Math.Abs(py.p2y.Y - p2.Y);

                    // 查找最短连接路径
                    if (length < minLength)
                    {
                        minLength = length;
                        point1 = p1;
                        point2 = py.p1y;
                        point3 = py.p2y;
                        point4 = p2;
                    }
                    result = true;
                }
            }

            if (removeCard && result)
            {
                RemoveCard(c1, c2, point1, point2, point3, point4);
            }

            return result;
        }

        /// <summary>
        /// 直线上的两个 CardPoint 是否可以消去
        /// </summary>
        private bool MatchLine(CardPoint p1, CardPoint p2)
        {
            if (p1.X != p2.X && p2.Y != p2.Y)
                return false;

            var range = _cards.Where(p =>
                p.Position > Math.Min(p1.Position, p2.Position)
                && p.Position < Math.Max(p1.Position, p2.Position));

            if (p1.X == p2.X)
            {
                range = range.Where(p => (p.Position - p1.Position) % Global.ContainerColumns == 0);
            };

            if (range.Count() == 0 || range.All(p => p.ImageName == Global.EmptyImageName))
                return true;

            return false;
        }

        /// <summary>
        /// 获取指定的 CardPoint 的 X 轴方向上的所有 ImageName 为 Global.EmptyImageName 的 CardPoint 集合
        /// </summary>
        private List<CardPoint> GetXPositions(CardPoint p)
        {
            var result = new List<CardPoint>() { p };
            for (int i = 0; i < Global.ContainerColumns; i++)
            {
                var point = new CardPoint(p.Y * Global.ContainerColumns + i);
                if (_cards[point.Position].ImageName == Global.EmptyImageName)
                    result.Add(point);
            }

            return result;
        }

        /// <summary>
        /// 获取指定的 CardPoint 的 Y 轴方向上的所有 ImageName 为 Global.EmptyImageName 的 CardPoint 集合
        /// </summary>
        private List<CardPoint> GetYPositions(CardPoint p)
        {
            var result = new List<CardPoint>() { p };
            for (int i = 0; i < Global.ContainerRows; i++)
            {
                var point = new CardPoint(i * Global.ContainerColumns + p.X);
                if (_cards[point.Position].ImageName == Global.EmptyImageName)
                    result.Add(point);
            }

            return result;
        }

        /// <summary>
        /// 执行消去两个卡片的任务
        /// 参数为：两个卡片对象和消去路径的四个点（此四个点需按路径方向依次传入）
        /// </summary>
        private void RemoveCard(CardModel c1, CardModel c2, CardPoint p1, CardPoint p2, CardPoint p3, CardPoint p4)
        {
            _cards[c1.Position] = new CardModel(Global.EmptyImageName, c1.Position);
            _cards[c2.Position] = new CardModel(Global.EmptyImageName, c2.Position);

            Points.Clear();
            Points.Add(CardPoint2Point(p1));
            Points.Add(CardPoint2Point(p2));
            Points.Add(CardPoint2Point(p3));
            Points.Add(CardPoint2Point(p4));

            Thread thread = new Thread
            (
                x =>
                {
                    Thread.Sleep(100);

                    _syncContext.Post
                    (
                        y =>
                        {
                            Points.Clear();
                        },
                        null
                    );
                }
            );
            thread.Start();
        }

        /// <summary>
        /// CardPoint 转换成坐标位置 Point
        /// </summary>
        private Point CardPoint2Point(CardPoint cardPoint)
        {
            // 38 - 每个正方形卡片的边长
            // 19 - 边长 / 2
            // cardPoint.X * 2 - 卡片的 Padding 为 1 ，所以卡片间的间距为 2
            var x = cardPoint.X * 38 + 19 + cardPoint.X * 2;
            var y = cardPoint.Y * 38 + 19 + cardPoint.Y * 2;

            return new Point(x, y);
        }

        /// <summary>
        /// 检查当前是否仍然有可消除的卡片
        /// </summary>
        public bool IsActive()
        {
            var currentCards = _cards.Where(p => p.ImageName != Global.EmptyImageName).ToList();

            for (int i = 0; i < currentCards.Count() - 1; i++)
            {
                for (int j = i + 1; j < currentCards.Count(); j++)
                {
                    if (Match(currentCards[i], currentCards[j], false))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 重新排列当前卡片集合
        /// 并保证有可消卡片
        /// </summary>
        public void Replace()
        {
            Random r = new Random();
            var currentCards = _cards.Where(p => p.ImageName != Global.EmptyImageName).ToList();
            var count = currentCards.Count;
            if (count == 0)
                return;

            var targetImageNames = currentCards.Select(p => p.ImageName).ToList();
            var targetPositions = currentCards.Select(p => p.Position).ToList();

            for (int i = 0; i < count; i++)
            {
                var index = r.Next(0, count - i);

                var targetImageName = targetImageNames.Skip(index).First();
                var targetPosition = targetPositions.Skip(i).First();

                _cards[targetPosition] = new CardModel(targetImageName, targetPosition);
                targetImageNames.RemoveAt(index);
            }

            while (!IsActive())
            {
                Replace();
            }
        }

        /// <summary>
        /// 清除卡片集合
        /// </summary>
        public void Clear()
        {
            _cards.Clear();
            Points.Clear();
        }

        /// <summary>
        /// 当前卡片数量
        /// </summary>
        public int CardCount
        {
            get { return _cards.Where(p => p.ImageName != Global.EmptyImageName).Count(); }
        }

        /// <summary>
        /// 连连看的卡片集合
        /// </summary>
        public ObservableCollection<CardModel> Cards
        {
            get { return _cards; }
        }

        private PointCollection _points;
        /// <summary>
        /// 可消两卡片的连接路径上的 4 个点的集合
        /// </summary>
        public PointCollection Points
        {
            get
            {
                if (_points == null)
                    _points = new PointCollection();

                return _points;
            }
            set
            {
                _points = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Points"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

