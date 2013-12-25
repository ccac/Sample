using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using Artech.WCFService.Contract;

namespace Artech.WCFService.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SessionfulCalculatorService:ISessionfulCalculator
    {
        private double _result;

        #region ISessionfulCalculator Members

        public void Add(double x)
        {
            this._result += x;
        }

        public double GetResult()
        {
            return this._result;
        }

        #endregion
    }
}
