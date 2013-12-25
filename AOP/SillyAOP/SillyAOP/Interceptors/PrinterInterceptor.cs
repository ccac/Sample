using System;
using AopAlliance.Intercept;

namespace SillyAOP.Interceptors
{
	/// <summary>
	/// Summary description for PrinterInterceptor.
	/// </summary>
	public class PrinterInterceptor :IMethodInterceptor
	{
		public object Invoke(IMethodInvocation invocation)
		{
			PropertyPrinter printer=new PropertyPrinter();
			string propName=invocation.Method.Name.Substring(4);
			
			object originalValue=invocation.Method.DeclaringType.GetProperty(propName).GetValue(invocation.This,null);
			printer.Print(false,originalValue);

			object returnVal = invocation.Proceed();
			
			object modifiedValue=invocation.Method.DeclaringType.GetProperty(propName).GetValue(invocation.This,null);
			printer.Print(true,modifiedValue);

			return returnVal;
		}
	}
}
