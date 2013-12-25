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
    public class DataSource
    {
        protected string m_sDefaultTitle = "";

        //General paths and constants
		protected const string SETTING_PATH = "settings/";
		protected const string SETTING_USERDATA_PATH = SETTING_PATH + "userdatas/userdata/";
		protected const string SETTING_CUSTOMCALCULATION_PATH = SETTING_PATH + "cuscalcs/cuscalc";
        protected const string TITLE_PATH = SETTING_USERDATA_PATH + "setting/title";
		protected const int MAX_LEGENDITEM_NUM = 15;


        protected Color GetHexColorStr(string colorStr)
        {
            if (colorStr.Length != 6)
                return Colors.Black;

            //Color.FromArgb(
            return Colors.Black;
        }

        public static string AddQuotations(string sText)
		{
			return "\"" + sText + "\"";
		}
    }
}
