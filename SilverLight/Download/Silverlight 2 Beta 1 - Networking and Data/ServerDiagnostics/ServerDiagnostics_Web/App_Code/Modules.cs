using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;

// NOTE: If you change the class name "Modules" here, you must also update the reference to "Modules" in Web.config.
public class Modules : IModules
{
    public ModuleSummary[] GetModules()
    {
        ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
        return (
                from m in modules.OfType<ProcessModule>()
                select new ModuleSummary
                {
                    Name = m.ModuleName,
                    Version = m.FileVersionInfo.FileVersion
                }
                ).ToArray();
    
    }
}
