using System;

namespace planerConsole_1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			//TESTOWANIE TUTAJ ;)

			Model mod = new Model("/home/ramzes/workspaces/mono-workspace/planer2/nowytyp.txt");

			mod.LoadCurrentSubNodesList(0);

			//TESTOWANIE TUTAJ ;)

			Console.ReadLine();
		}
	}
}
