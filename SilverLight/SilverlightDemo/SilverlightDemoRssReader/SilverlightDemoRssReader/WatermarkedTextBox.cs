﻿///////////////////////////////////////////////////////////////////////////////
//
//  WatermarkedTextBox.xaml.cs
//
// 
// © 2008 Microsoft Corporation. All Rights Reserved.
//
// This file is licensed as part of the Silverlight 2 SDK, for details look here: 
// http://go.microsoft.com/fwlink/?LinkID=111970&clcid=0x409
//
///////////////////////////////////////////////////////////////////////////////

using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace SilverlightDemoRssReader
{
    

    /// <summary>
    /// WatermarkedTextBox is a specialized form of TextBox which displays custom visuals when its contents are empty
    /// </summary>
    [TemplatePart(Name = WatermarkedTextBox.ElementContentName, Type = typeof(ContentControl))]
    [TemplateVisualState(Name = VisualStateHelper.StateNormal, GroupName = VisualStateHelper.GroupCommon)]
    [TemplateVisualState(Name = VisualStateHelper.StateMouseOver, GroupName = VisualStateHelper.GroupCommon)]
    [TemplateVisualState(Name = VisualStateHelper.StateDisabled, GroupName = VisualStateHelper.GroupCommon)]
    [TemplateVisualState(Name = VisualStateHelper.StateUnfocused, GroupName = VisualStateHelper.GroupFocus)]
    [TemplateVisualState(Name = VisualStateHelper.StateFocused, GroupName = VisualStateHelper.GroupFocus)]
    [TemplateVisualState(Name = VisualStateHelper.StateUnwatermarked, GroupName = VisualStateHelper.GroupWatermark)]
    [TemplateVisualState(Name = VisualStateHelper.StateWatermarked, GroupName = VisualStateHelper.GroupWatermark)]
    public partial class WatermarkedTextBox : TextBox
    {
        #region Constants
        private const string ElementContentName = "Watermark";
        private const string TemplateXamlPath = "SilverlightDemoRssReader.WatermarkedTextBox.xaml";


        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkedTextBox"/> class.
        /// </summary>
        public WatermarkedTextBox()
        {
            SetStyle();
            SetDefaults();

            this.MouseEnter += OnMouseEnter;
            this.MouseLeave += OnMouseLeave;
            this.Loaded += OnLoaded;
            this.LostFocus += OnLostFocus;
            this.GotFocus += OnGotFocus;
            this.TextChanged += OnTextChanged;
        }
        #endregion

        #region Internal

        internal ContentControl elementContent;
        internal bool isHovered;
        internal bool hasFocus;

        //this method is made 'internal virtual' so the a TestWatermarkedTextBox with custom verification code
        //that executes in OnLoaded could be created
        internal virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
                ApplyTemplate();
                ChangeVisualState(false);
        }

        /// <summary>
        /// Change to the correct visual state for the textbox.
        /// </summary>
        internal void ChangeVisualState()
        {
            ChangeVisualState(true);
        }

        /// <summary>
        /// Change to the correct visual state for the textbox.
        /// </summary>
        /// <param name="useTransitions">
        /// true to use transitions when updating the visual state, false to
        /// snap directly to the new visual state.
        /// </param>
        internal void ChangeVisualState(bool useTransitions)
        {
            // Update the CommonStates group
            if (!IsEnabled)
            {
                VisualStateHelper.GoToState(this, useTransitions, VisualStateHelper.StateDisabled, VisualStateHelper.StateNormal);
            }
            else if (isHovered)
            {
                VisualStateHelper.GoToState(this, useTransitions, VisualStateHelper.StateMouseOver, VisualStateHelper.StateNormal);
            }
            else
            {
                VisualStateHelper.GoToState(this, useTransitions, VisualStateHelper.StateNormal);
            }

            // Update the FocusStates group
            if (hasFocus && IsEnabled)
            {
                VisualStateHelper.GoToState(this, useTransitions, VisualStateHelper.StateFocused, VisualStateHelper.StateUnfocused);
            }
            else
            {
                VisualStateHelper.GoToState(this, useTransitions, VisualStateHelper.StateUnfocused);
            }

            // Update the WatermarkStates group
            if (!hasFocus && this.Watermark != null && string.IsNullOrEmpty(this.Text))
            {
                VisualStateHelper.GoToState(this, useTransitions, VisualStateHelper.StateWatermarked, VisualStateHelper.StateUnwatermarked);
            }
            else
            {
                VisualStateHelper.GoToState(this, useTransitions, VisualStateHelper.StateUnwatermarked);
            }
        }
        #endregion

        #region Protected

        /// <summary>
        /// Called when template is applied to the control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            elementContent = ExtractTemplatePart<ContentControl>(ElementContentName);

            OnWatermarkChanged();

            ChangeVisualState(false);
        }

        #endregion

        #region Public

        #region IsEnabled


        /// <summary>
        /// IsEnabled dependency property
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled", typeof(bool), typeof(WatermarkedTextBox), new PropertyMetadata(OnIsEnabledPropertyChanged));


        /// <summary>
        /// Gets or sets a value indicating whether this element is enabled in the user interface (UI).  This is a dependency property.
        /// </summary>
        /// <value></value>
        /// <returns>true if the element is enabled; otherwise, false. The default value is true.</returns>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        #endregion

        #region Watermark
        /// <summary>
        /// Watermark dependency property
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(object), typeof(WatermarkedTextBox), new PropertyMetadata(OnWatermarkPropertyChanged));

        /// <summary>
        /// Watermark content
        /// </summary>
        /// <value>The watermark.</value>
        public object Watermark
        {
            get { return (object)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        #endregion

        #endregion

        #region Private

        private static string styleXaml;


        private T ExtractTemplatePart<T>(string partName) where T : DependencyObject
        {
            DependencyObject obj = GetTemplateChild(partName);
            return ExtractTemplatePart<T>(partName, obj);
        }


        private static T ExtractTemplatePart<T>(string partName, DependencyObject obj) where T : DependencyObject
        {
            Debug.Assert(obj == null || typeof(T).IsInstanceOfType(obj), 
             string.Format(CultureInfo.InvariantCulture, Resource.WatermarkedTextBox_TemplatePartIsOfIncorrectType, partName, typeof(T).Name));
            return obj as T;
        }



        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                hasFocus = true;

                if (!string.IsNullOrEmpty(this.Text))
                {
                    Select(0, this.Text.Length);
                }

                ChangeVisualState();
            }
        }


        /// <summary>
        /// Called when is enabled property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WatermarkedTextBox watermarkedTextBox = sender as WatermarkedTextBox;
            Debug.Assert(watermarkedTextBox != null, "The source is not an instance of a WatermarkedTextBox!");
            bool newValue = (bool)args.NewValue;

            //MIX-only solution, as IsEnabled is not defined on Control level
            watermarkedTextBox.IsHitTestVisible = newValue;
            watermarkedTextBox.IsTabStop = newValue;
            watermarkedTextBox.IsReadOnly = !newValue;

            watermarkedTextBox.ChangeVisualState();
        }


        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            hasFocus = false;
            ChangeVisualState();
        }


        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            isHovered = true;

            if (!hasFocus)
            {
                ChangeVisualState();
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            isHovered = false;

            ChangeVisualState();
        }


        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisualState();
        }


        private void OnWatermarkChanged()
        {
            if (elementContent != null)
            {
                Control watermarkControl = this.Watermark as Control;
                if (watermarkControl != null)
                {
                    watermarkControl.IsTabStop = false;
                    watermarkControl.IsHitTestVisible = false;
                }
            }
        }

        /// <summary>
        /// Called when watermark property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WatermarkedTextBox watermarkTextBox = sender as WatermarkedTextBox;
            Debug.Assert(watermarkTextBox != null, "The source is not an instance of a WatermarkedTextBox!");
            watermarkTextBox.OnWatermarkChanged();
            watermarkTextBox.ChangeVisualState();

        }

        private void SetDefaults()
        {
            IsEnabled = true;
            this.Watermark = Resource.WatermarkedTextBox_DefaultWatermarkText;
        }

        private void SetStyle()
        {
            if (styleXaml == null)
            {
                Stream stream = typeof(WatermarkedTextBox).Assembly.GetManifestResourceStream(TemplateXamlPath);
                Debug.Assert(stream != null, string.Format(CultureInfo.InvariantCulture, "XAML resource '{0}' is not found!", TemplateXamlPath));
                using (StreamReader reader = new StreamReader(stream))
                {
                    styleXaml = reader.ReadToEnd();
                }
            }
            Style style = XamlReader.Load(styleXaml) as Style;
            this.Style = style;
        }

        #endregion
    }
}

