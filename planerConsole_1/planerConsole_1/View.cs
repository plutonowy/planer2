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
				Ls();
				break;
			case "help":
				Help();
				break;
			case "progress":
				Progress();
				break;
			case "tree":
				// wywolanie tree
				break;
			default:
				C.controllerInput(line);
				break;
			}
		}

	}
}

