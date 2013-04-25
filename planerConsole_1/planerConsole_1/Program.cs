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

			//mod.LoadCurrentSubNodesList(3);

			mod.SetReaderOn(1);
			mod.LoadCurrentSubNodesList();
			//TESTOWANIE TUTAJ ;)

			Console.ReadLine();
		}
	}
}
