using System;
using System.IO;
using AspectSharp;
using AspectSharp.Builder;
using SillyAOP.Objects;

namespace SillyAOP
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
//			PropertyPrinter printer=new PropertyPrinter();
//			Employer e=new Employer(printer);
			AspectLanguageEngineBuilder builder = new AspectLanguageEngineBuilder(File.OpenText(@"../../PropertyAspect.cfg"));
			AspectEngine engine = builder.Build();       
			Employer e= engine.WrapClass(typeof(Employer)) as Employer;

			e.ID=12;
			e.Name="idior";
			e.ID=13;
			e.Name="cnblogs";
		}
	}
}
