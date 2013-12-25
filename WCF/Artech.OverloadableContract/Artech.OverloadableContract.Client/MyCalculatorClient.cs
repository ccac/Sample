using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.OverloadableContract.Client
{
    class MyCalculatorClient:ClientBase<IMyCalculator>,IMyCalculator
    {
        #region IMyCalculator Members

        public double Add(double x, double y)
        {
            return this.Channel.Add(x, y);
        }

        public double Add(double x, double y, double z)
        {
            return this.Channel.Add(x, y,z);
        }

        #endregion
    }
}
