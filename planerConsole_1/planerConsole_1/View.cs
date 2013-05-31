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
			Console.WriteLine ("Czy chcesz utworyć nowy projekt? Y/N");
			string ans = Console.ReadLine ();

			if (ans == "y" || ans == "Y") 
			{
				Console.WriteLine ("Podaj Nazwę(sciezke) pliku:");
				string filePath = Console.ReadLine ();

				StreamWriter sw = new StreamWriter (filePath, false);
				sw.Close ();

				C = new Controller(filePath);
				Console.Write(">>:");
			}
			else
			{
				string filePath;
				while(true)
				{
					Console.WriteLine("Podaj Sciezke do istniejącego pliku:");
					filePath = Console.ReadLine(); 

					if(File.Exists(filePath)) break;
					else if(filePath == "exit") throw new Exception("exit");
					else Console.WriteLine("Nie Poprawna sciezka!!");
				}

				C = new Controller(filePath);
				Console.Write(">>:");
			}
		}

		public void Input (string line)
		{
			switch (line) 
			{
			case "ls":
				Ls ();
				break;
			case "help":
				Help ();
				break;
			case "progress":
				Progress ();
				break;
			case "tree":
				// wywolanie tree
				break;
			default:
				C.controllerInput (line);
				break;
			}

			if(C.currentNode != null)Console.Write("{0}>>:", C.currentNode.name);
			else Console.Write(">>:");
		}

		private void Help()
		{
			Console.WriteLine("cd <name> \t przechodzenie do wezle 'name'\nls \t wyswietlanie" +"podcelow\n" +
								"chState <name> <completed/uncompleted> \t zmiana stanu wezla 'name' na completed/uncompleted" +
			                  "\nchName <name> <newName> \t zmiana nazwy wezla 'name' na 'newName'" +
			                  "\nadd <name> \t tworzenie wezla 'name'" +
			                  "\ndel <name> \t usuwanie wezla 'name'" +
			                  "\nprogress \t wyswietla progres cuurent wezla");
		}
		private void Ls ()
		{
			if (C.currentNode != null) 
			{
				SetColor (C.currentNode.state);
				Console.WriteLine ("{0}:", C.currentNode.name);
				Console.ForegroundColor = ConsoleColor.White;
			}

			foreach (Node tmp in C.subNodesList) 
			{
				SetColor(tmp.state);

				Console.WriteLine("\t{0}({1})",tmp.name,tmp.state);

				Console.ForegroundColor = ConsoleColor.White;
			}

			Console.ForegroundColor = ConsoleColor.White;
		}

		private void Progress () // zrobic procent!!
		{
			if (C.currentNode != null)
			{
				float prog = C.mod.GetProgres (C.currentNode.GetID ()) * 100;
				//int prog2 = Convert.ToInt16(prog); // zmiana float na int
				Console.WriteLine ("postep dla {0}: {1}%", C.currentNode.name, (int)prog);
			}
		}

		private void SetColor(StateOfNode state)
		{
			switch(state)
				{
				case StateOfNode.completed:
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					break;
				case StateOfNode.dontcare:
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				default:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				}
		}
	}
}

