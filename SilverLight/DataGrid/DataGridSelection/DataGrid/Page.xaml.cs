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
using System.Xml.Linq;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DataGrid
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            this.LoadData();
        }

        private void LoadData()
        {
            XDocument nutritionsDoc = XDocument.Load( "Nutritions.xml" );
            List<Nutrition> data = ( from nutrition in nutritionsDoc.Descendants( "Nutrition" )
                                     select new Nutrition
                                     {
                                         Group = nutrition.Attribute( "Group" ).Value,
                                         Name = nutrition.Attribute( "Name" ).Value,
                                         Quantity = nutrition.Attribute( "Quantity" ).Value
                                     } ).ToList();

            Foods.ItemsSource = data;
            this.Foods.SelectedIndex = -1;
            TextBlock tb = new TextBlock();
            TextBox tx = new TextBox();
        }
    }
}
