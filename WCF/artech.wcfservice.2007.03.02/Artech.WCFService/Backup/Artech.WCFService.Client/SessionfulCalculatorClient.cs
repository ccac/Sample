using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class SessionfulCalculatorClient:ClientBase<ISessionfulCalculator>,ISessionfulCalculator
    {
        #region ISessionfulCalculator Members

        public void Add(double x)
        {
            this.Channel.Add(x);
        }

        public double GetResult()
        {
            return this.Channel.GetResult();
        }

        #endregion
    }
}
