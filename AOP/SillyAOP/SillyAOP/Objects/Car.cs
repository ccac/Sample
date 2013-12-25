namespace SillyAOP.Objects
{

	public class Car
	{
		private PropertyPrinter propPrinter;

		private double speed = 0;

		public Car(PropertyPrinter printer)
		{
			propPrinter = printer;
		}

		public double Speed
		{
			get
			{
				return speed;
			}
			set
			{
				propPrinter.Print(false,speed);
				speed = value;
				propPrinter.Print(true,speed);
			}
		}
	}
}