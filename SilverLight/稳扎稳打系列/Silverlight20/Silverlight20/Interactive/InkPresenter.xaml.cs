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
using System.Windows.Ink;
using System.Xml.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Silverlight20.Interactive
{
    public partial class InkPresenter : UserControl
    {
        // 在涂鸦板上描绘的笔划
        private System.Windows.Ink.Stroke _newStroke;

        // 在涂鸦板上描绘的笔划的颜色
        private System.Windows.Media.Color _currentColor = Colors.Red;

        // 是否是擦除操作
        private bool _isEraser = false;

        // 当前是否正在 InkPresenter 上捕获鼠标
        private bool _isCapture = false;

        public InkPresenter()
        {
            InitializeComponent();
        }

        void inkPresenter_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            // UIElement.CaptureMouse() - 为 UIElement 对象启用鼠标捕捉

            // 为 InkPresenter 启用鼠标捕捉
            inkPresenter.CaptureMouse();
            _isCapture = true;

            if (_isEraser)
            {
                // 擦除鼠标当前位置所属的 Stroke（笔划）
                RemoveStroke(e);
            }
            else
            {
                // System.Windows.Input.MouseEventArgs.StylusDevice.Inverted - 是否正在使用手写笔（tablet pen）的辅助笔尖

                // System.Windows.Ink.Stroke.DrawingAttributes - Stroke（笔划）的外观属性
                //     System.Windows.Ink.Stroke.DrawingAttributes.Width - 笔划的宽
                //     System.Windows.Ink.Stroke.DrawingAttributes.Height - 笔划的高
                //     System.Windows.Ink.Stroke.DrawingAttributes.ColorEnum - 笔划的颜色
                //     System.Windows.Ink.Stroke.DrawingAttributes.OutlineColor - 笔划的外框的颜色

                _newStroke = new System.Windows.Ink.Stroke();
                _newStroke.DrawingAttributes.Width = 3d;
                _newStroke.DrawingAttributes.Height = 3d;
                _newStroke.DrawingAttributes.Color = _currentColor;
                _newStroke.DrawingAttributes.OutlineColor = Colors.Yellow;

                // 为 Stroke（笔划） 在当前鼠标所在位置处增加 StylusPoint（点）
                _newStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkPresenter));
                // 将设置好的 Stroke（笔划） 添加到 InkPresenter 的 Strokes（笔划集） 中
                inkPresenter.Strokes.Add(_newStroke);

                // Stroke.GetBounds() - 获取当前 Stroke（笔划） 所在的 矩形范围 的位置信息
                // Strokes.GetBounds() - 获取当前 Strokes（笔划集） 所在的 矩形范围 的位置信息

                // 显示该 Stroke（笔划） 所在的 矩形范围 的位置信息
                Rect rect = _newStroke.GetBounds();
                txtMsg.Text = string.Format("上:{0}; 下:{1}; 左:{2}; 右:{3}",
                   rect.Top, rect.Bottom, rect.Left, rect.Right);
            }
        }

        void inkPresenter_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isCapture)
            {
                if (_isEraser)
                {
                    // 擦除鼠标当前位置所属的 Stroke
                    RemoveStroke(e);
                }
                else if (_newStroke != null)
                {
                    // 为已经添加到 InkPresenter 的 Strokes 中的 Stroke 增加 StylusPoint
                    _newStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkPresenter));

                    // 显示该 Stroke 所在的 矩形范围 的位置信息
                    Rect rect = _newStroke.GetBounds();
                    txtMsg.Text = string.Format("上:{0}; 下:{1}; 左:{2}; 右:{3}",
                           rect.Top, rect.Bottom, rect.Left, rect.Right);
                }
            }
        }

        void inkPresenter_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            // UIElement.CaptureMouse() - 为 UIElement 对象释放鼠标捕捉

            // 为 InkPresenter 释放鼠标捕捉
            inkPresenter.ReleaseMouseCapture();
            _newStroke = null;
            _isCapture = false;
        }

        void RemoveStroke(MouseEventArgs e)
        {
            // Stroke.HitTest(StylusPointCollection) -  Stroke 是否与指定的 StylusPoint 集合相连
            // Strokes.HitTest(StylusPointCollection) - 与指定的 StylusPoint 集合相连的 Stroke 集合

            // 获取当前鼠标所在位置处的 StylusPoint 集合
            StylusPointCollection erasePoints = new StylusPointCollection();
            erasePoints.Add(e.StylusDevice.GetStylusPoints(inkPresenter));

            // 与当前鼠标所在位置处的 StylusPoint 集合相连的 Stroke 集合
            StrokeCollection hitStrokes = inkPresenter.Strokes.HitTest(erasePoints);

            for (int i = 0; i < hitStrokes.Count; i++)
            {
                // 在 InkPresenter 上清除指定的 Stroke
                inkPresenter.Strokes.Remove(hitStrokes[i]);
            }
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            // 视频播放完后，再重新播放
            mediaElement.Position = TimeSpan.FromMilliseconds(0);
            mediaElement.Play();
        }

        private void ellipseRed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 单击了 红色取色点
            _currentColor = Colors.Red;
            inkPresenter.Cursor = Cursors.Stylus;
            _isEraser = false;
        }

        private void ellipseBlack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 单击了 黑色取色点
            _currentColor = Colors.Black;
            inkPresenter.Cursor = Cursors.Stylus;
            _isEraser = false;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            // 单击了 清除 按钮
            inkPresenter.Strokes.Clear();
        }

        private void btnEraser_Click(object sender, RoutedEventArgs e)
        {
            // 单击了 橡皮擦 按钮
            inkPresenter.Cursor = Cursors.Eraser;
            _isEraser = true;
        }
    }
}
