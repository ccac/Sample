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
using VIEWS;
using System.Windows.Browser;

namespace VTest
{
    public partial class Page : UserControl
    {
        private VEMap map;
        private VEShapeLayer layer;

        public Page()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            map = new VEMap("mapContainer");
            HtmlPage.RegisterScriptableObject("SLMAP", map);
            map.LoadMap();
            map.Click += new EventHandler<VEMapEventArgs>(map_Click);
        }

        void map_Click(object sender, VEMapEventArgs e)
        {
            VEShape shape = map.GetShapeById(e.ElementId);
            VEPushpin pin = shape as VEPushpin;

            if (pin != null)
            {
                VELatLong point = pin.Point;

                string msg = string.Format("{0} is at {1}, {2}", pin.Title, point.Latitude, point.Longitude);
                HtmlPage.Window.Alert(msg);
                pin.LineToGround = true;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            map.Find(WhatBox.Text, WhereBox.Text);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            map.Clear();
        }

        private void AddPinsButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a bunch of pins
            VEPushpin[] pins = new VEPushpin[]
            {
                new VEPushpin() { Point = new VELatLong(30.10, -95.45), Title="A" },
                new VEPushpin() { Point = new VELatLong(30.09, -95.44), Title="B" },
                new VEPushpin() { Point = new VELatLong(30.08, -95.43), Title="C" },
                new VEPushpin() { Point = new VELatLong(30.07, -95.42), Title="D" },
                new VEPushpin() { Point = new VELatLong(30.06, -95.41), Title="E" },
            };

            // Create a layer
            layer = new VEShapeLayer();

            // Add the layer to the map
            map.AddShapeLayer(layer);

            // Add the pins to the layer
            layer.AddShape(pins);
        }

        private void ShowHideLayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (layer != null)
            {
                layer.Visible = !layer.Visible;
            }
        }
    }
}
