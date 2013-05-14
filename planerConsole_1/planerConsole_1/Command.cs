using System;
using System.Collections.Generic;
using System.IO;

namespace planerConsole_1
{
	abstract public class Command
	{
		public readonly string commandName;
		public readonly string commandDescription;
		public readonly int numberOfParameters;


		public Command (string commandName, string commandDescription, int numberOfParameters)
		{
			this.commandName = commandName;
			this.commandDescription = commandDescription;
			this.numberOfParameters = numberOfParameters;
		}
		public abstract void realizeCommand(params string[] args);
	}

	public class CommandPRGS : Command
	{
		public Model mod;
		public Controller conductor;
		public double progressSum;
		public int numberOfNodes;
		public CommandPRGS (Model mod, Controller conductor) : base("prgs", "wyswietlanie postępu", 0)
		{
			this.mod = mod;
			this.conductor = conductor;
			this.progressSum = 0;
			this.numberOfNodes = 0;
		}
		public override void realizeCommand (params string[] args)
		{
			int countCompleted = 0;
			int countAll = 0;
			foreach (Node tmp in mod.currentSubNodesList) 
			{
				if(tmp.state == StateOfNode.completed)
					countCompleted++;
				countAll++;
			}
			Progress();
			double progress = (progressSum/numberOfNodes)*100;
			if(double.IsNaN(progress)) progress = 0;

			Console.WriteLine("progress: {0}%", progress);
		}

		public void Progress() //chyba nie moze zadzialac, nalezy przeszukac plikt tekstowy
		{			//bo zmienia sie currentSubnodesList
			List<Node> tempList = mod.currentSubNodesList;

			foreach (Node tmp in tempList) 
			{
				if(checkNode(tmp.name))
				{
					conductor.controllerInput(string.Format("cd {0}",tmp.name));
					if(tmp.state == StateOfNode.completed)
						progressSum++;
					numberOfNodes++;
					Console.WriteLine("jestem");
					Progress();
					conductor.controllerInput("cd ..");
				}
			}
		}
		public bool checkNode (string name)
		{
			foreach(Node tmp in mod.currentSubNodesList)
				if(tmp.name == name) return true;
			return false;
		}

	}

	public class CommandDEL : Command
	{
		public Model mod;
		public CommandDEL (Model mod) : base("del", "usuwanie podwezla: del <nazwa>", 1)
		{
			this.mod = mod;
		}
		public override void realizeCommand(params string[] args)
		{
			Console.WriteLine("usuwam podwezel o nazwie {0}",args[1]);
			//mod.DeleteNode(args[1]);
		}
	}

	public class CommandADD : Command
	{
		public Model mod;
		public CommandADD (Model mod) : base ("add", "dodawanie nowego podwezla: add <nazwa>", 1)
		{
			this.mod = mod;
		}
		public override void realizeCommand (params string[] args)
		{
			Console.WriteLine("dodaje nowy wezel o nazwie {0}", args[1]);

			//mod.AddNode(args[1]);
		}
	}

	public class CommandCN : Command
	{
		public Model mod;
		public CommandCN (Model mod) : base("cn", "zmiana nazwy podwezla: cn <nazwa zmienianego> <nazwa na ktora chcemy zmienic>", 2)
		{
			this.mod = mod;
		}
		public override void realizeCommand (params string[] args)
		{
			foreach (Node temp in mod.currentSubNodesList) 
			{
				if(temp.name == args[1])
				{
				Console.WriteLine ("zmieniam nazwe wezla {0} na {1}", args [1], args [2]);
				//mod.LoadNode(args[1],args[2]))
					return;
				}
			}
			Console.WriteLine("nie ma takiego podwezla!");
		}
	}

	public class CommandCS : Command
	{
		public Model mod;
		public CommandCS (Model mod) : base("cs", "zmiana stanu podwezla: cs <nazwa zmienianego> <uncompleted/completed>", 2)
		{
			this.mod = mod;
		}
		public override void realizeCommand (params string[] args)
		{
			foreach (Node temp in mod.currentSubNodesList) 
			{
				if(temp.name == args[1])
				{
				
					switch(args[2])
					{
						case "completed": 
							Console.WriteLine ("zmieniam stan wezla {0} na {1}", args[1], args[2]);
							//mod.LoadNode(args[1],StateOfNode.completed);
							break;
						case "uncompleted" :
							Console.WriteLine ("zmieniam stan wezla {0} na {1}", args[1], args[2]);
							//mod.LoadNode(args[1], StateOfNode.uncompleted);
							break;
					default: Console.WriteLine("halo halo! bledny stan!"); break;
					}

					return;
				}
			}
			Console.WriteLine("nie ma takiego podwezla!"); // niepotrzebne
		}
	}

	public class CommandCD : Command
	{
		public Model mod;
		public CommandCD (Model mod) : base("cd", "przechodzenie do kolejnego wezla: cd <nazwa>",1)
		{
			this.mod = mod;
		}
		public override void realizeCommand (params string[] args)
		{
			if (args [1] == "..") {
				mod.GoBack();
			}
			else
			{
				Console.WriteLine ("przechodze do wezla o nazwie {0}", args [1]);
				mod.LoadNode (args [1]);
			}
		}
	}

	public class CommandLS : Command
	{
		public Model mod;
		public CommandLS (Model mod) : base("ls", "wyswietlanie podwezlow", 0)
		{
			this.mod = mod;
		}
		public override void realizeCommand (params string[] args)
		{
			foreach (Node temp in mod.currentSubNodesList) 
			{
				Console.Write("{0}({1})\t",temp.name, temp.state);
			}
			Console.WriteLine();
		}
	}

	public class CommandHELP : Command
	{
		public Controller con;
		public CommandHELP (Controller con) : base("help", "wyswietlanie wszystkich komend", 0)
		{
			this.con = con;
		}

		public override void realizeCommand (params string[] args)
		{
			string temp = "\nlista komend: \n";
			foreach (Command one in con.commandList) 
			{
				temp=temp+one.commandName+"\t"+one.commandDescription+"\n";
			}
			Console.WriteLine(temp);
		}
	}
}

