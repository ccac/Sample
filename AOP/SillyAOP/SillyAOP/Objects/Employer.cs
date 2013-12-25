using System;
using System.Diagnostics;

namespace SillyAOP.Objects
{
	public class Employer
	{
		
        private PropertyPrinter propPrinter;
		private int id;
        private string name;

		public Employer()
		{
			
		}

//		public Employer(PropertyPrinter printer)
//		{
//            propPrinter = printer;
//		}

		public virtual int ID
		{
			get
			{
				return id;
			}
			set
			{
//				propPrinter.Print(false,id);
				id = value;
//				propPrinter.Print(true,id);
			}
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
//				propPrinter.Print(false,name);
				name = value;
//				propPrinter.Print(true,name);
			}
		}

		
		public void Work()
		{
		}
	}


}