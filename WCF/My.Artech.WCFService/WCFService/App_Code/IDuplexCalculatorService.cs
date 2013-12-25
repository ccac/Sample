using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the interface name "IDuplexCalculatorService" here, you must also update the reference to "IDuplexCalculatorService" in Web.config.
[ServiceContract]
public interface IDuplexCalculatorService
{
    [OperationContract]
    void DoWork();
}
