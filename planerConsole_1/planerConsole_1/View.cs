using System;
using System.Collections.Generic;
using System.IO;

namespace planerConsole_1
{
	public class View
	{
		public Controller C;

		public View ()
		{
	
		}

		public void Input (string line)
		{
			string[] inputTable = line.Split (' ');

			switch (inputTable [0]) 
			{
				case "Help": Help (); break;
				case "Ls" : Ls(); break;
				default: break;
			}
		}

		private void Help()
		{
			Console.WriteLine("Cd <name> \t przechodzenie do wezle 'name'\nLs \t wyswietlanie" +"podcelow\n" +
								"ChState <name> <completed/uncompleted> \t zmiana stanu wezla 'name' na completed/uncompleted" +
			                  "\nChName <name> <newName> \t zmiana nazwy wezla 'name' na 'newName'" +
			                  "\nAddNode <name> \t tworzenie wezla 'name'" +
			                  "\nDelNode <name> \t usuwanie wezla 'name'");
		}
		private void Ls ()
		{
			Console.WriteLine("{0}:",C.currentNode.name);
			foreach (Node tmp in C.subNodesList) 
			{
				Console.WriteLine("\t{0}({1})",tmp.name,tmp.state);
			}
		}

		private void Progress()
		{
			float prog = C.mod.GetProgres(C.currentNode.GetID());
			Console.WriteLine("postep dla {0}: {1}%",C.currentNode.name,prog*100);
		}
	}
}

