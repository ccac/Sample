using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.Linq;
using Microsoft.Data.Web;
public class StudentDataService :
          WebDataService<StudentDataContainer>
{
    public static void InitializeService(
                                IWebDataServiceConfiguration config)
    {
        config.SetResourceContainerAccessRule(
                "*", ResourceContainerRights.AllRead);
    }
}

