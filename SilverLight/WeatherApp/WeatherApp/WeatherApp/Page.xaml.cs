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
using ControlsLibrary;

namespace WeatherApp
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

        }

        private void RainyButton_Click(object sender, RoutedEventArgs e)
        {
            WeatherGadget.Condition = Condition.Rainy;
            WeatherGadget.Temperature = "60";
            WeatherGadget.ConditionDescription = "Seattle-esque";
        }

        private void CloudyButton_Click(object sender, RoutedEventArgs e)
        {
            WeatherGadget.Condition = Condition.Cloudy;
            WeatherGadget.Temperature = "60";
            WeatherGadget.ConditionDescription = "Cloudy";
        }

        private void PartyCloudyButton_Click(object sender, RoutedEventArgs e)
        {
            WeatherGadget.Condition = Condition.PartlyCloudy;
            WeatherGadget.Temperature = "60";
            WeatherGadget.ConditionDescription = "Seattle-esque";
        }

        private void SunnyButton_Click(object sender, RoutedEventArgs e)
        {
            WeatherGadget.Condition = Condition.Sunny;
            WeatherGadget.Temperature = "80";
            WeatherGadget.ConditionDescription = "Sunny with blue skies";
        }
    }
}
