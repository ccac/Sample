using System;
using System.Diagnostics;
using System.Reflection;

namespace SillyAOP
{
	/// <summary>
	/// Summary description for PropertyPrinter.
	/// </summary>
	public class PropertyPrinter
	{
		public void Print(bool bModified, object value)
		{
			StackFrame frame=new StackFrame(1,true);
			// Get Current Method Name set_XXX, then remove "set_"
			string propertyName = frame.GetMethod().Name.Substring(4); 

			string str = null;
			string valueString = null;

			if (value == null)
				valueString = "null";
			else
				valueString = value.ToString();

			if (bModified == true)
			{
				str = "Modified";
				valueString+=Environment.NewLine;
			}
			else
			{
				str = "Original";
			}

			Console.WriteLine("{0} Value of {1} : {2}", str, propertyName, valueString);
		}
	}
}
