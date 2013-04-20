using System;
using System.Collections.Generic;

namespace planerConsole_1
{
	public class Controller
	{
		public List<Command> commandList;

		public Controller ()
		{
			commandList = new List<Command>();
			initCommandlist();
		}

		public void initCommandlist()
		{
			commandList.Add (new CommandLS());
			commandList.Add(new CommandCD());
			commandList.Add (new CommandHELP(this));
			//pozostale komendy

		}

		public void controllerInput (string input)
		{
			string[] inputTable = input.Split(' ');
			//przeszukiwanie listy komend i potem wykonywanie jej funkcji realizeCommand
			foreach (Command lookingFor in commandList) 
			{
				if(lookingFor.commandName == inputTable[0])
				{
					if(lookingFor.numberOfParameters == inputTable.Length-1 && inputTable[inputTable.Length-1]!="")
						lookingFor.realizeCommand(inputTable);
					else Console.WriteLine("error, komenda przyjmuje {0} argumentow",lookingFor.numberOfParameters);
				}
			}
		}
	}
}

