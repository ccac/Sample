using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artech.WCFService.Contract;
using System.ServiceModel;

namespace Artech.WCFService.Service
{
    public class CalculatorService : ICalculator
    {
        #region ICalculator Members

        public double Add(double x, double y)
        {
            return x + y;
        }

        #endregion
    }
}
