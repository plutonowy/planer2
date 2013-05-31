using System;
using System.Collections.Generic;

namespace planerConsole_1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			//TESTOWANIE TUTAJ ;)
			//Model mod = new Model ("/home/kamil/Desktop/repositories/planer2/nowytyp.txt");
			Controller conductor = new Controller ("/home/kamil/Desktop/repositories/planer2/nowytyp.txt");
			while (true) 
			{
				Console.Write(":>> ");
				conductor.controllerInput(Console.ReadLine());


			//TESTOWANIE TUTAJ ;)

			//Console.ReadLine();
			}
		}
	}
}
