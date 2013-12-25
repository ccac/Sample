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

using System.Xml;
using System.IO;
using System.Text;
using Morningstar.Silverlight.Common.Xml;

namespace SilverlightApplication3
{
   
    public partial class Page : UserControl
    {
        List<vData> list = new List<vData>();
        double MinV = 0;
        double MaxV = 0;
        DateTime startDate;
        DateTime endDate;
        int currentIndex = 0;
        DateTime currentBeginDate;

        XmlReader xmlReader;
        //XmlDocument xmldoc;

        public Page()
        {
            
            InitializeComponent();

            //InitData();
            InitDataByXmlDoc();
            
            DrawPath(startDate);
        }

        /// <summary>
        /// 初始化xml文件里面的数据到List中
        /// </summary>
        private void InitData()
        {
            vData vdata;

            //StreamReader sr = new StreamReader("sampleData.xml");

            xmlReader = XmlReader.Create(new StringReader(str));

            xmlReader.ReadToFollowing("ts");
            xmlReader.MoveToAttribute("start");
            startDate = DateTime.Parse(xmlReader.Value);
            xmlReader.MoveToAttribute("end");
            endDate = DateTime.Parse(xmlReader.Value);
            currentBeginDate = startDate;

            vdata = new vData();
            xmlReader.ReadToFollowing("t");
            xmlReader.MoveToAttribute("d");
            vdata.AttributeD = int.Parse(xmlReader.Value);
            xmlReader.MoveToAttribute("v");
            vdata.AttributeV = double.Parse(xmlReader.Value);
            xmlReader.MoveToAttribute("td");
            vdata.AttributeTD = int.Parse(xmlReader.Value);
            xmlReader.MoveToAttribute("odate");
            vdata.AttributeOdate = DateTime.Parse(xmlReader.Value);
            list.Add(vdata);
            //MinV = vdata.AttributeV;
            //MaxV = vdata.AttributeV;

            while (xmlReader.ReadToFollowing("t"))
            {
                vdata = new vData();
                try
                {
                    xmlReader.ReadToFollowing("t");
                    xmlReader.MoveToAttribute("d");
                    vdata.AttributeD = int.Parse(xmlReader.Value);
                    xmlReader.MoveToAttribute("v");
                    vdata.AttributeV = double.Parse(xmlReader.Value);
                    xmlReader.MoveToAttribute("td");
                    vdata.AttributeTD = int.Parse(xmlReader.Value);
                    xmlReader.MoveToAttribute("odate");
                    vdata.AttributeOdate = DateTime.Parse(xmlReader.Value);
                    list.Add(vdata);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                
                //MinV = MinV > vdata.AttributeV ? vdata.AttributeV : MinV;
                //MaxV = MaxV < vdata.AttributeV ? vdata.AttributeV : MaxV;
            }

        }

        private void InitDataByXmlDoc()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(str);
            XmlNode rootNode = xdoc.DocumentElement;
            XmlNode tsNode = rootNode.ChildNodes[0].ChildNodes[0];

            startDate = DateTime.Parse(tsNode.Attributes["start"].Value);
            endDate = DateTime.Parse(tsNode.Attributes["end"].Value);

            foreach (XmlNode tNode in tsNode.ChildNodes)
            {
                vData vdata = new vData();
                vdata.AttributeD = int.Parse(tNode.Attributes["d"].Value);
                vdata.AttributeV = double.Parse(tNode.Attributes["v"].Value);
                vdata.AttributeTD = int.Parse(tNode.Attributes["td"].Value);
                vdata.AttributeOdate = DateTime.Parse(tNode.Attributes["odate"].Value);
                list.Add(vdata);
            }
        }

        private void DrawPath(DateTime begin)
        {
            //坐标轴
            canvas1.Children.Clear();
            canvas1.Children.Add(CreateASingleLine(60,0,60,400));   //X
            canvas1.Children.Add(CreateASingleLine(60,400,660,400));    //Y

            //path
            System.Windows.Shapes.Path path1 = new System.Windows.Shapes.Path();
            SolidColorBrush pathBrush = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            path1.Stroke = pathBrush;
            canvas1.Children.Add(path1);


            PathGeometry pg = new PathGeometry();
            PathFigureCollection pfc = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            PathSegmentCollection psc = new PathSegmentCollection();
            LineSegment ls;
            vData vdata;
            vdata = list[currentIndex];

            //数据时间段。取5个月内的数据
            DateTime date1 = begin;
            DateTime date2 = date1.AddMonths(5);

            //确定数据在List中起点
            if (currentBeginDate > date1)
            {
                currentIndex--;
                while (currentIndex > -1)
                {
                    vdata = list[currentIndex];
                    if (vdata.AttributeOdate >= date1)
                    {
                        currentIndex--;
                    }
                    else
                    {
                        break;
                    }
                }
                currentIndex++;
                //vdata = list[currentIndex];
                currentBeginDate = vdata.AttributeOdate;
            }
            else if (currentBeginDate < date1)
            {
                currentIndex++;
                while (currentIndex < list.Count)
                {
                    vdata = list[currentIndex];
                    if (vdata.AttributeOdate < date1)
                    {
                        currentIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
                currentBeginDate = vdata.AttributeOdate;
                if(currentIndex == list.Count)
                    currentIndex--;
            }

            //取这5个月时间段内的最小值跟最大值
            MinV = vdata.AttributeV;
            MaxV = vdata.AttributeV;
            int i = currentIndex;
            i++;
            while (i < list.Count)
            {
                vdata = list[i];
                if (vdata.AttributeOdate <= date2)
                {
                    MinV = MinV > vdata.AttributeV ? vdata.AttributeV : MinV;
                    MaxV = MaxV < vdata.AttributeV ? vdata.AttributeV : MaxV;
                    i++;
                }
                else
                {
                    break;
                }
            }

            if (MaxV == MinV)
            {
                return;
            }

            //确定纵轴上标志的最小值和最大值
            double valueSpace = MaxV - MinV;
            valueSpace = valueSpace / 10;
            int minLine = (int)(MinV - valueSpace);
            int maxLine = (int)(MaxV + valueSpace);

            //画出网格
            DrawGridding(date1, date2, minLine, maxLine);

            //Path 起点
            int j = currentIndex;
            vdata = list[j];
            Point p;
            p = new Point((double)((vdata.AttributeOdate - date1).TotalDays * 4) + 60, (maxLine - vdata.AttributeV) * 400 / (maxLine - minLine));
            pf.StartPoint = p;

            //构建path其他点
            j++;
            while (j < list.Count)
            {
                vdata = list[j];
                if (vdata.AttributeOdate <= date2)
                {
                    p = new Point((double)((vdata.AttributeOdate - date1).TotalDays * 4) + 60, (maxLine - vdata.AttributeV) * 400 / (maxLine - minLine));
                    j++;

                    ls = new LineSegment();
                    ls.Point = p;
                    psc.Add(ls);
                }
                else
                {
                    break;
                }
            }

            pf.Segments = psc;
            pfc.Add(pf);
            pg.Figures = pfc;

            path1.Data = pg;

            tbBeginDate.Text = begin.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 坐标网格
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="minL"></param>
        /// <param name="maxL"></param>
        private void DrawGridding(DateTime start, DateTime end, int minL, int maxL)
        {
            DrawVerticalLine(start, end);
            DrawHorizontalLine(minL, maxL);
        }

        /// <summary>
        /// 根据日期。以每月为单位画横轴上的标识
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void DrawVerticalLine(DateTime start, DateTime end)
        {
            DateTime t;
            t = start;
            //DrawAText(t.ToString("yyyy-MM-dd"), 25, 400);
            double x = (DateTime.DaysInMonth(t.Year, t.Month) - start.Day) * 4 + 60;
            DrawADashLine(x, 0, x, 400);
            DrawAText(t.ToString("yyyy-MM-01"), x-25, 400);
            t = t.AddMonths(1);
            while (t < end)
            {
                x = x + DateTime.DaysInMonth(t.Year, t.Month) * 4;
                DrawADashLine(x, 0, x, 400);
                DrawAText(t.ToString("yyyy-MM-01"), x-25, 400);
                t = t.AddMonths(1);
            }
        }

        /// <summary>
        /// 根据最大最小值。画纵轴上的标识
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private void DrawHorizontalLine(int min, int max)
        {
            int s = (max - min) / 5;
            int t = min + s;
            if (s >= 5)
            {
                t = GetTidyValue(t);
            }

            DrawAText(min.ToString("0.00"), 0, 400);
            while (t <= max)
            {
                int h = (max - t) * 400 / (max - min);
                DrawADashLine(60, h, 660, h);
                DrawAText(t.ToString("0.00"), 0, h);
                if (s < 5)
                {
                    t = t + s;
                }
                else
                {
                    t = GetTidyValue(t + s);
                }
            }
        }

        
        private void DrawAText(string content, double X1, double Y1)
        {
            TextBlock tb = new TextBlock();
            tb.Text = content;
            tb.FontSize = 9;
            Thickness tn = new Thickness();
            tn.Top = Y1;
            tn.Left = X1;

            tb.Margin = tn;

            canvas1.Children.Add(tb);
        }

        /// <summary>
        /// 根据点返回一个黑色的单线
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <returns></returns>
        private Line CreateASingleLine(double X1, double Y1, double X2, double Y2)
        {
            Line line = new Line();
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            line.Stroke = brush;
            line.StrokeThickness = 1;
            return line;
        }

        /// <summary>
        /// 红色虚线
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        private void DrawADashLine(double X1, double Y1, double X2, double Y2)
        {
            Line line = CreateASingleLine(X1, Y1, X2, Y2);
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            line.Stroke = brush;
            line.StrokeThickness = 0.5;
            DoubleCollection dc = new DoubleCollection();
            dc.Add(2);
            dc.Add(8);
            line.StrokeDashArray = dc;

            canvas1.Children.Add(line);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //vData vdata = list[0];
            try
            {
                DrawPath(DateTime.Parse(tbBeginDate.Text.Trim()));
            }
            catch(Exception ex)
            {
                mymsg.DataContext = ex;
                mymsg.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 取得最靠近num的能被10整除的整数（尾数四舍五入）。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int GetTidyValue(int num)
        {
            return (int)(((double)num / 10)+0.5)* 10;
        }

        private void btnFront_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbBeginDate.Text = DateTime.Parse(tbBeginDate.Text.Trim()).AddDays(-1).ToString("yyyy-MM-dd");
                DrawPath(DateTime.Parse(tbBeginDate.Text.Trim()));
            }
            catch (Exception ex)
            {
                mymsg.DataContext = ex;
                mymsg.Visibility = Visibility.Visible;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbBeginDate.Text = DateTime.Parse(tbBeginDate.Text.Trim()).AddDays(1).ToString("yyyy-MM-dd");
                DrawPath(DateTime.Parse(tbBeginDate.Text.Trim()));
            }
            catch (Exception ex)
            {
                mymsg.DataContext = ex;
                mymsg.Visibility = Visibility.Visible;
            }
        }


        string str = @"<securitydatas>
	<secdata secid='FOUSA00EA6;FO'>
		<ts id='Z1' start='2003-06-16' end='2008-04-14' freq='d'>
			<t d='2785' v='694.276565' td='37786' odate='2003-06-16'/>
			<t d='2786' v='694.656000' td='37787' odate='2003-06-17'/>
			<t d='2787' v='693.563200' td='37788' odate='2003-06-18'/>
			<t d='2788' v='689.578000' td='37789' odate='2003-06-19'/>
			<t d='2789' v='689.331600' td='37790' odate='2003-06-20'/>
			<t d='2792' v='684.734035' td='37793' odate='2003-06-23'/>
			<t d='2793' v='685.820550' td='37794' odate='2003-06-24'/>
			<t d='2794' v='686.707235' td='37795' odate='2003-06-25'/>
			<t d='2795' v='689.079750' td='37796' odate='2003-06-26'/>
			<t d='2796' v='688.886000' td='37797' odate='2003-06-27'/>
			<t d='2799' v='688.739000' td='37800' odate='2003-06-30'/>
			<t d='2800' v='688.833600' td='37801' odate='2003-07-01'/>
			<t d='2801' v='692.872365' td='37802' odate='2003-07-02'/>
			<t d='2802' v='688.021166' td='37803' odate='2003-07-03'/>
			<t d='2806' v='690.719634' td='37807' odate='2003-07-07'/>
			<t d='2807' v='694.011366' td='37808' odate='2003-07-08'/>
			<t d='2808' v='693.870034' td='37809' odate='2003-07-09'/>
			<t d='2809' v='687.514466' td='37810' odate='2003-07-10'/>
			<t d='2810' v='692.211766' td='37811' odate='2003-07-11'/>
			<t d='2813' v='694.132234' td='37814' odate='2003-07-14'/>
			<t d='2814' v='690.778534' td='37815' odate='2003-07-15'/>
			<t d='2815' v='688.631434' td='37816' odate='2003-07-16'/>
			<t d='2816' v='685.845600' td='37817' odate='2003-07-17'/>
			<t d='2817' v='689.400000' td='37818' odate='2003-07-18'/>
			<t d='2820' v='684.812666' td='37821' odate='2003-07-21'/>
			<t d='2821' v='689.344500' td='37822' odate='2003-07-22'/>
			<t d='2822' v='689.412604' td='37823' odate='2003-07-23'/>
			<t d='2823' v='686.562750' td='37824' odate='2003-07-24'/>
			<t d='2824' v='692.128034' td='37825' odate='2003-07-25'/>
			<t d='2827' v='692.614566' td='37828' odate='2003-07-28'/>
			<t d='2828' v='689.662766' td='37829' odate='2003-07-29'/>
			<t d='2829' v='688.975234' td='37830' odate='2003-07-30'/>
			<t d='2830' v='690.010691' td='37831' odate='2003-07-31'/>
			<t d='2831' v='688.865116' td='37832' odate='2003-08-01'/>
			<t d='2834' v='687.023166' td='37835' odate='2003-08-04'/>
			<t d='2835' v='682.260284' td='37836' odate='2003-08-05'/>
			<t d='2836' v='679.255366' td='37837' odate='2003-08-06'/>
			<t d='2837' v='682.010966' td='37838' odate='2003-08-07'/>
			<t d='2838' v='682.793600' td='37839' odate='2003-08-08'/>
			<t d='2841' v='684.739634' td='37842' odate='2003-08-11'/>
			<t d='2842' v='688.975234' td='37843' odate='2003-08-12'/>
			<t d='2843' v='687.286600' td='37844' odate='2003-08-13'/>
			<t d='2844' v='689.246000' td='37845' odate='2003-08-14'/>
			<t d='2845' v='691.212574' td='37846' odate='2003-08-15'/>
			<t d='2848' v='693.545966' td='37849' odate='2003-08-18'/>
			<t d='2849' v='698.345665' td='37850' odate='2003-08-19'/>
			<t d='2850' v='695.617466' td='37851' odate='2003-08-20'/>
			<t d='2851' v='698.501700' td='37852' odate='2003-08-21'/>
			<t d='2852' v='697.531671' td='37853' odate='2003-08-22'/>
			<t d='2855' v='695.431834' td='37856' odate='2003-08-25'/>
			<t d='2856' v='697.858720' td='37857' odate='2003-08-26'/>
			<t d='2857' v='699.118166' td='37858' odate='2003-08-27'/>
			<t d='2858' v='704.219180' td='37859' odate='2003-08-28'/>
			<t d='2859' v='705.362700' td='37860' odate='2003-08-29'/>
			<t d='2863' v='708.485470' td='37864' odate='2003-09-02'/>
			<t d='2864' v='708.257000' td='37865' odate='2003-09-03'/>
			<t d='2865' v='708.009420' td='37866' odate='2003-09-04'/>
			<t d='2866' v='704.474750' td='37867' odate='2003-09-05'/>
			<t d='2869' v='707.559034' td='37870' odate='2003-09-08'/>
			<t d='2870' v='702.554616' td='37871' odate='2003-09-09'/>
			<t d='2871' v='697.481366' td='37872' odate='2003-09-10'/>
			<t d='2872' v='698.808600' td='37873' odate='2003-09-11'/>
			<t d='2873' v='700.321604' td='37874' odate='2003-09-12'/>
			<t d='2876' v='699.050000' td='37877' odate='2003-09-15'/>
			<t d='2877' v='703.482966' td='37878' odate='2003-09-16'/>
			<t d='2878' v='701.707200' td='37879' odate='2003-09-17'/>
			<t d='2879' v='705.322134' td='37880' odate='2003-09-18'/>
			<t d='2880' v='705.175466' td='37881' odate='2003-09-19'/>
			<t d='2883' v='695.942000' td='37884' odate='2003-09-22'/>
			<t d='2884' v='695.072284' td='37885' odate='2003-09-23'/>
			<t d='2885' v='690.667634' td='37886' odate='2003-09-24'/>
			<t d='2886' v='685.936966' td='37887' odate='2003-09-25'/>
			<t d='2887' v='683.334866' td='37888' odate='2003-09-26'/>
			<t d='2890' v='685.832550' td='37891' odate='2003-09-29'/>
			<t d='2891' v='685.327966' td='37892' odate='2003-09-30'/>
			<t d='2892' v='692.224000' td='37893' odate='2003-10-01'/>
			<t d='2893' v='690.453066' td='37894' odate='2003-10-02'/>
			<t d='2894' v='691.904850' td='37895' odate='2003-10-03'/>
			<t d='2897' v='692.562000' td='37898' odate='2003-10-06'/>
			<t d='2898' v='694.648900' td='37899' odate='2003-10-07'/>
			<t d='2899' v='694.809550' td='37900' odate='2003-10-08'/>
			<t d='2900' v='693.694366' td='37901' odate='2003-10-09'/>
			<t d='2901' v='693.471200' td='37902' odate='2003-10-10'/>
			<t d='2904' v='697.590034' td='37905' odate='2003-10-13'/>
			<t d='2905' v='703.064500' td='37906' odate='2003-10-14'/>
			<t d='2906' v='701.730034' td='37907' odate='2003-10-15'/>
			<t d='2907' v='701.948434' td='37908' odate='2003-10-16'/>
			<t d='2908' v='700.638400' td='37909' odate='2003-10-17'/>
			<t d='2911' v='702.615166' td='37912' odate='2003-10-20'/>
			<t d='2912' v='705.396250' td='37913' odate='2003-10-21'/>
			<t d='2913' v='702.440000' td='37914' odate='2003-10-22'/>
			<t d='2914' v='701.510300' td='37915' odate='2003-10-23'/>
			<t d='2915' v='703.430200' td='37916' odate='2003-10-24'/>
			<t d='2918' v='704.479966' td='37919' odate='2003-10-27'/>
			<t d='2919' v='707.936366' td='37920' odate='2003-10-28'/>
			<t d='2920' v='710.938284' td='37921' odate='2003-10-29'/>
			<t d='2921' v='710.858366' td='37922' odate='2003-10-30'/>
			<t d='2922' v='714.154500' td='37923' odate='2003-10-31'/>
			<t d='2925' v='713.739600' td='37926' odate='2003-11-03'/>
			<t d='2926' v='711.280000' td='37927' odate='2003-11-04'/>
			<t d='2927' v='713.529800' td='37928' odate='2003-11-05'/>
			<t d='2928' v='716.696050' td='37929' odate='2003-11-06'/>
			<t d='2929' v='716.110950' td='37930' odate='2003-11-07'/>
			<t d='2932' v='713.135250' td='37933' odate='2003-11-10'/>
			<t d='2933' v='712.823784' td='37934' odate='2003-11-11'/>
			<t d='2934' v='718.291234' td='37935' odate='2003-11-12'/>
			<t d='2935' v='720.631166' td='37936' odate='2003-11-13'/>
			<t d='2936' v='720.058034' td='37937' odate='2003-11-14'/>
			<t d='2939' v='718.593600' td='37940' odate='2003-11-17'/>
			<t d='2940' v='718.056684' td='37941' odate='2003-11-18'/>
			<t d='2941' v='718.487800' td='37942' odate='2003-11-19'/>
			<t d='2942' v='712.560566' td='37943' odate='2003-11-20'/>
			<t d='2943' v='714.499600' td='37944' odate='2003-11-21'/>
			<t d='2946' v='720.074800' td='37947' odate='2003-11-24'/>
			<t d='2947' v='722.200466' td='37948' odate='2003-11-25'/>
			<t d='2948' v='726.627734' td='37949' odate='2003-11-26'/>
			<t d='2950' v='727.053534' td='37951' odate='2003-11-28'/>
			<t d='2953' v='732.449016' td='37954' odate='2003-12-01'/>
			<t d='2954' v='734.117500' td='37955' odate='2003-12-02'/>
			<t d='2955' v='732.000566' td='37956' odate='2003-12-03'/>
			<t d='2956' v='733.383450' td='37957' odate='2003-12-04'/>
			<t d='2957' v='732.234316' td='37958' odate='2003-12-05'/>
			<t d='2960' v='734.458500' td='37961' odate='2003-12-08'/>
			<t d='2961' v='730.337650' td='37962' odate='2003-12-09'/>
			<t d='2962' v='728.620000' td='37963' odate='2003-12-10'/>
			<t d='2963' v='734.691100' td='37964' odate='2003-12-11'/>
			<t d='2964' v='735.960716' td='37965' odate='2003-12-12'/>
			<t d='2967' v='731.322500' td='37968' odate='2003-12-15'/>
			<t d='2968' v='732.342984' td='37969' odate='2003-12-16'/>
			<t d='2969' v='732.161216' td='37970' odate='2003-12-17'/>
			<t d='2970' v='739.008366' td='37971' odate='2003-12-18'/>
			<t d='2971' v='739.457366' td='37972' odate='2003-12-19'/>
			<t d='2974' v='739.840000' td='37975' odate='2003-12-22'/>
			<t d='2975' v='742.329150' td='37976' odate='2003-12-23'/>
			<t d='2976' v='742.220000' td='37977' odate='2003-12-24'/>
			<t d='2978' v='743.919966' td='37979' odate='2003-12-26'/>
			<t d='2981' v='749.240334' td='37982' odate='2003-12-29'/>
			<t d='2982' v='750.361466' td='37983' odate='2003-12-30'/>
			<t d='2983' v='747.579000' td='37984' odate='2003-12-31'/>
			<t d='2985' v='745.071750' td='37986' odate='2004-01-02'/>
			<t d='2988' v='746.392466' td='37989' odate='2004-01-05'/>
			<t d='2989' v='745.420800' td='37990' odate='2004-01-06'/>
			<t d='2990' v='747.108766' td='37991' odate='2004-01-07'/>
			<t d='2991' v='748.912534' td='37992' odate='2004-01-08'/>
			<t d='2992' v='749.032216' td='37993' odate='2004-01-09'/>
			<t d='2995' v='748.715100' td='37996' odate='2004-01-12'/>
			<t d='2996' v='747.706050' td='37997' odate='2004-01-13'/>
			<t d='2997' v='752.414984' td='37998' odate='2004-01-14'/>
			<t d='2998' v='753.532034' td='37999' odate='2004-01-15'/>
			<t d='2999' v='755.217700' td='38000' odate='2004-01-16'/>
			<t d='3003' v='756.789534' td='38004' odate='2004-01-20'/>
			<t d='3004' v='756.114800' td='38005' odate='2004-01-21'/>
			<t d='3005' v='756.559400' td='38006' odate='2004-01-22'/>
			<t d='3006' v='756.223450' td='38007' odate='2004-01-23'/>
			<t d='3009' v='762.744116' td='38010' odate='2004-01-26'/>
			<t d='3010' v='755.417900' td='38011' odate='2004-01-27'/>
			<t d='3011' v='745.142067' td='38012' odate='2004-01-28'/>
			<t d='3012' v='744.255000' td='38013' odate='2004-01-29'/>
			<t d='3013' v='744.705500' td='38014' odate='2004-01-30'/>
			<t d='3016' v='746.258667' td='38017' odate='2004-02-02'/>
			<t d='3017' v='747.248283' td='38018' odate='2004-02-03'/>
			<t d='3018' v='741.356000' td='38019' odate='2004-02-04'/>
			<t d='3019' v='742.020000' td='38020' odate='2004-02-05'/>
			<t d='3020' v='749.567733' td='38021' odate='2004-02-06'/>
			<t d='3023' v='748.559283' td='38024' odate='2004-02-09'/>
			<t d='3024' v='750.297617' td='38025' odate='2004-02-10'/>
			<t d='3025' v='753.687000' td='38026' odate='2004-02-11'/>
			<t d='3026' v='752.476650' td='38027' odate='2004-02-12'/>
			<t d='3027' v='751.039033' td='38028' odate='2004-02-13'/>
			<t d='3031' v='756.004000' td='38032' odate='2004-02-17'/>
			<t d='3032' v='753.756417' td='38033' odate='2004-02-18'/>
			<t d='3033' v='750.027600' td='38034' odate='2004-02-19'/>
			<t d='3034' v='748.391033' td='38035' odate='2004-02-20'/>
			<t d='3037' v='750.007167' td='38038' odate='2004-02-23'/>
			<t d='3038' v='749.787467' td='38039' odate='2004-02-24'/>
			<t d='3039' v='752.976000' td='38040' odate='2004-02-25'/>
			<t d='3040' v='753.186033' td='38041' odate='2004-02-26'/>
			<t d='3041' v='760.160483' td='38042' odate='2004-02-27'/>
			<t d='3044' v='761.413850' td='38045' odate='2004-03-01'/>
			<t d='3045' v='759.430000' td='38046' odate='2004-03-02'/>
			<t d='3046' v='762.112000' td='38047' odate='2004-03-03'/>
			<t d='3047' v='764.441567' td='38048' odate='2004-03-04'/>
			<t d='3048' v='766.948400' td='38049' odate='2004-03-05'/>
			<t d='3051' v='766.233000' td='38052' odate='2004-03-08'/>
			<t d='3052' v='761.299300' td='38053' odate='2004-03-09'/>
			<t d='3053' v='759.963750' td='38054' odate='2004-03-10'/>
			<t d='3054' v='751.004550' td='38055' odate='2004-03-11'/>
			<t d='3055' v='759.288600' td='38056' odate='2004-03-12'/>
			<t d='3058' v='752.709500' td='38059' odate='2004-03-15'/>
			<t d='3059' v='750.897900' td='38060' odate='2004-03-16'/>
			<t d='3060' v='754.199067' td='38061' odate='2004-03-17'/>
			<t d='3061' v='753.148800' td='38062' odate='2004-03-18'/>
			<t d='3062' v='752.425300' td='38063' odate='2004-03-19'/>
			<t d='3065' v='747.764533' td='38066' odate='2004-03-22'/>
			<t d='3066' v='748.991967' td='38067' odate='2004-03-23'/>
			<t d='3067' v='743.762867' td='38068' odate='2004-03-24'/>
			<t d='3068' v='749.190000' td='38069' odate='2004-03-25'/>
			<t d='3069' v='751.421567' td='38070' odate='2004-03-26'/>
			<t d='3072' v='752.434650' td='38073' odate='2004-03-29'/>
			<t d='3073' v='755.535250' td='38074' odate='2004-03-30'/>
			<t d='3074' v='755.873033' td='38075' odate='2004-03-31'/>
			<t d='3075' v='755.320000' td='38076' odate='2004-04-01'/>
			<t d='3076' v='760.567500' td='38077' odate='2004-04-02'/>
			<t d='3079' v='763.048033' td='38080' odate='2004-04-05'/>
			<t d='3080' v='762.487233' td='38081' odate='2004-04-06'/>
			<t d='3081' v='763.085600' td='38082' odate='2004-04-07'/>
			<t d='3082' v='761.635000' td='38083' odate='2004-04-08'/>
			<t d='3086' v='764.267000' td='38087' odate='2004-04-12'/>
			<t d='3087' v='754.485200' td='38088' odate='2004-04-13'/>
			<t d='3088' v='756.926700' td='38089' odate='2004-04-14'/>
			<t d='3089' v='763.816800' td='38090' odate='2004-04-15'/>
			<t d='3090' v='772.475000' td='38091' odate='2004-04-16'/>
			<t d='3093' v='766.018800' td='38094' odate='2004-04-19'/>
			<t d='3094' v='761.385000' td='38095' odate='2004-04-20'/>
			<t d='3095' v='766.938450' td='38096' odate='2004-04-21'/>
			<t d='3096' v='776.195033' td='38097' odate='2004-04-22'/>
			<t d='3097' v='774.967800' td='38098' odate='2004-04-23'/>
			<t d='3100' v='772.454717' td='38101' odate='2004-04-26'/>
			<t d='3101' v='772.252800' td='38102' odate='2004-04-27'/>
			<t d='3102' v='765.660800' td='38103' odate='2004-04-28'/>
			<t d='3103' v='766.357100' td='38104' odate='2004-04-29'/>
			<t d='3104' v='767.049800' td='38105' odate='2004-04-30'/>
			<t d='3107' v='771.036167' td='38108' odate='2004-05-03'/>
			<t d='3108' v='769.991717' td='38109' odate='2004-05-04'/>
			<t d='3109' v='769.923750' td='38110' odate='2004-05-05'/>
			<t d='3110' v='765.387633' td='38111' odate='2004-05-06'/>
			<t d='3111' v='762.984433' td='38112' odate='2004-05-07'/>
			<t d='3114' v='762.509000' td='38115' odate='2004-05-10'/>
			<t d='3115' v='763.262500' td='38116' odate='2004-05-11'/>
			<t d='3116' v='761.186033' td='38117' odate='2004-05-12'/>
			<t d='3117' v='761.328750' td='38118' odate='2004-05-13'/>
			<t d='3118' v='764.255966' td='38119' odate='2004-05-14'/>
			<t d='3121' v='761.145250' td='38122' odate='2004-05-17'/>
			<t d='3122' v='761.703334' td='38123' odate='2004-05-18'/>
			<t d='3123' v='756.271200' td='38124' odate='2004-05-19'/>
			<t d='3124' v='761.476434' td='38125' odate='2004-05-20'/>
			<t d='3125' v='759.088033' td='38126' odate='2004-05-21'/>
			<t d='3128' v='760.760000' td='38129' odate='2004-05-24'/>
			<t d='3129' v='767.782367' td='38130' odate='2004-05-25'/>
			<t d='3130' v='767.646900' td='38131' odate='2004-05-26'/>
			<t d='3131' v='769.202033' td='38132' odate='2004-05-27'/>
			<t d='3132' v='766.475000' td='38133' odate='2004-05-28'/>
			<t d='3136' v='771.922200' td='38137' odate='2004-06-01'/>
			<t d='3137' v='772.888917' td='38138' odate='2004-06-02'/>
			<t d='3138' v='770.319267' td='38139' odate='2004-06-03'/>
			<t d='3139' v='773.120217' td='38140' odate='2004-06-04'/>
			<t d='3142' v='777.038817' td='38143' odate='2004-06-07'/>
			<t d='3143' v='778.172150' td='38144' odate='2004-06-08'/>
			<t d='3144' v='771.763233' td='38145' odate='2004-06-09'/>
			<t d='3145' v='776.388033' td='38146' odate='2004-06-10'/>
			<t d='3149' v='774.520033' td='38150' odate='2004-06-14'/>
			<t d='3150' v='784.044966' td='38151' odate='2004-06-15'/>
			<t d='3151' v='783.159000' td='38152' odate='2004-06-16'/>
			<t d='3152' v='785.772000' td='38153' odate='2004-06-17'/>
			<t d='3153' v='787.130850' td='38154' odate='2004-06-18'/>
			<t d='3156' v='786.283684' td='38157' odate='2004-06-21'/>
			<t d='3157' v='787.746500' td='38158' odate='2004-06-22'/>
			<t d='3158' v='794.270400' td='38159' odate='2004-06-23'/>
			<t d='3159' v='788.660000' td='38160' odate='2004-06-24'/>
			<t d='3160' v='785.959350' td='38161' odate='2004-06-25'/>
			<t d='3163' v='781.536000' td='38164' odate='2004-06-28'/>
			<t d='3164' v='783.928466' td='38165' odate='2004-06-29'/>
			<t d='3165' v='786.166834' td='38166' odate='2004-06-30'/>
			<t d='3166' v='777.018000' td='38167' odate='2004-07-01'/>
			<t d='3167' v='776.821016' td='38168' odate='2004-07-02'/>
			<t d='3171' v='774.690600' td='38172' odate='2004-07-06'/>
			<t d='3172' v='775.340534' td='38173' odate='2004-07-07'/>
			<t d='3173' v='771.880000' td='38174' odate='2004-07-08'/>
			<t d='3174' v='773.117134' td='38175' odate='2004-07-09'/>
			<t d='3177' v='774.498934' td='38178' odate='2004-07-12'/>
			<t d='3178' v='773.126700' td='38179' odate='2004-07-13'/>
			<t d='3179' v='773.063166' td='38180' odate='2004-07-14'/>
			<t d='3180' v='775.951200' td='38181' odate='2004-07-15'/>
			<t d='3181' v='774.696000' td='38182' odate='2004-07-16'/>
			<t d='3184' v='775.842000' td='38185' odate='2004-07-19'/>
			<t d='3185' v='772.771966' td='38186' odate='2004-07-20'/>
			<t d='3186' v='768.950000' td='38187' odate='2004-07-21'/>
			<t d='3187' v='768.503966' td='38188' odate='2004-07-22'/>
			<t d='3188' v='768.288000' td='38189' odate='2004-07-23'/>
			<t d='3191' v='766.591000' td='38192' odate='2004-07-26'/>
			<t d='3192' v='770.325966' td='38193' odate='2004-07-27'/>
			<t d='3193' v='771.232500' td='38194' odate='2004-07-28'/>
			<t d='3194' v='775.431234' td='38195' odate='2004-07-29'/>
			<t d='3195' v='774.971966' td='38196' odate='2004-07-30'/>
			<t d='3198' v='773.606584' td='38199' odate='2004-08-02'/>
			<t d='3199' v='775.888400' td='38200' odate='2004-08-03'/>
			<t d='3200' v='771.233400' td='38201' odate='2004-08-04'/>
			<t d='3201' v='765.672050' td='38202' odate='2004-08-05'/>
			<t d='3202' v='761.899600' td='38203' odate='2004-08-06'/>
			<t d='3205' v='760.799034' td='38206' odate='2004-08-09'/>
			<t d='3206' v='763.872850' td='38207' odate='2004-08-10'/>
			<t d='3207' v='763.755266' td='38208' odate='2004-08-11'/>
			<t d='3208' v='761.794000' td='38209' odate='2004-08-12'/>
			<t d='3209' v='762.384000' td='38210' odate='2004-08-13'/>
			<t d='3212' v='765.337500' td='38213' odate='2004-08-16'/>
			<t d='3213' v='764.994400' td='38214' odate='2004-08-17'/>
			<t d='3214' v='767.716766' td='38215' odate='2004-08-18'/>
			<t d='3215' v='763.539200' td='38216' odate='2004-08-19'/>
			<t d='3216' v='766.366000' td='38217' odate='2004-08-20'/>
			<t d='3219' v='764.669534' td='38220' odate='2004-08-23'/>
			<t d='3220' v='767.599734' td='38221' odate='2004-08-24'/>
			<t d='3221' v='769.823800' td='38222' odate='2004-08-25'/>
			<t d='3222' v='767.044284' td='38223' odate='2004-08-26'/>
			<t d='3223' v='768.854250' td='38224' odate='2004-08-27'/>
			<t d='3226' v='765.235850' td='38227' odate='2004-08-30'/>
			<t d='3227' v='771.006000' td='38228' odate='2004-08-31'/>
			<t d='3228' v='769.071966' td='38229' odate='2004-09-01'/>
			<t d='3229' v='770.639966' td='38230' odate='2004-09-02'/>
			<t d='3230' v='768.715200' td='38231' odate='2004-09-03'/>
			<t d='3234' v='775.414750' td='38235' odate='2004-09-07'/>
			<t d='3235' v='771.441534' td='38236' odate='2004-09-08'/>
			<t d='3236' v='771.992000' td='38237' odate='2004-09-09'/>
			<t d='3237' v='773.562000' td='38238' odate='2004-09-10'/>
			<t d='3240' v='778.458766' td='38241' odate='2004-09-13'/>
			<t d='3241' v='778.556350' td='38242' odate='2004-09-14'/>
			<t d='3242' v='773.212500' td='38243' odate='2004-09-15'/>
			<t d='3243' v='773.621700' td='38244' odate='2004-09-16'/>
			<t d='3244' v='780.249634' td='38245' odate='2004-09-17'/>
			<t d='3247' v='779.246050' td='38248' odate='2004-09-20'/>
			<t d='3248' v='783.436500' td='38249' odate='2004-09-21'/>
			<t d='3249' v='775.014766' td='38250' odate='2004-09-22'/>
			<t d='3250' v='776.621966' td='38251' odate='2004-09-23'/>
			<t d='3251' v='779.443234' td='38252' odate='2004-09-24'/>
			<t d='3254' v='777.431284' td='38255' odate='2004-09-27'/>
			<t d='3255' v='785.088034' td='38256' odate='2004-09-28'/>
			<t d='3256' v='780.134434' td='38257' odate='2004-09-29'/>
			<t d='3257' v='785.386434' td='38258' odate='2004-09-30'/>
			<t d='3258' v='784.603766' td='38259' odate='2004-10-01'/>
			<t d='3261' v='783.431750' td='38262' odate='2004-10-04'/>
			<t d='3262' v='789.541234' td='38263' odate='2004-10-05'/>
			<t d='3263' v='789.880384' td='38264' odate='2004-10-06'/>
			<t d='3264' v='785.810550' td='38265' odate='2004-10-07'/>
			<t d='3265' v='780.396466' td='38266' odate='2004-10-08'/>
			<t d='3268' v='783.410160' td='38269' odate='2004-10-11'/>
			<t d='3269' v='783.528716' td='38270' odate='2004-10-12'/>
			<t d='3270' v='780.832184' td='38271' odate='2004-10-13'/>
			<t d='3271' v='780.026134' td='38272' odate='2004-10-14'/>
			<t d='3272' v='780.011966' td='38273' odate='2004-10-15'/>
			<t d='3275' v='781.373400' td='38276' odate='2004-10-18'/>
			<t d='3276' v='778.217700' td='38277' odate='2004-10-19'/>
			<t d='3277' v='779.658566' td='38278' odate='2004-10-20'/>
			<t d='3278' v='780.771384' td='38279' odate='2004-10-21'/>
			<t d='3279' v='776.093466' td='38280' odate='2004-10-22'/>
			<t d='3282' v='778.612500' td='38283' odate='2004-10-25'/>
			<t d='3283' v='783.071800' td='38284' odate='2004-10-26'/>
			<t d='3284' v='782.064466' td='38285' odate='2004-10-27'/>
			<t d='3285' v='779.414433' td='38286' odate='2004-10-28'/>
			<t d='3286' v='779.004600' td='38287' odate='2004-10-29'/>
			<t d='3289' v='780.388500' td='38290' odate='2004-11-01'/>
			<t d='3290' v='779.530833' td='38291' odate='2004-11-02'/>
			<t d='3291' v='785.784383' td='38292' odate='2004-11-03'/>
			<t d='3292' v='787.644000' td='38293' odate='2004-11-04'/>
			<t d='3293' v='784.539467' td='38294' odate='2004-11-05'/>
			<t d='3296' v='782.508400' td='38297' odate='2004-11-08'/>
			<t d='3297' v='779.545600' td='38298' odate='2004-11-09'/>
			<t d='3298' v='783.512500' td='38299' odate='2004-11-10'/>
			<t d='3299' v='785.436800' td='38300' odate='2004-11-11'/>
			<t d='3300' v='790.200767' td='38301' odate='2004-11-12'/>
			<t d='3303' v='784.398000' td='38304' odate='2004-11-15'/>
			<t d='3304' v='780.967967' td='38305' odate='2004-11-16'/>
			<t d='3305' v='777.945000' td='38306' odate='2004-11-17'/>
			<t d='3306' v='778.909733' td='38307' odate='2004-11-18'/>
			<t d='3307' v='770.687500' td='38308' odate='2004-11-19'/>
			<t d='3310' v='775.383568' td='38311' odate='2004-11-22'/>
			<t d='3311' v='776.682400' td='38312' odate='2004-11-23'/>
			<t d='3312' v='773.917400' td='38313' odate='2004-11-24'/>
			<t d='3314' v='775.213168' td='38315' odate='2004-11-26'/>
			<t d='3317' v='771.196800' td='38318' odate='2004-11-29'/>
			<t d='3318' v='772.110968' td='38319' odate='2004-11-30'/>
			<t d='3319' v='775.577550' td='38320' odate='2004-12-01'/>
			<t d='3320' v='761.772600' td='38321' odate='2004-12-02'/>
			<t d='3321' v='773.001950' td='38322' odate='2004-12-03'/>
			<t d='3324' v='769.080000' td='38325' odate='2004-12-06'/>
			<t d='3325' v='763.100232' td='38326' odate='2004-12-07'/>
			<t d='3326' v='766.943132' td='38327' odate='2004-12-08'/>
			<t d='3327' v='745.339368' td='38328' odate='2004-12-09'/>
			<t d='3328' v='749.941500' td='38329' odate='2004-12-10'/>
			<t d='3331' v='750.031118' td='38332' odate='2004-12-13'/>
			<t d='3332' v='752.822968' td='38333' odate='2004-12-14'/>
			<t d='3333' v='758.213368' td='38334' odate='2004-12-15'/>
			<t d='3334' v='753.288968' td='38335' odate='2004-12-16'/>
			<t d='3335' v='753.457232' td='38336' odate='2004-12-17'/>
			<t d='3338' v='753.108032' td='38339' odate='2004-12-20'/>
			<t d='3339' v='755.489250' td='38340' odate='2004-12-21'/>
			<t d='3340' v='755.380018' td='38341' odate='2004-12-22'/>
			<t d='3341' v='754.475300' td='38342' odate='2004-12-23'/>
			<t d='3345' v='749.301500' td='38346' odate='2004-12-27'/>
			<t d='3346' v='751.541068' td='38347' odate='2004-12-28'/>
			<t d='3347' v='751.861418' td='38348' odate='2004-12-29'/>
			<t d='3348' v='750.857850' td='38349' odate='2004-12-30'/>
			<t d='3349' v='744.715000' td='38350' odate='2004-12-31'/>
			<t d='3352' v='736.855600' td='38353' odate='2005-01-03'/>
			<t d='3353' v='732.672032' td='38354' odate='2005-01-04'/>
			<t d='3354' v='733.112768' td='38355' odate='2005-01-05'/>
			<t d='3355' v='738.111532' td='38356' odate='2005-01-06'/>
			<t d='3356' v='736.870200' td='38357' odate='2005-01-07'/>
			<t d='3359' v='744.397500' td='38360' odate='2005-01-10'/>
			<t d='3360' v='738.009300' td='38361' odate='2005-01-11'/>
			<t d='3361' v='739.442000' td='38362' odate='2005-01-12'/>
			<t d='3362' v='734.146168' td='38363' odate='2005-01-13'/>
			<t d='3363' v='737.528032' td='38364' odate='2005-01-14'/>
			<t d='3367' v='743.036968' td='38368' odate='2005-01-18'/>
			<t d='3368' v='731.792600' td='38369' odate='2005-01-19'/>
			<t d='3369' v='734.377450' td='38370' odate='2005-01-20'/>
			<t d='3370' v='733.824032' td='38371' odate='2005-01-21'/>
			<t d='3373' v='731.311900' td='38374' odate='2005-01-24'/>
			<t d='3374' v='732.806250' td='38375' odate='2005-01-25'/>
			<t d='3375' v='736.631282' td='38376' odate='2005-01-26'/>
			<t d='3376' v='736.806118' td='38377' odate='2005-01-27'/>
			<t d='3377' v='731.434350' td='38378' odate='2005-01-28'/>
			<t d='3380' v='736.748750' td='38381' odate='2005-01-31'/>
			<t d='3381' v='738.915800' td='38382' odate='2005-02-01'/>
			<t d='3382' v='739.302300' td='38383' odate='2005-02-02'/>
			<t d='3383' v='735.332000' td='38384' odate='2005-02-03'/>
			<t d='3384' v='739.309982' td='38385' odate='2005-02-04'/>
			<t d='3387' v='737.124832' td='38388' odate='2005-02-07'/>
			<t d='3388' v='741.628500' td='38389' odate='2005-02-08'/>
			<t d='3389' v='742.166432' td='38390' odate='2005-02-09'/>
			<t d='3390' v='743.451118' td='38391' odate='2005-02-10'/>
			<t d='3391' v='743.458500' td='38392' odate='2005-02-11'/>
			<t d='3394' v='741.622950' td='38395' odate='2005-02-14'/>
			<t d='3395' v='741.079769' td='38396' odate='2005-02-15'/>
			<t d='3396' v='745.801532' td='38397' odate='2005-02-16'/>
			<t d='3397' v='740.911400' td='38398' odate='2005-02-17'/>
			<t d='3398' v='740.803831' td='38399' odate='2005-02-18'/>
			<t d='3402' v='728.636250' td='38403' odate='2005-02-22'/>
			<t d='3403' v='731.757281' td='38404' odate='2005-02-23'/>
			<t d='3404' v='733.859531' td='38405' odate='2005-02-24'/>
			<t d='3405' v='738.535781' td='38406' odate='2005-02-25'/>
			<t d='3408' v='733.562731' td='38409' odate='2005-02-28'/>
			<t d='3409' v='730.538000' td='38410' odate='2005-03-01'/>
			<t d='3410' v='726.622469' td='38411' odate='2005-03-02'/>
			<t d='3411' v='727.993731' td='38412' odate='2005-03-03'/>
			<t d='3412' v='733.207831' td='38413' odate='2005-03-04'/>
			<t d='3415' v='733.542480' td='38416' odate='2005-03-07'/>
			<t d='3416' v='733.008125' td='38417' odate='2005-03-08'/>
			<t d='3417' v='726.689950' td='38418' odate='2005-03-09'/>
			<t d='3418' v='723.565000' td='38419' odate='2005-03-10'/>
			<t d='3419' v='723.844350' td='38420' odate='2005-03-11'/>
			<t d='3422' v='726.865290' td='38423' odate='2005-03-14'/>
			<t d='3423' v='726.926581' td='38424' odate='2005-03-15'/>
			<t d='3424' v='725.581159' td='38425' odate='2005-03-16'/>
			<t d='3425' v='729.623594' td='38426' odate='2005-03-17'/>
			<t d='3426' v='729.563340' td='38427' odate='2005-03-18'/>
			<t d='3429' v='726.962800' td='38430' odate='2005-03-21'/>
			<t d='3430' v='727.134269' td='38431' odate='2005-03-22'/>
			<t d='3431' v='726.364569' td='38432' odate='2005-03-23'/>
			<t d='3432' v='729.231210' td='38433' odate='2005-03-24'/>
			<t d='3436' v='735.694100' td='38437' odate='2005-03-28'/>
			<t d='3437' v='735.273468' td='38438' odate='2005-03-29'/>
			<t d='3438' v='739.900042' td='38439' odate='2005-03-30'/>
			<t d='3439' v='735.665100' td='38440' odate='2005-03-31'/>
			<t d='3440' v='734.722850' td='38441' odate='2005-04-01'/>
			<t d='3443' v='740.960608' td='38444' odate='2005-04-04'/>
			<t d='3444' v='742.188750' td='38445' odate='2005-04-05'/>
			<t d='3445' v='743.789168' td='38446' odate='2005-04-06'/>
			<t d='3446' v='743.317968' td='38447' odate='2005-04-07'/>
			<t d='3447' v='741.231082' td='38448' odate='2005-04-08'/>
			<t d='3450' v='741.579300' td='38451' odate='2005-04-11'/>
			<t d='3451' v='743.973950' td='38452' odate='2005-04-12'/>
			<t d='3452' v='734.778350' td='38453' odate='2005-04-13'/>
			<t d='3453' v='736.971750' td='38454' odate='2005-04-14'/>
			<t d='3454' v='730.592768' td='38455' odate='2005-04-15'/>
			<t d='3457' v='732.240382' td='38458' odate='2005-04-18'/>
			<t d='3458' v='735.381782' td='38459' odate='2005-04-19'/>
			<t d='3459' v='726.757750' td='38460' odate='2005-04-20'/>
			<t d='3460' v='735.875250' td='38461' odate='2005-04-21'/>
			<t d='3461' v='732.421500' td='38462' odate='2005-04-22'/>
			<t d='3464' v='732.065281' td='38465' odate='2005-04-25'/>
			<t d='3465' v='728.745781' td='38466' odate='2005-04-26'/>
			<t d='3466' v='728.383705' td='38467' odate='2005-04-27'/>
			<t d='3467' v='722.008620' td='38468' odate='2005-04-28'/>
			<t d='3468' v='721.109229' td='38469' odate='2005-04-29'/>
			<t d='3471' v='725.139231' td='38472' odate='2005-05-02'/>
			<t d='3472' v='723.472069' td='38473' odate='2005-05-03'/>
			<t d='3473' v='724.900750' td='38474' odate='2005-05-04'/>
			<t d='3474' v='725.912031' td='38475' odate='2005-05-05'/>
			<t d='3475' v='727.700600' td='38476' odate='2005-05-06'/>
			<t d='3478' v='731.289500' td='38479' odate='2005-05-09'/>
			<t d='3479' v='727.700600' td='38480' odate='2005-05-10'/>
			<t d='3480' v='728.832031' td='38481' odate='2005-05-11'/>
			<t d='3481' v='726.392031' td='38482' odate='2005-05-12'/>
			<t d='3482' v='722.402445' td='38483' odate='2005-05-13'/>
			<t d='3485' v='727.444020' td='38486' odate='2005-05-16'/>
			<t d='3486' v='732.445350' td='38487' odate='2005-05-17'/>
			<t d='3487' v='734.099900' td='38488' odate='2005-05-18'/>
			<t d='3488' v='735.705569' td='38489' odate='2005-05-19'/>
			<t d='3489' v='733.780740' td='38490' odate='2005-05-20'/>
			<t d='3492' v='738.214000' td='38493' odate='2005-05-23'/>
			<t d='3493' v='737.545769' td='38494' odate='2005-05-24'/>
			<t d='3494' v='738.057600' td='38495' odate='2005-05-25'/>
			<t d='3495' v='741.394000' td='38496' odate='2005-05-26'/>
			<t d='3496' v='742.960000' td='38497' odate='2005-05-27'/>
			<t d='3500' v='741.314909' td='38501' odate='2005-05-31'/>
			<t d='3501' v='747.447650' td='38502' odate='2005-06-01'/>
			<t d='3502' v='747.124981' td='38503' odate='2005-06-02'/>
			<t d='3503' v='742.677810' td='38504' odate='2005-06-03'/>
			<t d='3506' v='743.467219' td='38507' odate='2005-06-06'/>
			<t d='3507' v='741.653281' td='38508' odate='2005-06-07'/>
			<t d='3508' v='741.977775' td='38509' odate='2005-06-08'/>
			<t d='3509' v='746.072800' td='38510' odate='2005-06-09'/>
			<t d='3510' v='745.697010' td='38511' odate='2005-06-10'/>
			<t d='3513' v='749.004826' td='38514' odate='2005-06-13'/>
			<t d='3514' v='750.444800' td='38515' odate='2005-06-14'/>
			<t d='3515' v='752.250479' td='38516' odate='2005-06-15'/>
			<t d='3516' v='756.666900' td='38517' odate='2005-06-16'/>
			<t d='3517' v='756.378000' td='38518' odate='2005-06-17'/>
			<t d='3520' v='755.872219' td='38521' odate='2005-06-20'/>
			<t d='3521' v='754.471391' td='38522' odate='2005-06-21'/>
			<t d='3522' v='756.017000' td='38523' odate='2005-06-22'/>
			<t d='3523' v='752.134480' td='38524' odate='2005-06-23'/>
			<t d='3524' v='751.361609' td='38525' odate='2005-06-24'/>
			<t d='3527' v='753.013625' td='38528' odate='2005-06-27'/>
			<t d='3528' v='755.577231' td='38529' odate='2005-06-28'/>
			<t d='3529' v='759.176120' td='38530' odate='2005-06-29'/>
			<t d='3530' v='759.088155' td='38531' odate='2005-06-30'/>
			<t d='3531' v='762.537600' td='38532' odate='2005-07-01'/>
			<t d='3535' v='775.325218' td='38536' odate='2005-07-05'/>
			<t d='3536' v='771.049125' td='38537' odate='2005-07-06'/>
			<t d='3537' v='775.095645' td='38538' odate='2005-07-07'/>
			<t d='3538' v='778.303750' td='38539' odate='2005-07-08'/>
			<t d='3541' v='776.582918' td='38542' odate='2005-07-11'/>
			<t d='3542' v='775.575282' td='38543' odate='2005-07-12'/>
			<t d='3543' v='774.805468' td='38544' odate='2005-07-13'/>
			<t d='3544' v='774.379200' td='38545' odate='2005-07-14'/>
			<t d='3545' v='774.864400' td='38546' odate='2005-07-15'/>
			<t d='3548' v='770.832818' td='38549' odate='2005-07-18'/>
			<t d='3549' v='773.223468' td='38550' odate='2005-07-19'/>
			<t d='3550' v='775.169968' td='38551' odate='2005-07-20'/>
			<t d='3551' v='771.791200' td='38552' odate='2005-07-21'/>
			<t d='3552' v='766.980468' td='38553' odate='2005-07-22'/>
			<t d='3555' v='769.676032' td='38556' odate='2005-07-25'/>
			<t d='3556' v='774.847360' td='38557' odate='2005-07-26'/>
			<t d='3557' v='778.987082' td='38558' odate='2005-07-27'/>
			<t d='3558' v='781.816468' td='38559' odate='2005-07-28'/>
			<t d='3559' v='778.075478' td='38560' odate='2005-07-29'/>
			<t d='3562' v='779.683450' td='38563' odate='2005-08-01'/>
			<t d='3563' v='780.961642' td='38564' odate='2005-08-02'/>
			<t d='3564' v='781.856980' td='38565' odate='2005-08-03'/>
			<t d='3565' v='780.054032' td='38566' odate='2005-08-04'/>
			<t d='3566' v='775.749800' td='38567' odate='2005-08-05'/>
			<t d='3569' v='780.172050' td='38570' odate='2005-08-08'/>
			<t d='3570' v='779.736518' td='38571' odate='2005-08-09'/>
			<t d='3571' v='783.979728' td='38572' odate='2005-08-10'/>
			<t d='3572' v='788.310743' td='38573' odate='2005-08-11'/>
			<t d='3573' v='786.726300' td='38574' odate='2005-08-12'/>
			<t d='3576' v='788.256900' td='38577' odate='2005-08-15'/>
			<t d='3577' v='784.099410' td='38578' odate='2005-08-16'/>
			<t d='3578' v='782.880780' td='38579' odate='2005-08-17'/>
			<t d='3579' v='782.297072' td='38580' odate='2005-08-18'/>
			<t d='3580' v='786.723632' td='38581' odate='2005-08-19'/>
			<t d='3583' v='786.508595' td='38584' odate='2005-08-22'/>
			<t d='3584' v='786.826965' td='38585' odate='2005-08-23'/>
			<t d='3585' v='790.214400' td='38586' odate='2005-08-24'/>
			<t d='3586' v='791.427900' td='38587' odate='2005-08-25'/>
			<t d='3587' v='787.967698' td='38588' odate='2005-08-26'/>
			<t d='3590' v='796.158330' td='38591' odate='2005-08-29'/>
			<t d='3591' v='803.374500' td='38592' odate='2005-08-30'/>
			<t d='3592' v='814.819967' td='38593' odate='2005-08-31'/>
			<t d='3593' v='821.042500' td='38594' odate='2005-09-01'/>
			<t d='3594' v='810.297390' td='38595' odate='2005-09-02'/>
			<t d='3598' v='815.903393' td='38599' odate='2005-09-06'/>
			<t d='3599' v='818.347475' td='38600' odate='2005-09-07'/>
			<t d='3600' v='819.984913' td='38601' odate='2005-09-08'/>
			<t d='3601' v='824.569460' td='38602' odate='2005-09-09'/>
			<t d='3604' v='820.355850' td='38605' odate='2005-09-12'/>
			<t d='3605' v='819.557323' td='38606' odate='2005-09-13'/>
			<t d='3606' v='821.644180' td='38607' odate='2005-09-14'/>
			<t d='3607' v='821.988113' td='38608' odate='2005-09-15'/>
			<t d='3608' v='826.718700' td='38609' odate='2005-09-16'/>
			<t d='3611' v='832.439850' td='38612' odate='2005-09-19'/>
			<t d='3612' v='827.091353' td='38613' odate='2005-09-20'/>
			<t d='3613' v='826.944090' td='38614' odate='2005-09-21'/>
			<t d='3614' v='830.194542' td='38615' odate='2005-09-22'/>
			<t d='3615' v='831.058767' td='38616' odate='2005-09-23'/>
			<t d='3618' v='836.608887' td='38619' odate='2005-09-26'/>
			<t d='3619' v='836.156913' td='38620' odate='2005-09-27'/>
			<t d='3620' v='840.678060' td='38621' odate='2005-09-28'/>
			<t d='3621' v='842.702638' td='38622' odate='2005-09-29'/>
			<t d='3622' v='843.243555' td='38623' odate='2005-09-30'/>
			<t d='3625' v='846.622767' td='38626' odate='2005-10-03'/>
			<t d='3626' v='842.093808' td='38627' odate='2005-10-04'/>
			<t d='3627' v='829.890402' td='38628' odate='2005-10-05'/>
			<t d='3628' v='825.451150' td='38629' odate='2005-10-06'/>
			<t d='3629' v='828.448830' td='38630' odate='2005-10-07'/>
			<t d='3632' v='824.461077' td='38633' odate='2005-10-10'/>
			<t d='3633' v='825.870630' td='38634' odate='2005-10-11'/>
			<t d='3634' v='822.706075' td='38635' odate='2005-10-12'/>
			<t d='3635' v='820.100750' td='38636' odate='2005-10-13'/>
			<t d='3636' v='826.635952' td='38637' odate='2005-10-14'/>
			<t d='3639' v='828.841355' td='38640' odate='2005-10-17'/>
			<t d='3640' v='823.815554' td='38641' odate='2005-10-18'/>
			<t d='3641' v='834.677164' td='38642' odate='2005-10-19'/>
			<t d='3642' v='824.284151' td='38643' odate='2005-10-20'/>
			<t d='3643' v='824.082000' td='38644' odate='2005-10-21'/>
			<t d='3646' v='837.495966' td='38647' odate='2005-10-24'/>
			<t d='3647' v='838.888304' td='38648' odate='2005-10-25'/>
			<t d='3648' v='833.421600' td='38649' odate='2005-10-26'/>
			<t d='3649' v='825.863809' td='38650' odate='2005-10-27'/>
			<t d='3650' v='834.424565' td='38651' odate='2005-10-28'/>
			<t d='3653' v='836.438895' td='38654' odate='2005-10-31'/>
			<t d='3654' v='837.858459' td='38655' odate='2005-11-01'/>
			<t d='3655' v='843.705474' td='38656' odate='2005-11-02'/>
			<t d='3656' v='845.257046' td='38657' odate='2005-11-03'/>
			<t d='3657' v='843.419326' td='38658' odate='2005-11-04'/>
			<t d='3660' v='840.178754' td='38661' odate='2005-11-07'/>
			<t d='3661' v='839.166316' td='38662' odate='2005-11-08'/>
			<t d='3662' v='834.059584' td='38663' odate='2005-11-09'/>
			<t d='3663' v='834.412150' td='38664' odate='2005-11-10'/>
			<t d='3664' v='831.456900' td='38665' odate='2005-11-11'/>
			<t d='3667' v='827.466183' td='38668' odate='2005-11-14'/>
			<t d='3668' v='829.049770' td='38669' odate='2005-11-15'/>
			<t d='3669' v='834.360266' td='38670' odate='2005-11-16'/>
			<t d='3670' v='839.479646' td='38671' odate='2005-11-17'/>
			<t d='3671' v='838.495620' td='38672' odate='2005-11-18'/>
			<t d='3674' v='843.400350' td='38675' odate='2005-11-21'/>
			<t d='3675' v='849.716385' td='38676' odate='2005-11-22'/>
			<t d='3676' v='845.496250' td='38677' odate='2005-11-23'/>
			<t d='3678' v='848.194594' td='38679' odate='2005-11-25'/>
			<t d='3681' v='842.364535' td='38682' odate='2005-11-28'/>
			<t d='3682' v='841.934514' td='38683' odate='2005-11-29'/>
			<t d='3683' v='839.563241' td='38684' odate='2005-11-30'/>
			<t d='3684' v='849.143866' td='38685' odate='2005-12-01'/>
			<t d='3685' v='849.624860' td='38686' odate='2005-12-02'/>
			<t d='3688' v='848.725290' td='38689' odate='2005-12-05'/>
			<t d='3689' v='849.600147' td='38690' odate='2005-12-06'/>
			<t d='3690' v='848.175050' td='38691' odate='2005-12-07'/>
			<t d='3691' v='852.303634' td='38692' odate='2005-12-08'/>
			<t d='3692' v='852.765909' td='38693' odate='2005-12-09'/>
			<t d='3695' v='858.956800' td='38696' odate='2005-12-12'/>
			<t d='3696' v='861.529947' td='38697' odate='2005-12-13'/>
			<t d='3697' v='860.085283' td='38698' odate='2005-12-14'/>
			<t d='3698' v='837.219633' td='38699' odate='2005-12-15'/>
			<t d='3699' v='832.785738' td='38700' odate='2005-12-16'/>
			<t d='3702' v='831.027553' td='38703' odate='2005-12-19'/>
			<t d='3703' v='832.862367' td='38704' odate='2005-12-20'/>
			<t d='3704' v='833.727920' td='38705' odate='2005-12-21'/>
			<t d='3705' v='836.131227' td='38706' odate='2005-12-22'/>
			<t d='3706' v='834.695580' td='38707' odate='2005-12-23'/>
			<t d='3710' v='827.674830' td='38711' odate='2005-12-27'/>
			<t d='3711' v='829.573677' td='38712' odate='2005-12-28'/>
			<t d='3712' v='823.233060' td='38713' odate='2005-12-29'/>
			<t d='3713' v='820.418140' td='38714' odate='2005-12-30'/>
			<t d='3717' v='822.879577' td='38718' odate='2006-01-03'/>
			<t d='3718' v='818.785500' td='38719' odate='2006-01-04'/>
			<t d='3719' v='806.959463' td='38720' odate='2006-01-05'/>
			<t d='3720' v='816.477912' td='38721' odate='2006-01-06'/>
			<t d='3723' v='811.197773' td='38724' odate='2006-01-09'/>
			<t d='3724' v='812.039912' td='38725' odate='2006-01-10'/>
			<t d='3725' v='813.012525' td='38726' odate='2006-01-11'/>
			<t d='3726' v='807.150960' td='38727' odate='2006-01-12'/>
			<t d='3727' v='810.651940' td='38728' odate='2006-01-13'/>
			<t d='3731' v='810.484010' td='38732' odate='2006-01-17'/>
			<t d='3732' v='810.771900' td='38733' odate='2006-01-18'/>
			<t d='3733' v='814.609575' td='38734' odate='2006-01-19'/>
			<t d='3734' v='810.569980' td='38735' odate='2006-01-20'/>
			<t d='3737' v='808.235680' td='38738' odate='2006-01-23'/>
			<t d='3738' v='807.495000' td='38739' odate='2006-01-24'/>
			<t d='3739' v='803.700628' td='38740' odate='2006-01-25'/>
			<t d='3740' v='806.064508' td='38741' odate='2006-01-26'/>
			<t d='3741' v='807.840060' td='38742' odate='2006-01-27'/>
			<t d='3744' v='809.119268' td='38745' odate='2006-01-30'/>
			<t d='3745' v='808.479712' td='38746' odate='2006-01-31'/>
			<t d='3746' v='806.880630' td='38747' odate='2006-02-01'/>
			<t d='3747' v='802.083480' td='38748' odate='2006-02-02'/>
			<t d='3748' v='803.250277' td='38749' odate='2006-02-03'/>
			<t d='3751' v='803.483552' td='38752' odate='2006-02-06'/>
			<t d='3752' v='801.795288' td='38753' odate='2006-02-07'/>
			<t d='3753' v='806.012480' td='38754' odate='2006-02-08'/>
			<t d='3754' v='804.627170' td='38755' odate='2006-02-09'/>
			<t d='3755' v='804.627170' td='38756' odate='2006-02-10'/>
			<t d='3758' v='801.182250' td='38759' odate='2006-02-13'/>
			<t d='3759' v='801.934510' td='38760' odate='2006-02-14'/>
			<t d='3760' v='805.241248' td='38761' odate='2006-02-15'/>
			<t d='3761' v='811.981558' td='38762' odate='2006-02-16'/>
			<t d='3762' v='812.165640' td='38763' odate='2006-02-17'/>
			<t d='3766' v='814.623480' td='38767' odate='2006-02-21'/>
			<t d='3767' v='819.916833' td='38768' odate='2006-02-22'/>
			<t d='3768' v='816.354410' td='38769' odate='2006-02-23'/>
			<t d='3769' v='816.373148' td='38770' odate='2006-02-24'/>
			<t d='3772' v='817.752632' td='38773' odate='2006-02-27'/>
			<t d='3773' v='814.183045' td='38774' odate='2006-02-28'/>
			<t d='3774' v='812.982092' td='38775' odate='2006-03-01'/>
			<t d='3775' v='816.322080' td='38776' odate='2006-03-02'/>
			<t d='3776' v='819.593468' td='38777' odate='2006-03-03'/>
			<t d='3779' v='814.896600' td='38780' odate='2006-03-06'/>
			<t d='3780' v='812.987500' td='38781' odate='2006-03-07'/>
			<t d='3781' v='815.076467' td='38782' odate='2006-03-08'/>
			<t d='3782' v='812.962500' td='38783' odate='2006-03-09'/>
			<t d='3783' v='815.038235' td='38784' odate='2006-03-10'/>
			<t d='3786' v='817.637100' td='38787' odate='2006-03-13'/>
			<t d='3787' v='822.450192' td='38788' odate='2006-03-14'/>
			<t d='3788' v='823.442770' td='38789' odate='2006-03-15'/>
			<t d='3789' v='827.220000' td='38790' odate='2006-03-16'/>
			<t d='3790' v='826.812000' td='38791' odate='2006-03-17'/>
			<t d='3793' v='823.417200' td='38794' odate='2006-03-20'/>
			<t d='3794' v='822.473728' td='38795' odate='2006-03-21'/>
			<t d='3795' v='826.795400' td='38796' odate='2006-03-22'/>
			<t d='3796' v='826.816917' td='38797' odate='2006-03-23'/>
			<t d='3797' v='831.349733' td='38798' odate='2006-03-24'/>
			<t d='3800' v='828.881083' td='38801' odate='2006-03-27'/>
			<t d='3801' v='827.570100' td='38802' odate='2006-03-28'/>
			<t d='3802' v='832.412483' td='38803' odate='2006-03-29'/>
			<t d='3803' v='831.792100' td='38804' odate='2006-03-30'/>
			<t d='3804' v='826.927920' td='38805' odate='2006-03-31'/>
			<t d='3807' v='831.300000' td='38808' odate='2006-04-03'/>
			<t d='3808' v='830.720000' td='38809' odate='2006-04-04'/>
			<t d='3809' v='832.270143' td='38810' odate='2006-04-05'/>
			<t d='3810' v='827.526032' td='38811' odate='2006-04-06'/>
			<t d='3811' v='823.307532' td='38812' odate='2006-04-07'/>
			<t d='3814' v='826.149000' td='38815' odate='2006-04-10'/>
			<t d='3815' v='825.845988' td='38816' odate='2006-04-11'/>
			<t d='3816' v='827.436033' td='38817' odate='2006-04-12'/>
			<t d='3817' v='828.072033' td='38818' odate='2006-04-13'/>
			<t d='3821' v='828.455760' td='38822' odate='2006-04-17'/>
			<t d='3822' v='838.373293' td='38823' odate='2006-04-18'/>
			<t d='3823' v='838.874803' td='38824' odate='2006-04-19'/>
			<t d='3824' v='833.443168' td='38825' odate='2006-04-20'/>
			<t d='3825' v='835.407332' td='38826' odate='2006-04-21'/>
			<t d='3828' v='825.904340' td='38829' odate='2006-04-24'/>
			<t d='3829' v='823.639500' td='38830' odate='2006-04-25'/>
			<t d='3830' v='825.313250' td='38831' odate='2006-04-26'/>
			<t d='3831' v='822.169040' td='38832' odate='2006-04-27'/>
			<t d='3832' v='823.174768' td='38833' odate='2006-04-28'/>
			<t d='3835' v='824.144895' td='38836' odate='2006-05-01'/>
			<t d='3836' v='827.207402' td='38837' odate='2006-05-02'/>
			<t d='3837' v='820.855168' td='38838' odate='2006-05-03'/>
			<t d='3838' v='823.389190' td='38839' odate='2006-05-04'/>
			<t d='3839' v='828.498068' td='38840' odate='2006-05-05'/>
			<t d='3842' v='823.515000' td='38843' odate='2006-05-08'/>
			<t d='3843' v='828.173819' td='38844' odate='2006-05-09'/>
			<t d='3844' v='823.978969' td='38845' odate='2006-05-10'/>
			<t d='3845' v='822.862800' td='38846' odate='2006-05-11'/>
			<t d='3846' v='813.628800' td='38847' odate='2006-05-12'/>
			<t d='3849' v='818.210432' td='38850' odate='2006-05-15'/>
			<t d='3850' v='819.374132' td='38851' odate='2006-05-16'/>
			<t d='3851' v='809.642250' td='38852' odate='2006-05-17'/>
			<t d='3852' v='813.130532' td='38853' odate='2006-05-18'/>
			<t d='3853' v='816.463048' td='38854' odate='2006-05-19'/>
			<t d='3856' v='821.523928' td='38857' odate='2006-05-22'/>
			<t d='3857' v='813.701568' td='38858' odate='2006-05-23'/>
			<t d='3858' v='814.809078' td='38859' odate='2006-05-24'/>
			<t d='3859' v='823.069608' td='38860' odate='2006-05-25'/>
			<t d='3860' v='824.477940' td='38861' odate='2006-05-26'/>
			<t d='3864' v='822.656000' td='38865' odate='2006-05-30'/>
			<t d='3865' v='825.862332' td='38866' odate='2006-05-31'/>
			<t d='3866' v='833.308500' td='38867' odate='2006-06-01'/>
			<t d='3867' v='834.669560' td='38868' odate='2006-06-02'/>
			<t d='3870' v='825.479200' td='38871' odate='2006-06-05'/>
			<t d='3871' v='823.467292' td='38872' odate='2006-06-06'/>
			<t d='3872' v='824.279490' td='38873' odate='2006-06-07'/>
			<t d='3873' v='831.854130' td='38874' odate='2006-06-08'/>
			<t d='3874' v='828.193232' td='38875' odate='2006-06-09'/>
			<t d='3877' v='824.276195' td='38878' odate='2006-06-12'/>
			<t d='3878' v='819.703250' td='38879' odate='2006-06-13'/>
			<t d='3879' v='822.884875' td='38880' odate='2006-06-14'/>
			<t d='3880' v='831.843240' td='38881' odate='2006-06-15'/>
			<t d='3881' v='827.908500' td='38882' odate='2006-06-16'/>
			<t d='3884' v='824.425767' td='38885' odate='2006-06-19'/>
			<t d='3885' v='824.302293' td='38886' odate='2006-06-20'/>
			<t d='3886' v='830.607007' td='38887' odate='2006-06-21'/>
			<t d='3887' v='827.021580' td='38888' odate='2006-06-22'/>
			<t d='3888' v='830.707000' td='38889' odate='2006-06-23'/>
			<t d='3891' v='833.051167' td='38892' odate='2006-06-26'/>
			<t d='3892' v='830.064680' td='38893' odate='2006-06-27'/>
			<t d='3893' v='832.651500' td='38894' odate='2006-06-28'/>
			<t d='3894' v='841.680360' td='38895' odate='2006-06-29'/>
			<t d='3895' v='837.916728' td='38896' odate='2006-06-30'/>
			<t d='3898' v='837.912557' td='38899' odate='2006-07-03'/>
			<t d='3900' v='836.582040' td='38901' odate='2006-07-05'/>
			<t d='3901' v='839.157032' td='38902' odate='2006-07-06'/>
			<t d='3902' v='836.464720' td='38903' odate='2006-07-07'/>
			<t d='3905' v='835.342200' td='38906' odate='2006-07-10'/>
			<t d='3906' v='841.506290' td='38907' odate='2006-07-11'/>
			<t d='3907' v='839.246100' td='38908' odate='2006-07-12'/>
			<t d='3908' v='837.177533' td='38909' odate='2006-07-13'/>
			<t d='3909' v='837.056673' td='38910' odate='2006-07-14'/>
			<t d='3912' v='840.482060' td='38913' odate='2006-07-17'/>
			<t d='3913' v='839.585742' td='38914' odate='2006-07-18'/>
			<t d='3914' v='848.729667' td='38915' odate='2006-07-19'/>
			<t d='3915' v='840.840240' td='38916' odate='2006-07-20'/>
			<t d='3916' v='837.391930' td='38917' odate='2006-07-21'/>
			<t d='3919' v='847.677797' td='38920' odate='2006-07-24'/>
			<t d='3920' v='852.186110' td='38921' odate='2006-07-25'/>
			<t d='3921' v='856.467200' td='38922' odate='2006-07-26'/>
			<t d='3922' v='852.430473' td='38923' odate='2006-07-27'/>
			<t d='3923' v='860.015967' td='38924' odate='2006-07-28'/>
			<t d='3926' v='858.652527' td='38927' odate='2006-07-31'/>
			<t d='3927' v='859.162150' td='38928' odate='2006-08-01'/>
			<t d='3928' v='862.312500' td='38929' odate='2006-08-02'/>
			<t d='3929' v='862.982380' td='38930' odate='2006-08-03'/>
			<t d='3930' v='862.745625' td='38931' odate='2006-08-04'/>
			<t d='3933' v='861.524350' td='38934' odate='2006-08-07'/>
			<t d='3934' v='860.081117' td='38935' odate='2006-08-08'/>
			<t d='3935' v='855.572513' td='38936' odate='2006-08-09'/>
			<t d='3936' v='852.332040' td='38937' odate='2006-08-10'/>
			<t d='3937' v='852.243488' td='38938' odate='2006-08-11'/>
			<t d='3940' v='853.349310' td='38941' odate='2006-08-14'/>
			<t d='3941' v='860.538633' td='38942' odate='2006-08-15'/>
			<t d='3942' v='863.216867' td='38943' odate='2006-08-16'/>
			<t d='3943' v='859.537000' td='38944' odate='2006-08-17'/>
			<t d='3944' v='860.591000' td='38945' odate='2006-08-18'/>
			<t d='3947' v='862.283027' td='38948' odate='2006-08-21'/>
			<t d='3948' v='865.545620' td='38949' odate='2006-08-22'/>
			<t d='3949' v='863.306670' td='38950' odate='2006-08-23'/>
			<t d='3950' v='866.582967' td='38951' odate='2006-08-24'/>
			<t d='3951' v='868.570710' td='38952' odate='2006-08-25'/>
			<t d='3954' v='872.974300' td='38955' odate='2006-08-28'/>
			<t d='3955' v='872.067663' td='38956' odate='2006-08-29'/>
			<t d='3956' v='871.982500' td='38957' odate='2006-08-30'/>
			<t d='3957' v='872.709200' td='38958' odate='2006-08-31'/>
			<t d='3958' v='875.079293' td='38959' odate='2006-09-01'/>
			<t d='3962' v='872.459967' td='38963' odate='2006-09-05'/>
			<t d='3963' v='868.872000' td='38964' odate='2006-09-06'/>
			<t d='3964' v='870.013640' td='38965' odate='2006-09-07'/>
			<t d='3965' v='869.282700' td='38966' odate='2006-09-08'/>
			<t d='3968' v='866.125167' td='38969' odate='2006-09-11'/>
			<t d='3969' v='868.494867' td='38970' odate='2006-09-12'/>
			<t d='3970' v='871.063167' td='38971' odate='2006-09-13'/>
			<t d='3971' v='867.836733' td='38972' odate='2006-09-14'/>
			<t d='3972' v='867.507900' td='38973' odate='2006-09-15'/>
			<t d='3975' v='868.166633' td='38976' odate='2006-09-18'/>
			<t d='3976' v='865.598967' td='38977' odate='2006-09-19'/>
			<t d='3977' v='866.783600' td='38978' odate='2006-09-20'/>
			<t d='3978' v='867.007440' td='38979' odate='2006-09-21'/>
			<t d='3979' v='864.675000' td='38980' odate='2006-09-22'/>
			<t d='3982' v='868.298433' td='38983' odate='2006-09-25'/>
			<t d='3983' v='872.448150' td='38984' odate='2006-09-26'/>
			<t d='3984' v='873.557520' td='38985' odate='2006-09-27'/>
			<t d='3985' v='875.554633' td='38986' odate='2006-09-28'/>
			<t d='3986' v='876.752775' td='38987' odate='2006-09-29'/>
			<t d='3989' v='875.759533' td='38990' odate='2006-10-02'/>
			<t d='3990' v='870.484320' td='38991' odate='2006-10-03'/>
			<t d='3991' v='878.249580' td='38992' odate='2006-10-04'/>
			<t d='3992' v='881.517567' td='38993' odate='2006-10-05'/>
			<t d='3993' v='878.512917' td='38994' odate='2006-10-06'/>
			<t d='3996' v='877.546800' td='38997' odate='2006-10-09'/>
			<t d='3997' v='877.255080' td='38998' odate='2006-10-10'/>
			<t d='3998' v='881.133327' td='38999' odate='2006-10-11'/>
			<t d='3999' v='885.712750' td='39000' odate='2006-10-12'/>
			<t d='4000' v='883.283730' td='39001' odate='2006-10-13'/>
			<t d='4003' v='890.896267' td='39004' odate='2006-10-16'/>
			<t d='4004' v='885.356337' td='39005' odate='2006-10-17'/>
			<t d='4005' v='885.997377' td='39006' odate='2006-10-18'/>
			<t d='4006' v='886.916927' td='39007' odate='2006-10-19'/>
			<t d='4007' v='886.108033' td='39008' odate='2006-10-20'/>
			<t d='4010' v='890.176767' td='39011' odate='2006-10-23'/>
			<t d='4011' v='891.441000' td='39012' odate='2006-10-24'/>
			<t d='4012' v='893.503600' td='39013' odate='2006-10-25'/>
			<t d='4013' v='895.493800' td='39014' odate='2006-10-26'/>
			<t d='4014' v='893.433750' td='39015' odate='2006-10-27'/>
			<t d='4017' v='890.429967' td='39018' odate='2006-10-30'/>
			<t d='4018' v='893.363633' td='39019' odate='2006-10-31'/>
			<t d='4019' v='887.763000' td='39020' odate='2006-11-01'/>
			<t d='4020' v='884.172945' td='39021' odate='2006-11-02'/>
			<t d='4021' v='879.488767' td='39022' odate='2006-11-03'/>
			<t d='4024' v='886.549950' td='39025' odate='2006-11-06'/>
			<t d='4025' v='885.321720' td='39026' odate='2006-11-07'/>
			<t d='4026' v='888.205833' td='39027' odate='2006-11-08'/>
			<t d='4027' v='886.801500' td='39028' odate='2006-11-09'/>
			<t d='4028' v='885.958780' td='39029' odate='2006-11-10'/>
			<t d='4031' v='883.382700' td='39032' odate='2006-11-13'/>
			<t d='4032' v='886.485567' td='39033' odate='2006-11-14'/>
			<t d='4033' v='891.789390' td='39034' odate='2006-11-15'/>
			<t d='4034' v='891.619575' td='39035' odate='2006-11-16'/>
			<t d='4035' v='894.708100' td='39036' odate='2006-11-17'/>
			<t d='4038' v='892.732333' td='39039' odate='2006-11-20'/>
			<t d='4039' v='894.585267' td='39040' odate='2006-11-21'/>
			<t d='4040' v='893.473000' td='39041' odate='2006-11-22'/>
			<t d='4042' v='894.291560' td='39043' odate='2006-11-24'/>
			<t d='4045' v='884.902557' td='39046' odate='2006-11-27'/>
			<t d='4046' v='888.080033' td='39047' odate='2006-11-28'/>
			<t d='4047' v='889.815000' td='39048' odate='2006-11-29'/>
			<t d='4048' v='888.408000' td='39049' odate='2006-11-30'/>
			<t d='4049' v='888.111288' td='39050' odate='2006-12-01'/>
			<t d='4052' v='893.750460' td='39053' odate='2006-12-04'/>
			<t d='4053' v='893.925600' td='39054' odate='2006-12-05'/>
			<t d='4054' v='894.710032' td='39055' odate='2006-12-06'/>
			<t d='4055' v='893.842560' td='39056' odate='2006-12-07'/>
			<t d='4056' v='895.445250' td='39057' odate='2006-12-08'/>
			<t d='4059' v='898.914347' td='39060' odate='2006-12-11'/>
			<t d='4060' v='899.925032' td='39061' odate='2006-12-12'/>
			<t d='4061' v='899.634710' td='39062' odate='2006-12-13'/>
			<t d='4062' v='845.142390' td='39063' odate='2006-12-14'/>
			<t d='4063' v='844.606433' td='39064' odate='2006-12-15'/>
			<t d='4066' v='844.732800' td='39067' odate='2006-12-18'/>
			<t d='4067' v='852.955117' td='39068' odate='2006-12-19'/>
			<t d='4068' v='848.942453' td='39069' odate='2006-12-20'/>
			<t d='4069' v='845.974500' td='39070' odate='2006-12-21'/>
			<t d='4070' v='843.500050' td='39071' odate='2006-12-22'/>
			<t d='4074' v='847.241583' td='39075' odate='2006-12-26'/>
			<t d='4075' v='850.396950' td='39076' odate='2006-12-27'/>
			<t d='4076' v='849.030000' td='39077' odate='2006-12-28'/>
			<t d='4077' v='843.299767' td='39078' odate='2006-12-29'/>
			<t d='4082' v='835.724982' td='39083' odate='2007-01-03'/>
			<t d='4083' v='838.829673' td='39084' odate='2007-01-04'/>
			<t d='4084' v='838.669350' td='39085' odate='2007-01-05'/>
			<t d='4087' v='842.369967' td='39088' odate='2007-01-08'/>
			<t d='4088' v='842.696467' td='39089' odate='2007-01-09'/>
			<t d='4089' v='844.562967' td='39090' odate='2007-01-10'/>
			<t d='4090' v='847.164400' td='39091' odate='2007-01-11'/>
			<t d='4091' v='850.647317' td='39092' odate='2007-01-12'/>
			<t d='4095' v='850.381533' td='39096' odate='2007-01-16'/>
			<t d='4096' v='852.134130' td='39097' odate='2007-01-17'/>
			<t d='4097' v='850.913508' td='39098' odate='2007-01-18'/>
			<t d='4098' v='854.437533' td='39099' odate='2007-01-19'/>
			<t d='4101' v='854.745947' td='39102' odate='2007-01-22'/>
			<t d='4102' v='860.549553' td='39103' odate='2007-01-23'/>
			<t d='4103' v='858.152640' td='39104' odate='2007-01-24'/>
			<t d='4104' v='853.667540' td='39105' odate='2007-01-25'/>
			<t d='4105' v='856.665360' td='39106' odate='2007-01-26'/>
			<t d='4108' v='856.942280' td='39109' odate='2007-01-29'/>
			<t d='4109' v='860.530050' td='39110' odate='2007-01-30'/>
			<t d='4110' v='863.345907' td='39111' odate='2007-01-31'/>
			<t d='4111' v='863.429433' td='39112' odate='2007-02-01'/>
			<t d='4112' v='866.322800' td='39113' odate='2007-02-02'/>
			<t d='4115' v='866.928420' td='39116' odate='2007-02-05'/>
			<t d='4116' v='871.751127' td='39117' odate='2007-02-06'/>
			<t d='4117' v='872.084280' td='39118' odate='2007-02-07'/>
			<t d='4118' v='873.721727' td='39119' odate='2007-02-08'/>
			<t d='4119' v='872.359707' td='39120' odate='2007-02-09'/>
			<t d='4122' v='870.473133' td='39123' odate='2007-02-12'/>
			<t d='4123' v='878.353617' td='39124' odate='2007-02-13'/>
			<t d='4124' v='879.709950' td='39125' odate='2007-02-14'/>
			<t d='4125' v='879.564015' td='39126' odate='2007-02-15'/>
			<t d='4126' v='882.152600' td='39127' odate='2007-02-16'/>
			<t d='4130' v='884.428355' td='39131' odate='2007-02-20'/>
			<t d='4131' v='882.018900' td='39132' odate='2007-02-21'/>
			<t d='4132' v='882.645150' td='39133' odate='2007-02-22'/>
			<t d='4133' v='882.974988' td='39134' odate='2007-02-23'/>
			<t d='4136' v='882.673825' td='39137' odate='2007-02-26'/>
			<t d='4137' v='867.929340' td='39138' odate='2007-02-27'/>
			<t d='4138' v='867.573500' td='39139' odate='2007-02-28'/>
			<t d='4139' v='866.980875' td='39140' odate='2007-03-01'/>
			<t d='4140' v='861.520083' td='39141' odate='2007-03-02'/>
			<t d='4143' v='856.065033' td='39144' odate='2007-03-05'/>
			<t d='4144' v='865.054080' td='39145' odate='2007-03-06'/>
			<t d='4145' v='867.572217' td='39146' odate='2007-03-07'/>
			<t d='4146' v='868.139387' td='39147' odate='2007-03-08'/>
			<t d='4147' v='868.496000' td='39148' odate='2007-03-09'/>
			<t d='4150' v='871.939200' td='39151' odate='2007-03-12'/>
			<t d='4151' v='863.337860' td='39152' odate='2007-03-13'/>
			<t d='4152' v='864.772707' td='39153' odate='2007-03-14'/>
			<t d='4153' v='867.671830' td='39154' odate='2007-03-15'/>
			<t d='4154' v='867.960627' td='39155' odate='2007-03-16'/>
			<t d='4157' v='873.588137' td='39158' odate='2007-03-19'/>
			<t d='4158' v='878.210000' td='39159' odate='2007-03-20'/>
			<t d='4159' v='884.752607' td='39160' odate='2007-03-21'/>
			<t d='4160' v='881.506127' td='39161' odate='2007-03-22'/>
			<t d='4161' v='883.556450' td='39162' odate='2007-03-23'/>
			<t d='4164' v='884.976045' td='39165' odate='2007-03-26'/>
			<t d='4165' v='883.553412' td='39166' odate='2007-03-27'/>
			<t d='4166' v='880.456533' td='39167' odate='2007-03-28'/>
			<t d='4167' v='882.371433' td='39168' odate='2007-03-29'/>
			<t d='4168' v='883.887287' td='39169' odate='2007-03-30'/>
			<t d='4171' v='885.819300' td='39172' odate='2007-04-02'/>
			<t d='4172' v='889.961303' td='39173' odate='2007-04-03'/>
			<t d='4173' v='890.451900' td='39174' odate='2007-04-04'/>
			<t d='4174' v='891.931127' td='39175' odate='2007-04-05'/>
			<t d='4178' v='890.615495' td='39179' odate='2007-04-09'/>
			<t d='4179' v='894.834753' td='39180' odate='2007-04-10'/>
			<t d='4180' v='892.943570' td='39181' odate='2007-04-11'/>
			<t d='4181' v='900.578433' td='39182' odate='2007-04-12'/>
			<t d='4182' v='901.421920' td='39183' odate='2007-04-13'/>
			<t d='4185' v='906.994375' td='39186' odate='2007-04-16'/>
			<t d='4186' v='907.049075' td='39187' odate='2007-04-17'/>
			<t d='4187' v='908.002907' td='39188' odate='2007-04-18'/>
			<t d='4188' v='904.998747' td='39189' odate='2007-04-19'/>
			<t d='4189' v='911.128560' td='39190' odate='2007-04-20'/>
			<t d='4192' v='915.492000' td='39193' odate='2007-04-23'/>
			<t d='4193' v='918.035400' td='39194' odate='2007-04-24'/>
			<t d='4194' v='921.520960' td='39195' odate='2007-04-25'/>
			<t d='4195' v='919.197400' td='39196' odate='2007-04-26'/>
			<t d='4196' v='920.580900' td='39197' odate='2007-04-27'/>
			<t d='4199' v='917.175748' td='39200' odate='2007-04-30'/>
			<t d='4200' v='918.855280' td='39201' odate='2007-05-01'/>
			<t d='4201' v='924.213750' td='39202' odate='2007-05-02'/>
			<t d='4202' v='925.336867' td='39203' odate='2007-05-03'/>
			<t d='4203' v='926.612640' td='39204' odate='2007-05-04'/>
			<t d='4206' v='925.648533' td='39207' odate='2007-05-07'/>
			<t d='4207' v='925.209740' td='39208' odate='2007-05-08'/>
			<t d='4208' v='926.459520' td='39209' odate='2007-05-09'/>
			<t d='4209' v='921.812215' td='39210' odate='2007-05-10'/>
			<t d='4210' v='925.212933' td='39211' odate='2007-05-11'/>
			<t d='4213' v='925.199205' td='39214' odate='2007-05-14'/>
			<t d='4214' v='926.254683' td='39215' odate='2007-05-15'/>
			<t d='4215' v='929.882583' td='39216' odate='2007-05-16'/>
			<t d='4216' v='930.368532' td='39217' odate='2007-05-17'/>
			<t d='4217' v='935.947193' td='39218' odate='2007-05-18'/>
			<t d='4220' v='936.766075' td='39221' odate='2007-05-21'/>
			<t d='4221' v='934.183982' td='39222' odate='2007-05-22'/>
			<t d='4222' v='935.802000' td='39223' odate='2007-05-23'/>
			<t d='4223' v='925.143483' td='39224' odate='2007-05-24'/>
			<t d='4224' v='927.372407' td='39225' odate='2007-05-25'/>
			<t d='4228' v='921.095107' td='39229' odate='2007-05-29'/>
			<t d='4229' v='923.482000' td='39230' odate='2007-05-30'/>
			<t d='4230' v='926.729892' td='39231' odate='2007-05-31'/>
			<t d='4231' v='930.813580' td='39232' odate='2007-06-01'/>
			<t d='4234' v='934.833867' td='39235' odate='2007-06-04'/>
			<t d='4235' v='930.046635' td='39236' odate='2007-06-05'/>
			<t d='4236' v='925.346490' td='39237' odate='2007-06-06'/>
			<t d='4237' v='917.107120' td='39238' odate='2007-06-07'/>
			<t d='4238' v='920.937150' td='39239' odate='2007-06-08'/>
			<t d='4241' v='922.039125' td='39242' odate='2007-06-11'/>
			<t d='4242' v='917.709203' td='39243' odate='2007-06-12'/>
			<t d='4243' v='922.890990' td='39244' odate='2007-06-13'/>
			<t d='4244' v='928.090317' td='39245' odate='2007-06-14'/>
			<t d='4245' v='932.728407' td='39246' odate='2007-06-15'/>
			<t d='4248' v='932.549283' td='39249' odate='2007-06-18'/>
			<t d='4249' v='932.700267' td='39250' odate='2007-06-19'/>
			<t d='4250' v='923.174800' td='39251' odate='2007-06-20'/>
			<t d='4251' v='921.451350' td='39252' odate='2007-06-21'/>
			<t d='4252' v='911.285830' td='39253' odate='2007-06-22'/>
			<t d='4255' v='909.067540' td='39256' odate='2007-06-25'/>
			<t d='4256' v='905.632213' td='39257' odate='2007-06-26'/>
			<t d='4257' v='910.260360' td='39258' odate='2007-06-27'/>
			<t d='4258' v='911.310000' td='39259' odate='2007-06-28'/>
			<t d='4259' v='913.346173' td='39260' odate='2007-06-29'/>
			<t d='4262' v='916.590852' td='39263' odate='2007-07-02'/>
			<t d='4263' v='918.540000' td='39264' odate='2007-07-03'/>
			<t d='4265' v='918.573945' td='39266' odate='2007-07-05'/>
			<t d='4266' v='922.443738' td='39267' odate='2007-07-06'/>
			<t d='4269' v='922.856253' td='39270' odate='2007-07-09'/>
			<t d='4270' v='915.216643' td='39271' odate='2007-07-10'/>
			<t d='4271' v='917.262720' td='39272' odate='2007-07-11'/>
			<t d='4272' v='924.219460' td='39273' odate='2007-07-12'/>
			<t d='4273' v='924.984180' td='39274' odate='2007-07-13'/>
			<t d='4276' v='923.629650' td='39277' odate='2007-07-16'/>
			<t d='4277' v='924.108200' td='39278' odate='2007-07-17'/>
			<t d='4278' v='927.730000' td='39279' odate='2007-07-18'/>
			<t d='4279' v='930.687975' td='39280' odate='2007-07-19'/>
			<t d='4280' v='924.804933' td='39281' odate='2007-07-20'/>
			<t d='4283' v='925.185633' td='39284' odate='2007-07-23'/>
			<t d='4284' v='914.024992' td='39285' odate='2007-07-24'/>
			<t d='4285' v='915.001142' td='39286' odate='2007-07-25'/>
			<t d='4286' v='904.468713' td='39287' odate='2007-07-26'/>
			<t d='4287' v='899.665233' td='39288' odate='2007-07-27'/>
			<t d='4290' v='902.496700' td='39291' odate='2007-07-30'/>
			<t d='4291' v='898.310387' td='39292' odate='2007-07-31'/>
			<t d='4292' v='903.871200' td='39293' odate='2007-08-01'/>
			<t d='4293' v='907.841813' td='39294' odate='2007-08-02'/>
			<t d='4294' v='900.418827' td='39295' odate='2007-08-03'/>
			<t d='4297' v='903.968340' td='39298' odate='2007-08-06'/>
			<t d='4298' v='904.133220' td='39299' odate='2007-08-07'/>
			<t d='4299' v='908.656603' td='39300' odate='2007-08-08'/>
			<t d='4300' v='897.143753' td='39301' odate='2007-08-09'/>
			<t d='4301' v='901.702150' td='39302' odate='2007-08-10'/>
			<t d='4304' v='902.800707' td='39305' odate='2007-08-13'/>
			<t d='4305' v='894.173553' td='39306' odate='2007-08-14'/>
			<t d='4306' v='891.459000' td='39307' odate='2007-08-15'/>
			<t d='4307' v='892.907433' td='39308' odate='2007-08-16'/>
			<t d='4308' v='900.271875' td='39309' odate='2007-08-17'/>
			<t d='4311' v='896.388750' td='39312' odate='2007-08-20'/>
			<t d='4312' v='896.259297' td='39313' odate='2007-08-21'/>
			<t d='4313' v='900.357920' td='39314' odate='2007-08-22'/>
			<t d='4314' v='902.473077' td='39315' odate='2007-08-23'/>
			<t d='4315' v='911.375517' td='39316' odate='2007-08-24'/>
			<t d='4318' v='908.407443' td='39319' odate='2007-08-27'/>
			<t d='4319' v='897.980833' td='39320' odate='2007-08-28'/>
			<t d='4320' v='909.205000' td='39321' odate='2007-08-29'/>
			<t d='4321' v='907.758917' td='39322' odate='2007-08-30'/>
			<t d='4322' v='911.432413' td='39323' odate='2007-08-31'/>
			<t d='4326' v='917.917628' td='39327' odate='2007-09-04'/>
			<t d='4327' v='915.304983' td='39328' odate='2007-09-05'/>
			<t d='4328' v='918.786747' td='39329' odate='2007-09-06'/>
			<t d='4329' v='914.259810' td='39330' odate='2007-09-07'/>
			<t d='4332' v='914.233425' td='39333' odate='2007-09-10'/>
			<t d='4333' v='920.738400' td='39334' odate='2007-09-11'/>
			<t d='4334' v='922.350157' td='39335' odate='2007-09-12'/>
			<t d='4335' v='926.030000' td='39336' odate='2007-09-13'/>
			<t d='4336' v='928.042377' td='39337' odate='2007-09-14'/>
			<t d='4339' v='926.972820' td='39340' odate='2007-09-17'/>
			<t d='4340' v='942.192908' td='39341' odate='2007-09-18'/>
			<t d='4341' v='944.171853' td='39342' odate='2007-09-19'/>
			<t d='4342' v='938.469807' td='39343' odate='2007-09-20'/>
			<t d='4343' v='937.598233' td='39344' odate='2007-09-21'/>
			<t d='4346' v='932.436000' td='39347' odate='2007-09-24'/>
			<t d='4347' v='936.251550' td='39348' odate='2007-09-25'/>
			<t d='4348' v='937.474362' td='39349' odate='2007-09-26'/>
			<t d='4349' v='940.565175' td='39350' odate='2007-09-27'/>
			<t d='4350' v='935.645450' td='39351' odate='2007-09-28'/>
			<t d='4353' v='937.153980' td='39354' odate='2007-10-01'/>
			<t d='4354' v='937.891863' td='39355' odate='2007-10-02'/>
			<t d='4355' v='934.748923' td='39356' odate='2007-10-03'/>
			<t d='4356' v='936.669825' td='39357' odate='2007-10-04'/>
			<t d='4357' v='940.821603' td='39358' odate='2007-10-05'/>
			<t d='4360' v='939.612945' td='39361' odate='2007-10-08'/>
			<t d='4361' v='944.421967' td='39362' odate='2007-10-09'/>
			<t d='4362' v='946.036930' td='39363' odate='2007-10-10'/>
			<t d='4363' v='946.021593' td='39364' odate='2007-10-11'/>
			<t d='4364' v='948.534828' td='39365' odate='2007-10-12'/>
			<t d='4367' v='945.740505' td='39368' odate='2007-10-15'/>
			<t d='4368' v='949.244867' td='39369' odate='2007-10-16'/>
			<t d='4369' v='947.814317' td='39370' odate='2007-10-17'/>
			<t d='4370' v='947.978920' td='39371' odate='2007-10-18'/>
			<t d='4371' v='934.662853' td='39372' odate='2007-10-19'/>
			<t d='4374' v='933.910915' td='39375' odate='2007-10-22'/>
			<t d='4375' v='938.361567' td='39376' odate='2007-10-23'/>
			<t d='4376' v='942.814683' td='39377' odate='2007-10-24'/>
			<t d='4377' v='944.416620' td='39378' odate='2007-10-25'/>
			<t d='4378' v='947.846632' td='39379' odate='2007-10-26'/>
			<t d='4381' v='949.424143' td='39382' odate='2007-10-29'/>
			<t d='4382' v='943.589952' td='39383' odate='2007-10-30'/>
			<t d='4383' v='952.235968' td='39384' odate='2007-10-31'/>
			<t d='4384' v='944.346600' td='39385' odate='2007-11-01'/>
			<t d='4385' v='950.793250' td='39386' odate='2007-11-02'/>
			<t d='4388' v='949.171772' td='39389' odate='2007-11-05'/>
			<t d='4389' v='956.506262' td='39390' odate='2007-11-06'/>
			<t d='4390' v='945.556878' td='39391' odate='2007-11-07'/>
			<t d='4391' v='947.123960' td='39392' odate='2007-11-08'/>
			<t d='4392' v='937.129590' td='39393' odate='2007-11-09'/>
			<t d='4395' v='924.362463' td='39396' odate='2007-11-12'/>
			<t d='4396' v='937.832312' td='39397' odate='2007-11-13'/>
			<t d='4397' v='933.976120' td='39398' odate='2007-11-14'/>
			<t d='4398' v='931.243600' td='39399' odate='2007-11-15'/>
			<t d='4399' v='937.449630' td='39400' odate='2007-11-16'/>
			<t d='4402' v='936.889122' td='39403' odate='2007-11-19'/>
			<t d='4403' v='939.898193' td='39404' odate='2007-11-20'/>
			<t d='4404' v='931.859418' td='39405' odate='2007-11-21'/>
			<t d='4406' v='937.762755' td='39407' odate='2007-11-23'/>
			<t d='4409' v='933.329368' td='39410' odate='2007-11-26'/>
			<t d='4410' v='934.414565' td='39411' odate='2007-11-27'/>
			<t d='4411' v='941.464192' td='39412' odate='2007-11-28'/>
			<t d='4412' v='939.548820' td='39413' odate='2007-11-29'/>
			<t d='4413' v='939.146852' td='39414' odate='2007-11-30'/>
			<t d='4416' v='939.327620' td='39417' odate='2007-12-03'/>
			<t d='4417' v='935.596208' td='39418' odate='2007-12-04'/>
			<t d='4418' v='940.867232' td='39419' odate='2007-12-05'/>
			<t d='4419' v='948.063420' td='39420' odate='2007-12-06'/>
			<t d='4420' v='947.108653' td='39421' odate='2007-12-07'/>
			<t d='4423' v='949.385282' td='39424' odate='2007-12-10'/>
			<t d='4424' v='938.815717' td='39425' odate='2007-12-11'/>
			<t d='4425' v='945.749098' td='39426' odate='2007-12-12'/>
			<t d='4426' v='874.825982' td='39427' odate='2007-12-13'/>
			<t d='4427' v='868.174793' td='39428' odate='2007-12-14'/>
			<t d='4430' v='862.476718' td='39431' odate='2007-12-17'/>
			<t d='4431' v='865.963000' td='39432' odate='2007-12-18'/>
			<t d='4432' v='864.440900' td='39433' odate='2007-12-19'/>
			<t d='4433' v='867.723332' td='39434' odate='2007-12-20'/>
			<t d='4434' v='873.927200' td='39435' odate='2007-12-21'/>
			<t d='4437' v='878.026500' td='39438' odate='2007-12-24'/>
			<t d='4439' v='876.818283' td='39440' odate='2007-12-26'/>
			<t d='4440' v='873.755833' td='39441' odate='2007-12-27'/>
			<t d='4441' v='876.592392' td='39442' odate='2007-12-28'/>
			<t d='4444' v='871.839328' td='39445' odate='2007-12-31'/>
			<t d='4446' v='871.891227' td='39447' odate='2008-01-02'/>
			<t d='4447' v='873.419400' td='39448' odate='2008-01-03'/>
			<t d='4448' v='865.539220' td='39449' odate='2008-01-04'/>
			<t d='4451' v='866.637677' td='39452' odate='2008-01-07'/>
			<t d='4452' v='859.918380' td='39453' odate='2008-01-08'/>
			<t d='4453' v='866.041575' td='39454' odate='2008-01-09'/>
			<t d='4454' v='866.468295' td='39455' odate='2008-01-10'/>
			<t d='4455' v='859.779120' td='39456' odate='2008-01-11'/>
			<t d='4458' v='860.828033' td='39459' odate='2008-01-14'/>
			<t d='4459' v='848.925030' td='39460' odate='2008-01-15'/>
			<t d='4460' v='848.042492' td='39461' odate='2008-01-16'/>
			<t d='4461' v='836.284115' td='39462' odate='2008-01-17'/>
			<t d='4462' v='835.640072' td='39463' odate='2008-01-18'/>
			<t d='4466' v='837.254415' td='39467' odate='2008-01-22'/>
			<t d='4467' v='838.045052' td='39468' odate='2008-01-23'/>
			<t d='4468' v='839.973648' td='39469' odate='2008-01-24'/>
			<t d='4469' v='833.895630' td='39470' odate='2008-01-25'/>
			<t d='4472' v='842.803995' td='39473' odate='2008-01-28'/>
			<t d='4473' v='845.430740' td='39474' odate='2008-01-29'/>
			<t d='4474' v='840.550500' td='39475' odate='2008-01-30'/>
			<t d='4475' v='848.741953' td='39476' odate='2008-01-31'/>
			<t d='4476' v='857.845568' td='39477' odate='2008-02-01'/>
			<t d='4479' v='853.439008' td='39480' odate='2008-02-04'/>
			<t d='4480' v='840.679468' td='39481' odate='2008-02-05'/>
			<t d='4481' v='840.065625' td='39482' odate='2008-02-06'/>
			<t d='4482' v='842.305800' td='39483' odate='2008-02-07'/>
			<t d='4483' v='845.186025' td='39484' odate='2008-02-08'/>
			<t d='4486' v='851.906582' td='39487' odate='2008-02-11'/>
			<t d='4487' v='850.451680' td='39488' odate='2008-02-12'/>
			<t d='4488' v='847.884968' td='39489' odate='2008-02-13'/>
			<t d='4489' v='845.635940' td='39490' odate='2008-02-14'/>
			<t d='4490' v='845.751075' td='39491' odate='2008-02-15'/>
			<t d='4494' v='847.430710' td='39495' odate='2008-02-19'/>
			<t d='4495' v='848.874370' td='39496' odate='2008-02-20'/>
			<t d='4496' v='840.585060' td='39497' odate='2008-02-21'/>
			<t d='4497' v='840.181450' td='39498' odate='2008-02-22'/>
			<t d='4500' v='845.757151' td='39501' odate='2008-02-25'/>
			<t d='4501' v='847.635480' td='39502' odate='2008-02-26'/>
			<t d='4502' v='838.638900' td='39503' odate='2008-02-27'/>
			<t d='4503' v='847.758831' td='39504' odate='2008-02-28'/>
			<t d='4504' v='839.960550' td='39505' odate='2008-02-29'/>
			<t d='4507' v='847.731480' td='39508' odate='2008-03-03'/>
			<t d='4508' v='841.115489' td='39509' odate='2008-03-04'/>
			<t d='4509' v='842.669069' td='39510' odate='2008-03-05'/>
			<t d='4510' v='831.626800' td='39511' odate='2008-03-06'/>
			<t d='4511' v='827.904031' td='39512' odate='2008-03-07'/>
			<t d='4514' v='819.486250' td='39515' odate='2008-03-10'/>
			<t d='4515' v='826.249405' td='39516' odate='2008-03-11'/>
			<t d='4516' v='827.800360' td='39517' odate='2008-03-12'/>
			<t d='4517' v='835.812000' td='39518' odate='2008-03-13'/>
			<t d='4518' v='832.452740' td='39519' odate='2008-03-14'/>
			<t d='4521' v='826.391161' td='39522' odate='2008-03-17'/>
			<t d='4522' v='835.385591' td='39523' odate='2008-03-18'/>
			<t d='4523' v='821.599500' td='39524' odate='2008-03-19'/>
			<t d='4524' v='825.292000' td='39525' odate='2008-03-20'/>
			<t d='4528' v='812.124450' td='39529' odate='2008-03-24'/>
			<t d='4529' v='815.466100' td='39530' odate='2008-03-25'/>
			<t d='4530' v='815.864030' td='39531' odate='2008-03-26'/>
			<t d='4531' v='815.656020' td='39532' odate='2008-03-27'/>
			<t d='4532' v='820.516500' td='39533' odate='2008-03-28'/>
			<t d='4535' v='824.499600' td='39536' odate='2008-03-31'/>
			<t d='4536' v='828.597630' td='39537' odate='2008-04-01'/>
			<t d='4537' v='831.391150' td='39538' odate='2008-04-02'/>
			<t d='4538' v='832.209150' td='39539' odate='2008-04-03'/>
			<t d='4539' v='836.467500' td='39540' odate='2008-04-04'/>
			<t d='4542' v='836.716650' td='39543' odate='2008-04-07'/>
			<t d='4543' v='839.632710' td='39544' odate='2008-04-08'/>
			<t d='4544' v='837.608750' td='39545' odate='2008-04-09'/>
			<t d='4545' v='832.947330' td='39546' odate='2008-04-10'/>
			<t d='4546' v='829.391070' td='39547' odate='2008-04-11'/>
			<t d='4549' v='832.504790' td='39550' odate='2008-04-14'/>
		</ts>
	</secdata>
</securitydatas>";
    }

    /// <summary>
    /// XML Data Entity
    /// </summary>
    public class vData
    {
        public vData()
        {}

        public int AttributeD { get; set; }
        public double AttributeV { get; set; }
        public int AttributeTD { get; set; }
        public DateTime AttributeOdate { get; set; }
    }

    public class ErrorMsg
    {
        public ErrorMsg() { }

        public string Msg { get; set; }
    }
}
