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


/*
*	A Recruison Tree Demonstratoin in C#
*   from shinedraw.com
*/

namespace RecursionTree
{
    public partial class Page : UserControl
    {
        private RecursionTree _RecursionTree;
        public Page()
        {
            InitializeComponent();
            Cover.MouseLeftButtonDown += new MouseButtonEventHandler(Cover_MouseLeftButtonDown);
        }

        void Cover_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LayoutRoot.Children.Remove(Cover);

            // put the tree to the bottom center
            _RecursionTree = new RecursionTree(0);
            _RecursionTree.SetValue(Canvas.TopProperty, Height);
            _RecursionTree.SetValue(Canvas.LeftProperty, Width/2);
            LayoutRoot.Children.Insert(0, _RecursionTree);
            _RecursionTree.Start();
        }
    }
}
