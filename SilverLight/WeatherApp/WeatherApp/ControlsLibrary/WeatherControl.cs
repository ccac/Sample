using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;


namespace ControlsLibrary
{
    [TemplatePart(Name = "Core", Type = typeof(FrameworkElement))]

    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]

    [TemplateVisualState(Name = "Sunny", GroupName = "WeatherStates")]
    [TemplateVisualState(Name = "PartlyCloudy", GroupName = "WeatherStates")]
    [TemplateVisualState(Name = "Cloudy", GroupName = "WeatherStates")]
    [TemplateVisualState(Name = "Rainy", GroupName = "WeatherStates")]
    public class WeatherControl : Control
    {

        public WeatherControl()
        {
            //Set DefaultStyleKey
            DefaultStyleKey = typeof(WeatherControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CorePart = (FrameworkElement)GetTemplateChild("Core");

            GoToState(false);
        }

        private FrameworkElement CorePart
        {
            get
            {
                return corePart;
            }

            set
            {
                FrameworkElement oldCorePart = corePart;

                if (oldCorePart != null)
                {
                    oldCorePart.MouseEnter -= new MouseEventHandler(corePart_MouseEnter);
                    oldCorePart.MouseLeave -= new MouseEventHandler(corePart_MouseLeave);
                    oldCorePart.MouseLeftButtonDown -= new MouseButtonEventHandler(corePart_MouseLeftButtonDown);
                    oldCorePart.MouseLeftButtonUp -= new MouseButtonEventHandler(corePart_MouseLeftButtonUp);
                }

                corePart = value;

                if (corePart != null)
                {
                    corePart.MouseEnter += new MouseEventHandler(corePart_MouseEnter);
                    corePart.MouseLeave += new MouseEventHandler(corePart_MouseLeave);
                    corePart.MouseLeftButtonDown += new MouseButtonEventHandler(corePart_MouseLeftButtonDown);
                    corePart.MouseLeftButtonUp += new MouseButtonEventHandler(corePart_MouseLeftButtonUp);
                }
            }
        }



        private void GoToState(bool useTransitions)
        {

            //  Go to states in NormalStates state group
            if (isPressed)
            {
                VisualStateManager.GoToState(this, "Pressed", useTransitions);
            }
            else if (isMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }


            //  Go to states in WeatherStates state group
            if (Condition == Condition.PartlyCloudy)
            {
                VisualStateManager.GoToState(this, "PartlyCloudy", useTransitions);
            }
            else if (Condition == Condition.Sunny)
            {
                VisualStateManager.GoToState(this, "Sunny", useTransitions);
            }
            else if (Condition == Condition.Cloudy)
            {
                VisualStateManager.GoToState(this, "Cloudy", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Rainy", useTransitions);
            }
        }

        #region input event handlers

        void corePart_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOver = true;
            GoToState(true);
        }

        void corePart_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOver = false;
            GoToState(true);
        }

        void corePart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isPressed = true;
            GoToState(true);
        }

        void corePart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isPressed = false;
            GoToState(true);
        }

        #endregion


        #region public properties

        // Temperature DP

        public static readonly DependencyProperty TemperatureProperty = DependencyProperty.Register("Temperature", typeof(string), typeof(WeatherControl), new PropertyMetadata(new PropertyChangedCallback(WeatherControl.OnTemperaturePropertyChanged)));

        public string Temperature
        {
            get
            {
                return (string)GetValue(TemperatureProperty);
            }

            set
            {
                SetValue(TemperatureProperty, value);
            }
        }

        // Condition DP

        public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(Condition), typeof(WeatherControl), new PropertyMetadata(new PropertyChangedCallback(WeatherControl.OnConditionPropertyChanged)));

        public Condition Condition
        {
            get
            {
                return (Condition)GetValue(ConditionProperty);
            }

            set
            {
                SetValue(ConditionProperty, value);
            }
        }


        // ConditionDescription DP

        public static readonly DependencyProperty ConditionDescriptionProperty = DependencyProperty.Register("ConditionDescription", typeof(string), typeof(WeatherControl), new PropertyMetadata(new PropertyChangedCallback(WeatherControl.OnConditionDescriptionPropertyChanged)));

        public string ConditionDescription
        {
            get
            {
                return (string)GetValue(ConditionDescriptionProperty);
            }

            set
            {
                SetValue(ConditionDescriptionProperty, value);
            }
        }




        #endregion

        #region Change Notification Handlers


        private static void OnTemperaturePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WeatherControl weather = d as WeatherControl;
            var newValue = (string)e.NewValue;
            RoutedEventArgs args = new RoutedEventArgs();
            //args.Source = weather;
            weather.OnWeatherChange(null);
        }

        private static void OnConditionDescriptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WeatherControl weather = d as WeatherControl;
            var newValue = (string)e.NewValue;
            RoutedEventArgs args = new RoutedEventArgs();
            //args.Source = weather;
            weather.OnWeatherChange(null);
        }

        private static void OnConditionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WeatherControl weather = d as WeatherControl;
            var newValue = (Condition)e.NewValue;
            RoutedEventArgs args = new RoutedEventArgs();
            //args.Source = weather;
            weather.OnWeatherChange(null);
        }

        #endregion Change Notification Handlers
        #region protected methods

        protected  void OnWeatherChange(RoutedEventArgs e)
        {
            GoToState(true);
        }
        #endregion

        #region private

        private FrameworkElement corePart;

        private bool isMouseOver, isPressed;
        #endregion
    }
}
