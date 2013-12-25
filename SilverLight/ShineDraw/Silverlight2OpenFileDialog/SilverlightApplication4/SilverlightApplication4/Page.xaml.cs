using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Imaging;

namespace SilverlightApplication4
{
	public partial class Page : UserControl
	{
		public Page()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        public void Button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg | All Files (*.*)|*.*";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = ofd.SelectedFile.OpenRead();
                BitmapImage bi = new BitmapImage();

                bi.SetSource(stream);
                Image img = new Image();
                img.Source = bi;
                bd.Child = img;

                stream.Close();
            }

        }
	}
}