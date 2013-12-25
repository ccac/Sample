using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace CustomControlWalkthroug
{
    
    [TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "TextElement", Type = typeof(TextBlock))]
    [TemplatePart(Name = "UpButtonElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "DownButtonElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "NegativeState", Type = typeof(Storyboard))]
    [TemplatePart(Name = "NormalState", Type = typeof(Storyboard))]
    public class NumericUpDown:Control
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown),new PropertyMetadata(new PropertyChangedCallback(ValueChangedCallback)));

        private static void ValueChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            //Add animation and event logic here
            NumericUpDown ctl = (NumericUpDown)obj;
            int oldValue = (int)args.OldValue;
            int newValue = (int)args.NewValue;

            //Update the TextElement to the new value
            if (ctl.TextElement != null)
            {
                ctl.TextElement.Text = newValue.ToString();
            }

            // Initiate the animation according to the value;
            ctl.DoValueAnimation(oldValue, newValue);

            // Raise the ValueChanged event.
            ValueChangedEventArgs e = new ValueChangedEventArgs(newValue);
            ctl.OnValueChanged(e);
        }


        // If Value changes from positive to negative,
        // begin the Negative State.
        // If value changes from negative to positive,
        // begin the Normal State.
        private void DoValueAnimation(int oldValue, int newValue)
        {
            if (oldValue < 0 && newValue >= 0)
            {
                GotoState(NormalState);
            }

            if (oldValue >= 0 && newValue < 0)
            {
                GotoState(NegativeState);
            }
        }

        private void GotoState(Storyboard state)
        {
            if (state != null)
            {
                //Begin new state storyboard
                state.Begin();
            }

            if (currentState != null)
            {
                //Stop old state storyboard.
                currentState.Stop();
            }
            currentState = state;
        }


        //This method searches for the elements in the ControlTemplate and 
        //assigns them to the appropriate property so that the control has a reference to the element.
        public override void OnApplyTemplate()
        {
            RootElement = (FrameworkElement)GetTemplateChild("RootElement");
            TextElement = (TextBlock)GetTemplateChild("TextElement");
            UpButtonElement = (RepeatButton)GetTemplateChild("UpButtonElement");
            DownButtonElement = (RepeatButton)GetTemplateChild("DownButtonElement");

            if (RootElement != null)
            {
                NormalState = (Storyboard)RootElement.Resources["NormalState"];
                NegativeState = (Storyboard)RootElement.Resources["NegativeState"];
            }
        }

        public event ValueChangedEventHandler ValueChanged;

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChangedEventHandler handler = ValueChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }


        private Storyboard negativeState;
        private Storyboard normalState;
        private Storyboard currentState;

        private Storyboard NegativeState
        {
            get { return negativeState; }
            set { negativeState = value; }
        }

        private Storyboard NormalState
        {
            get { return normalState; }
            set { normalState = value; }
        }


        private RepeatButton upButtonElement;
        private RepeatButton downButtonElement;
        private TextBlock textElement;
        private FrameworkElement rootElement;

        private RepeatButton UpButtonElement
        {
            get
            {
                return upButtonElement;
            }
            set
            {
                if (upButtonElement != null)
                {
                    upButtonElement.Click -= new RoutedEventHandler(upButtonElement_Click);
                }
                upButtonElement = value;
                if (upButtonElement != null)
                {
                    upButtonElement.Click += new RoutedEventHandler(upButtonElement_Click);
                }
            }
        }

        private RepeatButton DownButtonElement
        {
            get
            {
                return downButtonElement;
            }
            set
            {
                if (downButtonElement != null)
                {
                    downButtonElement.Click -= new RoutedEventHandler(downButtonElement_Click);
                }
                downButtonElement = value;
                if (downButtonElement != null)
                {
                    downButtonElement.Click += new RoutedEventHandler(downButtonElement_Click);
                }
            }
        }

        private TextBlock TextElement
        {
            get { return textElement; }
            set
            {
                textElement = value;

                if (textElement != null)
                {
                    textElement.Text = Value.ToString();
                }
            }
        }

        private FrameworkElement RootElement
        {
            get
            {
                return rootElement;
            }
            set
            {
                rootElement = value;
            }
        }

        void upButtonElement_Click(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        void downButtonElement_Click(object sender, RoutedEventArgs e)
        {
            Value--;
        }
 
    }

    public delegate void ValueChangedEventHandler(object sender,ValueChangedEventArgs e);

    public class ValueChangedEventArgs : EventArgs
    {
        private int _value;

        public ValueChangedEventArgs(int num)
        {
            _value = num;
        }

        public int Value
        {
            get { return _value; }
        }
    }
}
