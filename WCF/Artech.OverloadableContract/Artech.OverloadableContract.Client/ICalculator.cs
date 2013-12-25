using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.OverloadableContract.Client
{
    [ServiceContract]
   public interface IMyCalculator
    {
        [OperationContract(Name = "AddWithTwoOperands")]
       double Add(double x, double y);

       [OperationContract(Name = "AddWithThreeOperands")]
       double Add(double x, double y, double z);
    }
}
