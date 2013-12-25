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

namespace CoreyMiller.VisualFx.Demo.CycloneParts
{
    public partial class CycloneParticle : UserControl
    {
        public CycloneParticle(double scale)
        {
            InitializeComponent();
            
            scale = scale / 100 + .25;

            ParticleScale.ScaleX = scale;
            ParticleScale.ScaleY = scale;

        }
    }
}
