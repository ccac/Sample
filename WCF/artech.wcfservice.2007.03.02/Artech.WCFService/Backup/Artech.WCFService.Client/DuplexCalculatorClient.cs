using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class DuplexCalculatorClient:ClientBase<IDuplexCalculator>,IDuplexCalculator
    {

        public DuplexCalculatorClient(InstanceContext callbackInstance)
            : base(callbackInstance)
        { }

        #region IDuplexCalculator Members

        public void Add(double x, double y)
        {
            this.Channel.Add(x, y);
        }

        #endregion
    }
}
