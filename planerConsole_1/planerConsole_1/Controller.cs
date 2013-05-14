using System;
using System.Collections.Generic;

namespace planerConsole_1
{
	public class Controller
	{
		public List<Command> commandList;
		public Model mod;
		public Controller (Model mod)
		{
			commandList = new List<Command>();
			this.mod = mod;
			initCommandlist();
		}

		public void initCommandlist()
		{
			commandList.Add (new CommandLS(mod));
			commandList.Add(new CommandCD(mod));
			commandList.Add(new CommandCN(mod));
			commandList.Add(new CommandCS(mod));
			commandList.Add (new CommandADD(mod));
			commandList.Add (new CommandDEL(mod));
			commandList.Add(new CommandPRGS(mod,this));
		
			//pozostale komendy
			commandList.Add (new CommandHELP(this));
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
					else Console.WriteLine("error, komenda przyjmuje {0} argument(ow)",lookingFor.numberOfParameters);
					return;
				}
			}
			Console.WriteLine("nie ma takiej komendy");
		}
	}
}

