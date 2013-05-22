using System;
using System.Collections.Generic;

namespace planerConsole_1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//TESTOWANIE TUTAJ ;)
			Node nod = new Node("NAZWA", StateOfNode.uncompleted, 15, 0);

			Console.WriteLine(nod.ToString());
			//TESTOWANIE TUTAJ ;)
			Console.ReadLine();
		}
	}
}
