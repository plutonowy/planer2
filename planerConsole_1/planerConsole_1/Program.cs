using System;

namespace planerConsole_1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			//TESTOWANIE TUTAJ ;)

			Model mod = new Model("/home/ramzes/Dokumenty/mono-workspace/planer2/nowytyp.txt");

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

			//TESTOWANIE TUTAJ ;)

			Console.ReadLine();
		}
	}
}
