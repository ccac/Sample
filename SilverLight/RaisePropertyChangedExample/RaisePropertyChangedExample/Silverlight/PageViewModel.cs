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
using System.ComponentModel;

namespace RaisePropertyChangedExample
{
    #region PageViewModel
        public class PageViewModel : ObservableBase
        {
            #region Constructor
            public PageViewModel()
            {
                Id = 1;
                //Name = "Michael Sync";
            }
            #endregion

            #region ID
            private int id;        
            public int Id
            {
                get { return id; }
                set { 
                    id = value;
                    this.RaisePropertyChanged(p => p.Id);
                }
            }
            #endregion 

            #region Name
            private string name = string.Empty;
            public string Name
            {
                get { 
                    return name; 
                }
                set { 
                    name = value;
                    this.RaisePropertyChanged(p => p.Name); 
                }
            }
            #endregion
        }
    #endregion
}
