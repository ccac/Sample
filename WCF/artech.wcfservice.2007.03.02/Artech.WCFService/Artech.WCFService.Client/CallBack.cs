using System;
using System.Collections.Generic;
using System.Text;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    public class CallBack:ICallBack
    {

        #region ICallBack Members

        public void DisplayResult(double result)
        {
            //throw new NotImplementedException();
            Console.WriteLine("The Result is {0}", result);
        }

        #endregion
    }
}
