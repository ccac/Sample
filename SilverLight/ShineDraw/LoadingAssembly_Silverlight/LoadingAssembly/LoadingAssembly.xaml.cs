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
using System.Reflection;

/*
*	Loading External Assembly/Library Dynamically Demonstratoin in C#
*   from shinedraw.com
*/

namespace LoadingAssembly
{
    public partial class LoadingAssembly : UserControl
    {
        private const string ASSEMBLY = "SimpleGameSystem.dll";                 // ASSEMBLY PATH
        private const string CLASS_NAME = "SimpleGameSystem.SimpleGameSystem";  // ASSEMBLY CLASS, This file is located in the folder LoadingAssembly.Web
        private Assembly _assembly;

        public LoadingAssembly()
        {
            InitializeComponent();
            Cover.MouseLeftButtonDown += new MouseButtonEventHandler(Cover_DownloadAssembly);
        }



        /////////////////////////////////////////////////////        
        // Handlers 
        /////////////////////////////////////////////////////

        // Click to download assembly
        void Cover_DownloadAssembly(object sender, MouseButtonEventArgs e)
        {
            Cover.MouseLeftButtonDown -= new MouseButtonEventHandler(Cover_DownloadAssembly);
            downloadAssembly();
        }

        // Click to initialize an instance
        void Cover_CreateInstance(object sender, MouseButtonEventArgs e)
        {
            Cover.MouseLeftButtonDown -= new MouseButtonEventHandler(Cover_CreateInstance);
            createInstance();
        }

        // Once the assembly is downloaded
        private void onDownloadCompleted(object o, OpenReadCompletedEventArgs args)
        {
            try
            {
                AssemblyPart ap = new AssemblyPart();
                _assembly = ap.Load(args.Result);
                Cover.MouseLeftButtonDown += new MouseButtonEventHandler(Cover_CreateInstance);
                Label.Text = "Assembly downloaded.\rClick to create an instance.";
            }
            catch (Exception e)
            {
                Label.Text = "Download Error: " + e.Message;
            }
        }

        /////////////////////////////////////////////////////        
        // Private Method 
        /////////////////////////////////////////////////////

        // Download Assembly using WebClient
        private void downloadAssembly()
        {
            WebClient downloader = new WebClient();
            downloader.OpenReadCompleted += new OpenReadCompletedEventHandler(onDownloadCompleted);
            downloader.OpenReadAsync(new Uri(GetAbsoluteUrl(ASSEMBLY), UriKind.Absolute));
            Label.Text = "Downloading assembly: \r" + GetAbsoluteUrl(ASSEMBLY);
        }

        // Create an instance from the assembly
        private void createInstance()
        {
            UserControl control = (UserControl) _assembly.CreateInstance(CLASS_NAME);
            LayoutRoot.Children.Add(control);
        }

        /////////////////////////////////////////////////////        
        // Static Method 
        /////////////////////////////////////////////////////

        // get the url path
        public static string GetAbsoluteUrl(string strRelativePath)
        {

            string strFullUrl;
            if (string.IsNullOrEmpty(strRelativePath))
                return strRelativePath;

            if (strRelativePath.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
              || strRelativePath.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
              || strRelativePath.StartsWith("file:", StringComparison.OrdinalIgnoreCase)
              )
            {
                //already absolute
                strFullUrl = strRelativePath;
            }
            else
            {
                strFullUrl = System.Windows.Application.Current.Host.Source.AbsoluteUri;
                //relative, need to convert to absolute
                if (strFullUrl.IndexOf("ClientBin") > 0)
                    strFullUrl = strFullUrl.Substring(0, strFullUrl.IndexOf("ClientBin")) + strRelativePath;
                else if(strFullUrl.LastIndexOf("/") > 0)
                {
                    strFullUrl = strFullUrl.Substring(0, strFullUrl.LastIndexOf("/") + 1) + strRelativePath;
                }
            }

            return strFullUrl;
        }
    }
}
