using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class CalculatorClient:ClientBase<ICalculator>,ICalculator
    {
        internal CalculatorClient() : base() { }

        #region ICalculator Members

        public double Add(double x, double y)
        {
            return this.Channel.Add(x, y);
        }

        #endregion
    }
}
