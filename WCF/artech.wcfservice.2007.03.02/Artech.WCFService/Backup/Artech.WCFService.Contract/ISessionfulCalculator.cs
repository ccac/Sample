using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.WCFService.Contract
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ISessionfulCalculator
    {
        [OperationContract(IsOneWay =true, IsInitiating = true, IsTerminating = false)]
        void Add(double x);

        [OperationContract(IsOneWay = false, IsInitiating = false, IsTerminating = true)]
        double GetResult();
    }
}
