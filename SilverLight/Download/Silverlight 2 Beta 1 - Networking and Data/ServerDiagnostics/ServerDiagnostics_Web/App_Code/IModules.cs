using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the interface name "IModules" here, you must also update the reference to "IModules" in Web.config.
[ServiceContract]
public interface IModules
{
    [OperationContract]
    ModuleSummary[] GetModules();
}
