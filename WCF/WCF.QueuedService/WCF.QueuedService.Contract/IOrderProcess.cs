using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCF.QueuedService.Contract
{
    [ServiceContract]
    [ServiceKnownType(typeof(Order))]
    public interface IOrderProcess
    {
        [OperationContract(IsOneWay=true)]
        void Submit(Order order);
    }
}
