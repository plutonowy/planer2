using System;

namespace planerConsole_1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			//TESTOWANIE TUTAJ ;)
			Model mod = new Model ("/home/kamil/Desktop/repositories/connector/nowytyp.txt");
			Controller conductor = new Controller (mod);
			while (true) 
			{
				Console.Write(":>> ");
				conductor.controllerInput(Console.ReadLine());


			}
			/*
			mod.LoadNode(0);
			mod.LoadNode(1);
			mod.LoadNode(3);
			mod.LoadNode(4);
			mod.LoadNode(5);

			mod.GoBack();
			mod.GoBack();
			mod.GoBack();
			mod.GoBack();
			mod.GoBack();
*/
			//TESTOWANIE TUTAJ ;)

			//Console.ReadLine();
		}
	}
}
