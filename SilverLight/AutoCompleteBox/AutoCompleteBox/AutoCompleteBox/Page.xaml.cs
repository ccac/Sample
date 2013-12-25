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

using Microsoft.Windows.Controls;
using System.Json;
using System.Windows.Browser;
using AutoCompleteBoxSample.WCFAutoCompletedService;

namespace AutoCompleteBoxSample
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NowAutoComplete.Populating += OnPopulatingSynchronous;


            WCFAutoComplete.IsTextCompletionEnabled = false;

            // Server does the filtering   
            WCFAutoComplete.SearchMode = AutoCompleteSearchMode.None;
            WCFAutoComplete.Populating += (s, args) =>
            {
                args.Cancel = true;
                AutoCompletedServiceClient acsc = new AutoCompletedServiceClient();
                acsc.GetEmployeeCollectionCompleted += new EventHandler<GetEmployeeCollectionCompletedEventArgs>(acsc_GetEmployeeCollectionCompleted);
                acsc.GetEmployeeCollectionAsync(args.Parameter, s);
            };
        }


        void acsc_GetEmployeeCollectionCompleted(object sender, GetEmployeeCollectionCompletedEventArgs e)
        {
            AutoCompleteBox acb = e.UserState as AutoCompleteBox;
            if (acb != null && e.Error == null && !e.Cancelled)
            {
                            
                if (e.Result.Count > 0)
                {
                    List<string> employeeStrList = new List<string>();
                    foreach(EmployeeInfo employeeinfo in e.Result)
                    {
                        employeeStrList.Add(employeeinfo.FirstName + " " + employeeinfo.LastName);
                    }
                    acb.ItemsSource = employeeStrList;
                    acb.PopulateComplete();
                }
            }
        }

       
             /// <summary>
        /// The Populating event handler.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnPopulatingSynchronous(object sender, PopulatingEventArgs e)
        {
            AutoCompleteBox source = (AutoCompleteBox)sender;

            source.ItemsSource = new string[]
            {
                e.Parameter + "后续内容1",
                e.Parameter + "后续内容2",
                e.Parameter + "后续内容3",
            };           
        }

  

    }
}
