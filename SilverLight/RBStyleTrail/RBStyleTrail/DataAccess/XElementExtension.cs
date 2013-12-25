using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;

namespace RBStyleTrail.DataAccess
{


    public class XElementList : IEnumerable
    {

        private List<XElement> elementList_ = new List<XElement>();

        public int Count { get { return elementList_.Count; } }

        public XElement this[int i]
        {
            get { return i >= elementList_.Count ? null : elementList_[i]; }
        }

#if DEBUG
        public
#else
		internal
#endif
 void Append(XElement element)
        {
            elementList_.Add(element);
        }


#if DEBUG
        public
#else
		internal
#endif
 void Remove(XElement element)
        {
            elementList_.Remove(element);
        }

#if DEBUG
        public
#else
		internal
#endif
 void RemoveRange(int index,int count)
        {
            elementList_.RemoveRange(index,count);
        }


#if DEBUG
        public
#else
		internal
#endif
        void AppendRange(XElementList elementList)
        {
            elementList_.AddRange(elementList.elementList_);
        }


        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            //throw new NotImplementedException();
            return elementList_.GetEnumerator();
        }

        #endregion
    }

    public static class XElementExtension
    {
        public static XElementList SelectElements(this XElement parentElement, string sPath)
        {
            if (parentElement == null)
                return null;

            string[] pathSegment = sPath.Split('/');

            XElementList elementList = new XElementList();
            //XElement temp = parentElement;
            elementList.Append(parentElement);
            for (int i = 0; i < pathSegment.Length; i++)
            {
                string seg = pathSegment[i].Trim();
                if (seg != "")
                {
                    string tagName = seg;
                    string atbName = "";
                    string atbValue = "";

                    //such as seg = "secdata[@secid=fundID]"
                    if (seg.Contains("[") && seg.Contains("]"))
                    {
                        tagName = seg.Substring(0, seg.IndexOf("["));
                        if (seg.Contains("@") && seg.Contains("="))
                        {
                            atbName = seg.Substring(seg.IndexOf("@") + 1, seg.IndexOf("=") - seg.IndexOf("@") - 1);
                            atbValue = seg.Substring(seg.IndexOf("=") + 2, seg.IndexOf("]") - seg.IndexOf("=") - 3);
                        }
                    }

                    int count = elementList.Count;
                    for (int j = 0; j < count; j++)
                    {
                        XElement element = elementList[j];
                        if (atbName != "")
                        {
                            //this mean we need to do attribute filtration
                            foreach (XElement temp in element.Elements(tagName))
                            {
                                if ((temp.Attribute(atbName) != null) && (temp.Attribute(atbName).Value == atbValue))
                                {
                                    elementList.Append(temp);
                                }
                            }
                        }
                        else
                        {
                            foreach (XElement temp in element.Elements(tagName))
                            {
                                elementList.Append(temp);
                            }
                        }
                    }
                    elementList.RemoveRange(0, count);
                    if (elementList.Count == 0)
                    {
                        return null;
                    }
                }
            }

            return elementList;
        }

        public static XElement SelectSingleElement(this XElement parentElement, string sPath)
        {
            XElementList elementList = SelectElements(parentElement, sPath);
            if((elementList != null) && (elementList.Count > 0))
                return elementList[0];
            return null;
        }

        public static IEnumerable<XElement> ChildElements(this XElement parentElement, string sTagName)
        {
            var items = from c in parentElement.Elements(sTagName) select c;
            return items;
        }

        //public static IEnumerable<XElement> ChildElements(this XElement parentElement, string sTagName,MapItem<string,string>[] conditioncollections)
        //{

        //    var items = from c in parentElement.Elements(sTagName) select c;
        //    return items;
        //}
        //private void GetChildElement
    }
}
