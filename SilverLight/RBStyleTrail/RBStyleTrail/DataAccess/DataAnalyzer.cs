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
//using Morningstar.Silverlight.Common.Xml;
using System.Linq;
using System.Xml;
using System.Xml.Linq; 

namespace RBStyleTrail.DataAccess
{
    public class DataAnalyzer : DataSource
    {
        private const string USERDATA_PATH = "settings/userdatas/userdata";
		private const string START_DATE_PATH = USERDATA_PATH+"/setting/start";
		private const string END_DATE_PATH = USERDATA_PATH+"/setting/end";
		private const string CALC_WINSZ_PATH =USERDATA_PATH+"/setting/calc";
		private const string ITEM_PATH = USERDATA_PATH+"/ItemList";
		private const string INDEX_PATH = USERDATA_PATH +"/styleindex";

        private const string RSQUARED_PATH = "securitydatas/secdata[@secid=fundID]/perfs/risks/risk";
		private const string RBST_PATH = "securitydatas/secdata[@secid=fundID]/rbst";

        private string m_sXml = "";
        private XDocument m_xDoc;
        private XElement m_xElement;

        public string m_sTitle { get; set; }
        public int m_iCalcWinSZ { get; set; }
        public DateTime m_dtStart { get; set; }
        public DateTime m_dtEnd { get; set; }
        public List<InvestmentData> m_listInvestData { get; set; }
        public List<InvestmentData> m_listInvestOrder { get; set; }
        public List<string> m_listIndexName { get; set; }
        public int m_iIndexRange { get; set; }

        private static DataAnalyzer instance;

        public static DataAnalyzer GetInstance()
        {
            if (instance == null)
            {
                instance = new DataAnalyzer();
            }

            return instance;
        }

        private DataAnalyzer()
        {
            m_sDefaultTitle = "Returns-Based Style Trail";
            m_listInvestData = new List<InvestmentData>();
            m_listInvestOrder = new List<InvestmentData>();
            m_listIndexName = new List<string>();
        }

        public void SetSouceXml(string sXml)
        {
            m_sXml = sXml;
            //m_oXmlDoc = new XmlDocument();
            //m_oXmlDoc.LoadXml(m_sXml);
            m_xDoc = XDocument.Parse(sXml);
            m_xElement = m_xDoc.Element("awd");

            ParseXml();
        }

        public void SetSouceXmlByFile(string sFileName)
        {
            m_xDoc = System.Xml.Linq.XDocument.Load(sFileName);
            m_xElement = m_xDoc.Element("awd");

            //m_oXmlDoc = new XmlDocument();
            //m_oXmlDoc.Load(sFileName);

            ParseXml();
        }

        private void ParseXml()
        {
            
            ParseSetting();
            ParseData();
        }

        private void ParseData()
        {
            GerRSquared();
            GetRBSTData();
        }

        private void GerRSquared()
        {

            string sPath = "";
            for (int i = 0; i < m_listInvestData.Count; i++)
            {
                sPath = RSQUARED_PATH;
                sPath = sPath.Replace("fundID", AddQuotations(m_listInvestData[i].m_sID));
                XElement element = m_xElement.SelectSingleElement(sPath);
                m_listInvestData[i].m_sRsquared = element.Attribute("R-Squared").Value;
                m_listInvestData[i].m_sRsquared = (decimal.Parse(m_listInvestData[i].m_sRsquared) * 100).ToString("0.00");
            }
        }

        private void GetRBSTData()
        {
            InvestmentData tempInvest;
            string sPath = "";
            for (int i = 0; i < m_listInvestData.Count; i++)
            {
                tempInvest = m_listInvestData[i];
                sPath = RBST_PATH;
                sPath = sPath.Replace("fundID", AddQuotations(m_listInvestData[i].m_sID));
                XElement element = m_xElement.SelectSingleElement(sPath);

                int dateStep = int.Parse(element.Attribute("step").Value);

                tempInvest.m_dtEarliest = DateTime.Parse(element.Attribute("start").Value);
                tempInvest.m_dtLatest = DateTime.Parse(element.Attribute("end").Value);
                tempInvest.m_nTotalTimeSpan = tempInvest.m_dtLatest - tempInvest.m_dtEarliest;
                tempInvest.m_listCR.Add(decimal.Parse(element.Attribute("v0").Value));
                tempInvest.m_listCR.Add(decimal.Parse(element.Attribute("v1").Value));
                tempInvest.m_listCR.Add(decimal.Parse(element.Attribute("v2").Value));
                tempInvest.m_listCR.Add(decimal.Parse(element.Attribute("v3").Value));

                foreach (XElement pelement in element.ChildElements("p"))
                {
                    int d = int.Parse(pelement.Attribute("d").Value.Trim());
                    tempInvest.m_listDate.Add(DateFunc.IncreaseMonthForMonthEnd(tempInvest.m_dtEarliest, d * dateStep));
                    tempInvest.m_listX.Add(decimal.Parse(pelement.Attribute("x").Value));
                    tempInvest.m_listY.Add(decimal.Parse(pelement.Attribute("y").Value));
                }
            }
        }

        private void ParseSetting()
        {
            
            //setting/title
            //XElement nodeTitle = m_xDoc.Element("awd").Element("settings").Element("userdates").Element("userdata").Element("setting").Element("title");
            XElement nodeTitle = m_xElement.SelectSingleElement(TITLE_PATH);
            if (nodeTitle == null)
            {
                m_sTitle = m_sDefaultTitle;
            }
            else
            {
                m_sTitle = nodeTitle.Attribute("v").Value;
            }

            //setting calc
            XElement nodeCalc = m_xElement.SelectSingleElement(CALC_WINSZ_PATH);
            m_iCalcWinSZ = int.Parse(nodeCalc.Attribute("winsz").Value.Trim());

            //setting start / end date
            GetDateRange();

            //setting itemlist
            GetItemList();

            //setting styleindex
            GetStyleIndex();
        }

        private void GetDateRange()
        {
            //setting start
            XElement nodeStart = m_xElement.SelectSingleElement(START_DATE_PATH);
            m_dtStart = DateTime.Parse(nodeStart.Attribute("date").Value.Trim());
            m_dtStart = m_dtStart.AddDays(-1);
            m_dtStart = DateFunc.IncreaseMonthForMonthEnd(m_dtStart, m_iCalcWinSZ);

            //setting end
            XElement nodeEnd = m_xElement.SelectSingleElement(END_DATE_PATH);
            m_dtEnd = DateTime.Parse(nodeEnd.Attribute("date").Value.Trim());
        }

        private void GetItemList()
        {
            InvestmentData tempInvest;
            XElement itemListNode = m_xElement.SelectSingleElement(ITEM_PATH);
            var items = itemListNode.ChildElements("Item");

            m_listInvestData.Clear();
            m_listInvestOrder.Clear();

            foreach (XElement item in items)
            {
                tempInvest = new InvestmentData();
                tempInvest.m_sID = item.Attribute("Id").Value.Trim();
                tempInvest.m_sName = item.Attribute("Name").Value;
                tempInvest.m_bShowCR = item.Attribute("cr").Value == "Show";
                tempInvest.m_uColor = GetHexColorStr(item.Attribute("color").Value.Trim());
                m_listInvestData.Add(tempInvest);
            }

            CalculateZorder();

            for (int i = 0; i < m_listInvestData.Count; i++)
            {
                m_listInvestOrder.Insert(0, GetInvestByZorder(i + 1));
            }
        }

        private void GetStyleIndex()
        {
            XElement styleIndexNode = m_xElement.SelectSingleElement(INDEX_PATH);
            var styleIndexs = styleIndexNode.ChildElements("Item"); 

            string strValue = "";
            strValue = styleIndexNode.Attribute("range").Value;
            m_iIndexRange = int.Parse(strValue);
            m_listIndexName.Clear();

            foreach (XElement styleIndex in styleIndexs)
            {
                if (styleIndex.Attribute("displayname") != null)
                {
                    strValue = styleIndex.Attribute("displayname").Value;
                }
                else
                {
                    strValue = styleIndex.Attribute("Name").Value;
                }
                m_listIndexName.Add(strValue);
            }
        }

        private void CalculateZorder()
		{
			bool bNeedReCal = false;
            int len = m_listInvestData.Count;
			int zorder = 0;

			for(int i = 0; i< len; i++)
			{
                zorder = m_listInvestData[i].m_uZorder;
				if(zorder == 0 || zorder >len)
				{
					bNeedReCal = true;
					break;
				}
			}
			
			if(bNeedReCal)
			{
				for(int i =0; i< len; i++)
                    m_listInvestData[i].m_uZorder = i + 1;
			}
			
		}

        private InvestmentData GetInvestByZorder(int zOrder)
        {
            for (int i = 0; i < m_listInvestData.Count; i++)
            {
                if (m_listInvestData[i].m_uZorder == zOrder)
                {
                    return m_listInvestData[i];
                }
            }
            return null;
        }

        
    }
}
