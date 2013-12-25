using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.WCFService.Contract
{
    [ServiceContract]
    public interface ICallBack
    {
        [OperationContract(IsOneWay=true)]
        void DisplayResult(double result);
    }
}
