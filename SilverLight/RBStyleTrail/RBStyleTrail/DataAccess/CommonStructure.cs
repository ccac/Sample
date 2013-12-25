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
    public class CommonStructure
    {

    }

    public class MapItem<IndexType, TValueType>
    {
        protected IndexType m_Index;
        protected TValueType m_Value;
        public MapItem(IndexType index, TValueType value)
        {
            m_Index = index;
            m_Value = value;
        }
        public IndexType Index
        {
            get
            {
                return m_Index;
            }
            set
            {
                m_Index = value;
            }
        }
        public TValueType Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }
    }
}
