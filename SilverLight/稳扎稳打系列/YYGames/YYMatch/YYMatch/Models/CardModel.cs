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

namespace YYMatch.Models
{
    public class CardModel
    {
        public string ImageName { get; set; }
        public int Position { get; set; }

        public CardModel(string imageName, int position)
        {
            ImageName = imageName;
            Position = position;
        }
    }
}
