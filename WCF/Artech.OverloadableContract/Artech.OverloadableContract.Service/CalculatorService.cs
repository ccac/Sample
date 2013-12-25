using System;
using System.Collections.Generic;
using System.Text;

namespace Artech.OverloadableContract.Service
{
    public class CalculatorService:ICalculator
    {
        #region ICalculator Members

        public double Add(double x, double y)
        {
            return x + y;
        }

        public double Add(double x, double y, double z)
        {
            return x + y + z;
        }

        #endregion
    }
}
