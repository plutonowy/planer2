using System;
using System.Collections.Generic;

namespace planerConsole_1
{
	public class Controller
	{
		public List<Command> commandList;
		public Model mod;
		public List<Node> subNodesList;
		public List<Node> prevSubNodesList;
		public Node currentNode;
		public int currentLvl;

		public Controller (Model mod)
		{
			subNodesList = mod.GetSubNodesList(0);
			prevSubNodesList = mod.GetPrevNodesList(0);
			currentNode = null;
			currentLvl = 0;

			commandList = new List<Command>();
			this.mod = mod;
			initCommandlist();
		}

		public void initCommandlist()
		{
			commandList.Add (new CommandLS(this));
			commandList.Add(new CommandCD(mod, this));
			commandList.Add(new CommandCN(mod,this));
			commandList.Add(new CommandCS(mod,this));
			commandList.Add (new CommandADD(mod, this));
			commandList.Add (new CommandDEL(mod,this));
			commandList.Add(new CommandPRGS(mod,this));
		
			//pozostale komendy
			commandList.Add (new CommandHELP(this));
		}

		public void controllerInput (string input)
		{
			string[] inputTable = input.Split (' ');
			//przeszukiwanie listy komend i potem wykonywanie jej funkcji realizeCommand
			foreach (Command lookingFor in commandList) {
				if (lookingFor.commandName == inputTable [0]) {
					if (lookingFor.numberOfParameters == inputTable.Length - 1 && inputTable [inputTable.Length - 1] != "")
						lookingFor.realizeCommand (inputTable);
					else
						View.WriteMessage(string.Format ("error, komenda przyjmuje {0} argument(ow)", lookingFor.numberOfParameters));
					return;
				}
			}
			View.WriteMessage("nie ma takiej komendy");

		}
	}
}

