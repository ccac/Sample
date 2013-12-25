using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace SilverlightDemoDataBinding
{
    public partial class Page : UserControl
    {
        User user;

        public Page()
        {
            InitializeComponent();

            user = new User();
            user.Name = "Alan Chen";
            user.Address = "Shen Zhen";

            lblName.DataContext = user;
            lblAddress.DataContext = user;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            user.Name = "Alan";
            user.Address = "深圳";
        }
    }

    //public class User
    //{
    //    public string Name { get; set; }
    //    public string Address { get; set; }
    //}

    public class User : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            {
                _name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,new PropertyChangedEventArgs("Name"));
                }
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                }
            }
        }
    }
}
