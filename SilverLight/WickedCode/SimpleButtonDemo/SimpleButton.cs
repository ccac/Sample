using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SimpleButtonDemo
{
    [TemplatePart(Name = SimpleButton.RootElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = SimpleButton.BodyElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = SimpleButton.HighlightElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = SimpleButton.MouseEnterAnimation, Type = typeof(Storyboard))]
    [TemplatePart(Name = SimpleButton.MouseLeaveAnimation, Type = typeof(Storyboard))]
    public class SimpleButton : ContentControl
    {
        private const string RootElement = "RootElement";
        private const string BodyElement = "BodyElement";
        private const string HighlightElement = "HighlightElement";
        private const string MouseEnterAnimation = "MouseEnterAnimation";
        private const string MouseLeaveAnimation = "MouseLeaveAnimation";

        private FrameworkElement _root;
        private FrameworkElement _body;
        private FrameworkElement _highlight;
        private Storyboard _enter;
        private Storyboard _leave;

        public event RoutedEventHandler Click;

        public SimpleButton()
        {
            this.MouseLeftButtonUp += new MouseButtonEventHandler(SimpleButton_MouseLeftButtonUp);
            this.MouseEnter += new MouseEventHandler(SimpleButton_MouseEnter);
            this.MouseLeave += new MouseEventHandler(SimpleButton_MouseLeave);
        }

        void SimpleButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
                Click(this, new RoutedEventArgs());
        }

        void SimpleButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_enter != null && _highlight != null)
                _enter.Begin();
        }

        void SimpleButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_leave != null && _highlight != null)
                _leave.Begin();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _root = (FrameworkElement)GetTemplateChild(SimpleButton.RootElement);
            _body = (FrameworkElement)GetTemplateChild(SimpleButton.BodyElement);
            _highlight = (FrameworkElement)GetTemplateChild(SimpleButton.HighlightElement);

            if (_root != null)
            {
                _enter = (Storyboard)_root.Resources[SimpleButton.MouseEnterAnimation];
                _leave = (Storyboard)_root.Resources[SimpleButton.MouseLeaveAnimation];
            }
        }
    }
}
