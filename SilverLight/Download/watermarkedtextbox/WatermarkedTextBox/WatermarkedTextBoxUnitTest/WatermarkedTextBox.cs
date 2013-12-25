///////////////////////////////////////////////////////////////////////////////
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

namespace System.Windows.Controls.Extended.Test
{
    using System.Globalization;
    using System.Windows.Markup;
    using System.Collections.Generic;
    using System.Windows.Media;
    using System.Windows.Controls.Test;
    using System.Diagnostics;
    using Microsoft.Silverlight.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WatermarkedTextBoxTest : SilverlightControlTest
    {
        private const string SomeText = "some text";
        private const string SomeMoreText = "some more text";
        private const double DefaultFontSize = 11.00;
        private const string DefaultFontFamily = "Trebuchet MS";
        private static readonly Thickness DefaultThickness = new Thickness(1, 1, 1, 1);
        private static readonly Color DefaultBorderColor = Color.FromArgb(0xFF, 0, 0, 0);
        private static readonly string DefaultWatermark = System.Windows.Controls.Resource.WatermarkedTextBox_DefaultWatermarkText;

        #region Synchronous

        [TestMethod]
        [Owner("albulank")]
        [Description("Ensures that WatermarkedTextBox can be created and verifies property combinations.")]
        public void CreateFromXamlTest()
        {
            CreateFromXaml(SomeText, false, SomeText);
            CreateFromXaml(string.Empty, true, null);
        }

        [TestMethod]
        [Owner("albulank")]
        [Description("Ensures that Watermark property can be set to a UIElement.")]
        public void SetWatermarkToUIElement()
        {
            WatermarkedTextBox watermarkedTextBox = CreateFromCode();
            TextBlock textBlock = new TextBlock();
            watermarkedTextBox.Watermark = textBlock;
            Assert.AreEqual(textBlock, watermarkedTextBox.Watermark, "Watermark property should be set to TextBlock.");

        }

        [TestMethod]
        [Owner("albulank")]
        [Description("Ensures that Watermark property can be set a string.")]
        public void SetWatermarkToText()
        {
            WatermarkedTextBox watermarkedTextBox = CreateFromCode();
            watermarkedTextBox.Watermark = SomeText;
            Assert.AreEqual<string>(SomeText, watermarkedTextBox.Watermark as string,
                string.Format(CultureInfo.InvariantCulture, "Watermark property should be set to '{0}'", SomeText));
        }

        [TestMethod]
        [Owner("albulank")]
        [Description("Ensures that Watermark property can be set to an object.")]
        public void SetWatermarkToObject()
        {
            WatermarkedTextBox watermarkedTextBox = CreateFromCode();
            object o = new object();
            watermarkedTextBox.Watermark = o;
            Assert.AreEqual(o, watermarkedTextBox.Watermark, string.Format(CultureInfo.InvariantCulture,
                "Watermark property should be set to the specified object."));
        }

        [TestMethod]
        [Owner("albulank")]
        [Description("Ensures that IsEnabled property can be set.")]
        public void SetIsEnabled()
        {
            WatermarkedTextBox watermarkedTextBox = CreateFromCode();
            Assert.IsTrue(watermarkedTextBox.IsEnabled, "Control should be enabled.");
            watermarkedTextBox.IsEnabled = false;
            Assert.IsFalse(watermarkedTextBox.IsEnabled, "Control should be disabled.");
            watermarkedTextBox.IsEnabled = true;
            Assert.IsTrue(watermarkedTextBox.IsEnabled, "Control should be enabled.");
        }

        [TestMethod]
        [Owner("albulank")]
        [Description("Ensures that IsEnabled property changes IsHitTestVisible, IsReadOnly and IsTabStop. " +
        "Also ensures that is developer has specified these values manually, they are restored when the control is re-enabled")]
        public void VerifyIsEnabledLogic()
        {
            WatermarkedTextBox watermarkedTextBox = CreateFromCode();
            Assert.IsTrue(watermarkedTextBox.IsEnabled, "Control should be enabled.");
            Assert.IsTrue(watermarkedTextBox.IsHitTestVisible, "Control should have IsHitTestVisible=True");
            Assert.IsFalse(watermarkedTextBox.IsReadOnly, "Control should not be read-only");
            Assert.IsTrue(watermarkedTextBox.IsTabStop, "Control should have IsTabStop=True");

            watermarkedTextBox.IsEnabled = false;
            Assert.IsFalse(watermarkedTextBox.IsEnabled, "Control should be disabled.");
            Assert.IsFalse(watermarkedTextBox.IsHitTestVisible, "Control should not have IsHitTestVisible=True");
            Assert.IsTrue(watermarkedTextBox.IsReadOnly, "Control should  be read-only");
            Assert.IsFalse(watermarkedTextBox.IsTabStop, "Control should not have IsTabStop=True");

            watermarkedTextBox.IsEnabled = true;
            Assert.IsTrue(watermarkedTextBox.IsEnabled, "Control should be enabled.");
            Assert.IsTrue(watermarkedTextBox.IsHitTestVisible, "Control should have IsHitTestVisible=True");
            Assert.IsFalse(watermarkedTextBox.IsReadOnly, "Control should not be read-only");
            Assert.IsTrue(watermarkedTextBox.IsTabStop, "Control should have IsTabStop=True");

            watermarkedTextBox.IsHitTestVisible = false;
            watermarkedTextBox.IsReadOnly = true;

            watermarkedTextBox.IsEnabled = false;
            Assert.IsFalse(watermarkedTextBox.IsEnabled, "Control should be disabled.");
            Assert.IsFalse(watermarkedTextBox.IsHitTestVisible, "Control should not have IsHitTestVisible=True");
            Assert.IsTrue(watermarkedTextBox.IsReadOnly, "Control should  be read-only");
            Assert.IsFalse(watermarkedTextBox.IsTabStop, "Control should not have IsTabStop=True");

            watermarkedTextBox.IsEnabled = true;
            Assert.IsTrue(watermarkedTextBox.IsEnabled, "Control should be enabled.");
            Assert.IsTrue(watermarkedTextBox.IsHitTestVisible, "Control should have IsHitTestVisible=True");
            Assert.IsFalse(watermarkedTextBox.IsReadOnly, "Control should not be read-only");
            Assert.IsTrue(watermarkedTextBox.IsTabStop, "Control should have IsTabStop=True");
        }


        #endregion

        #region Asynchronous

        [TestMethod]
        [Owner("albulank")]
        [Description("Verifies that control is in 'stateDisabled' state when Watermark is null and IsEnabled = false.")]
        [Asynchronous]
        public void VerifyWatermarkStateDisabled()
        {
            WTBReference reference = new WTBReference(this);
            reference.LoadedActions.Add(new Action(delegate
             {
                 reference.WatermarkedTextBox.IsEnabled = false;
                 Assert.IsFalse(reference.WatermarkedTextBox.IsEnabled);
             }));
        }


        [TestMethod]
        [Owner("albulank")]
        [Description("Verifies that default template is correctly applied and all template parts are initialized.")]
        [Asynchronous]
        public void VerifyDefaultTemplateAppliedCorrectly()
        {
            WTBReference reference = new WTBReference(this);
            reference.LoadedActions.Add(new Action(delegate
             {
                 Assert.IsNotNull(reference.WatermarkedTextBox.elementContent);
             }));
        }


        [TestMethod]
        [Owner("albulank")]
        [Description("Verifies that custom template is loaded, the provided TemplateParts are initialized and the control defaults to 'stateNormal' state.")]
        [Asynchronous]
        public void VerifyCustomLimitedTemplate()
        {
            string template = Resource.WTB_LimitedTemplate;
            WTBReference reference = new WTBReference(this, template);
            reference.LoadedActions.Add(new Action(delegate
       {

           //only the following members should be initialized by the limited template
           Assert.IsNull(reference.WatermarkedTextBox.elementContent);
           reference.WatermarkedTextBox.Watermark = SomeText;
           reference.WatermarkedTextBox.IsEnabled = false;
       }));

        }
        #endregion


        #region Helper code


        private static WatermarkedTextBox CreateFromCode()
        {
            WatermarkedTextBox watermarkedTextBox = new WatermarkedTextBox();
            VerifyWatermarkedTextBoxCreated(watermarkedTextBox);
            VerifyDefaultStyledPropertiesApplied(watermarkedTextBox);

            return watermarkedTextBox;
        }

        private static WatermarkedTextBox CreateFromXaml()
        {
            string xaml = string.Format(CultureInfo.InvariantCulture, Resource.WTB_DefaultXaml);
            WatermarkedTextBox watermarkedTextBox = (WatermarkedTextBox)XamlReader.Load(xaml);
            VerifyWatermarkedTextBoxCreated(watermarkedTextBox);
            VerifyDefaultStyledPropertiesApplied(watermarkedTextBox);
            return watermarkedTextBox;
        }

        private static WatermarkedTextBox CreateFromXaml(string text, bool isEnabled, string watermark)
        {
            string xaml = string.Format(CultureInfo.InvariantCulture, Resource.WTB_CustomXaml, text, isEnabled, watermark ?? SomeMoreText);
            WatermarkedTextBox watermarkedTextBox = (WatermarkedTextBox)XamlReader.Load(xaml);

            Assert.IsNotNull(watermarkedTextBox, "WatermarkedTextBox should be created.");
            VerifyDefaultStyledPropertiesApplied(watermarkedTextBox);


            Assert.AreEqual<string>(text, watermarkedTextBox.GetValue(WatermarkedTextBox.TextProperty) as string, "DP Text property should be set.");
            Assert.AreEqual<string>(text, watermarkedTextBox.Text, "Text property should be set.");

            Assert.AreEqual<bool>(isEnabled, (bool)watermarkedTextBox.GetValue(WatermarkedTextBox.IsEnabledProperty), "DP IsEnabled property should be set");
            Assert.AreEqual<bool>(isEnabled, watermarkedTextBox.IsEnabled, "IsEnabled property should be set");

            if (watermark != null)
            {
                Assert.AreEqual<string>(watermark, watermarkedTextBox.GetValue(WatermarkedTextBox.WatermarkProperty) as string, "DP Watermark property should be set.");
                Assert.AreEqual<string>(watermark, watermarkedTextBox.Watermark as string, "Watermark property should be set.");
            }
            else
            {
                Assert.AreEqual<string>(SomeMoreText, watermarkedTextBox.GetValue(WatermarkedTextBox.WatermarkProperty) as string, "DP Watermark property should be set.");
                Assert.AreEqual<string>(SomeMoreText, watermarkedTextBox.Watermark as string, "Watermark property should be set.");
            }

            return watermarkedTextBox;
        }


        private static void VerifyDefaultStyledPropertiesApplied(WatermarkedTextBox watermarkedTextBox)
        {
            //Assert.AreEqual<Type>(typeof(SolidColorBrush), watermarkedTextBox.BorderBrush.GetType());
            //Assert.AreEqual<Color>(DefaultBorderColor, ((SolidColorBrush)watermarkedTextBox.BorderBrush).Color);
            Assert.AreEqual<Thickness>(DefaultThickness, watermarkedTextBox.BorderThickness);
            Assert.AreEqual<double>(DefaultFontSize, watermarkedTextBox.FontSize);
            Assert.AreEqual<string>(DefaultFontFamily, watermarkedTextBox.FontFamily.Source);
        }

        private static void VerifyWatermarkedTextBoxCreated(WatermarkedTextBox watermarkedTextBox)
        {
            Assert.IsNotNull(watermarkedTextBox, "WatermarkedTextBox should be created.");
            Assert.IsTrue(watermarkedTextBox.IsEnabled, "Default value for WatermarkedTextBox.IsEnabled should be True.");
            Assert.AreEqual<string>(DefaultWatermark, watermarkedTextBox.Watermark as string,
                string.Format(CultureInfo.InvariantCulture, "By default Watermark property should be set to '{0}'", DefaultWatermark));

            watermarkedTextBox.Watermark = null;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
            Justification = "IDisposable pattern is not applicable in this case.")]
        private class WTBReference
        {
            public List<Action> LoadedActions = new List<Action>();
            public WatermarkedTextBox WatermarkedTextBox;
            public LiveReference LiveReference;

            private WatermarkedTextBoxTest test;


            public WTBReference(WatermarkedTextBoxTest test) : this(test, null) { }

            public WTBReference(WatermarkedTextBoxTest test, string templateXaml)
            {
                this.test = test;

                if (templateXaml != null)
                {
                    this.WatermarkedTextBox = new TestWatermarkedTextBox(templateXaml);
                }
                else
                {
                    this.WatermarkedTextBox = WatermarkedTextBoxTest.CreateFromXaml();
                }

                WatermarkedTextBox.Loaded += new RoutedEventHandler(WatermarkedTextBox_Loaded);
                this.LiveReference = new LiveReference(test, WatermarkedTextBox);
            }

            void WatermarkedTextBox_Loaded(object sender, RoutedEventArgs e)
            {
                //ensure that template is applied if it wasn't applied previously
                WatermarkedTextBox.ApplyTemplate();

                try
                {
                    foreach (Action action in LoadedActions)
                    {
                        action();
                    }

                    //TestComplete won't be called if Exception is thrown during one of the actions
                    this.test.TestComplete();
                }
                catch (AssertFailedException ex)
                {
                    //This is needed because of the bug with Application.OnUnhandledException (see Jolt Bugs 12131)
                    //Asserts failed in OnLoad are not reported properly                
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
                finally
                {
                    this.LiveReference.Dispose();
                }
            }
        }

        private class TestWatermarkedTextBox : WatermarkedTextBox
        {
            public TestWatermarkedTextBox(string templateXaml)
            {
                this.Watermark = null;
                if (templateXaml != null)
                {
                    ControlTemplate template = (ControlTemplate)XamlReader.Load(templateXaml);
                    Assert.IsNotNull(template);
                    this.Template = template;
                    Assert.AreEqual(template, this.Template);
                }
            }
        }

        #endregion
    }
}
