using System;
using System.Collections.Generic;

namespace planerConsole_1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("PlanYour World!");
			View viewer = new View();

			string line = "";
			while (true) 
			{
				line = Console.ReadLine();
				if(line=="Exit") break;
				viewer.Input(line);
			}
		}
	}
}
