using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RBStyleTrail.DataAccess
{
    public class DateFunc
    {
        public static DateTime IncreaseMonthForMonthEnd(DateTime dt, int inum)
        {

            DateTime dtNew = dt.AddMonths(inum);
            return dtNew.AddDays((0 - dtNew.Day));
        }
    }
}
