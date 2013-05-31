using System;
using System.Collections.Generic;
using System.IO;
namespace planerConsole_1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//TESTOWANIE TUTAJ ;)
			UInt32 cos = 1;
			Model mod = new Model(@"/home/ramzes/Dokumenty/mono-workspace/planer2/nowytyp.txt");
			List<Node> lista = mod.GetSubNodesList(cos);

			Node nowyW;
			nowyW = new Node("NOWY WEZEL1");
			mod.NewNode(1,nowyW);

			nowyW = new Node("NOWY WEZEL2");
			mod.NewNode(1,nowyW);

			nowyW = new Node("NOWY WEZEL3");
			mod.NewNode(1,nowyW);

			nowyW = mod.GetNode(11);
			nowyW.name = "ZMIENIONA NAZWA1";
			mod.Save(nowyW);

			mod.Delete(12);

			lista = mod.GetSubNodesList(cos);
			mod.Close();
			//TESTOWANIE TUTAJ ;)
			Console.ReadLine();
		}
	}
}
