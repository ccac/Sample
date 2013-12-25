using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.WCFService.Contract
{
    [ServiceContract(CallbackContract=typeof(ICallBack))]
    public interface ITcpDuplexCalculator
    {
        [OperationContract(IsOneWay=true)]
        void Add(double x, double y);
    }
}
