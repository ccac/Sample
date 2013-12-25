using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace RBStyleTrail.DataAccess
{
    public class InvestmentData
    {
        public String m_sID;
		public String m_sName;
		public Color m_uColor;
		public int m_uZorder;
		public bool m_bShowCR;
		public List<DateTime> m_listDate;
		public DateTime m_dtEarliest;
		public DateTime m_dtLatest;
		public TimeSpan m_nTotalTimeSpan;
		public List<decimal> m_listX;
		public List<decimal> m_listY;
		public string m_sRsquared;
		public List<decimal> m_listCR;//Matrix (2*2)

        public InvestmentData()
        {
            m_sID = "";
            m_sName = "";
            m_uColor = Colors.Black;
            m_bShowCR = false;
            m_sRsquared = "";
            m_listDate = new List<DateTime>();
            m_listX = new List<decimal>();
            m_listY = new List<decimal>();
            m_listCR = new List<decimal>();
            m_nTotalTimeSpan = new TimeSpan(1);
        }


    }
}
