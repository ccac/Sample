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
using System.Windows.Markup;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using System.Threading;


namespace MessageBox
{
    /// <summary>
    /// 硬编码模式的MessageBox
    /// </summary>
    public class HardCodeMessageBox
    {
        private static UIElement realVisual;
        private static Grid parentGrid;

        public static void Show(string text)
        {
            UserControl uc = Application.Current.RootVisual as UserControl;

            if (uc != null)
            {
                realVisual = UserControlContentAccessor.GetContent(uc);
                realVisual.IsHitTestVisible = false;

                parentGrid = new Grid();

                UserControlContentAccessor.SetContent(uc, parentGrid);
                parentGrid.Children.Add(realVisual);

                FrameworkElement dialogElement = LoadDialogResourceXaml(text);

                parentGrid.Children.Add(dialogElement);
            }
        }
        private static FrameworkElement LoadDialogResourceXaml(string text)
        {
            FrameworkElement element = null;

            using (Stream stream =
              Assembly.GetExecutingAssembly().GetManifestResourceStream("MessageBox.dialog.xaml"))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    element = XamlReader.Load(streamReader.ReadToEnd()) as FrameworkElement;
                    streamReader.Close();
                }
                stream.Close();
            }
            if (element != null)
            {
                Button okButton = element.FindName("buttonOk") as Button;
                Button cancelButton = element.FindName("buttonCancel") as Button;
                TextBlock messageContent = element.FindName("messageContent") as TextBlock;
                if (okButton != null)
                {
                    okButton.Click += DismissDialog;
                }
                if (cancelButton != null)
                {
                    cancelButton.Click += DismissDialog;
                }
                if (messageContent != null)
                {
                    messageContent.Text = text;
                }
                
            }
            return (element);
        }
        static void DismissDialog(object sender, EventArgs args)
        {
            UserControl uc = Application.Current.RootVisual as UserControl;

            if (uc != null)
            {
                parentGrid.Children.Clear();
                realVisual.IsHitTestVisible = true;
                UserControlContentAccessor.SetContent(uc, realVisual);
            }
        }

    }
}
