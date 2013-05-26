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

		}
	}

	public class CommandDEL : Command
	{
		private Model mod;
		private Controller con;
		public CommandDEL (Model mod, Controller con) : base("del", "usuwanie podwezla: del <nazwa>", 1)
		{
			this.mod = mod;
			this.con = con;
		}
		public override void realizeCommand (params string[] args)
		{
			foreach (Node tmp in con.subNodesList) 
			{
				if(tmp.name == args[1])
				{
					DestroyChildren(tmp);
					if(con.currentLvl==0)
						con.subNodesList = mod.GetSubNodesList(0);
					else
						con.subNodesList = mod.GetSubNodesList(con.currentNode.GetID());
				}
			}
		}
		void DestroyChildren (Node current)
		{
			List<Node> children = mod.GetSubNodesList (current.GetID ());
			if (children.Count == 0)
				mod.Delete (current.GetID());
			else 
			{
				foreach(Node tmp in children)
				{
					DestroyChildren(tmp);
					children = mod.GetSubNodesList(current.GetID());
				}
				DestroyChildren(current);
			}

		}
	}

	public class CommandADD : Command //nie dziala wpisuywanie do plikuu 
	{
		public Model mod;
		public Controller con;
		public CommandADD (Model mod, Controller con) : base ("add", "dodawanie nowego podwezla: add <nazwa>", 1)
		{
			this.mod = mod;
			this.con = con;
		}
		public override void realizeCommand (params string[] args)
		{
			View.WriteMessage(string.Format("dodaje nowy wezel o nazwie {0}", args[1]));
			UInt32 newID = 1000; //tymczasowa wartosc
			Node newNode = new Node(args[1],StateOfNode.uncompleted,newID,con.currentLvl+1);
			if(con.currentLvl!=0)
				mod.NewNode(con.currentNode.GetID(),newNode);
			else mod.NewNode(true,newNode);
			con.subNodesList.Add(newNode);
		}
	}

	public class CommandCN : Command
	{
		private Model mod;
		private Controller con;
		public CommandCN (Model mod, Controller con) : base("cn", "zmiana nazwy podwezla: cn <nazwa zmienianego> <nazwa na ktora chcemy zmienic>", 2)
		{
			this.mod = mod;
			this.con = con;
		}
		public override void realizeCommand (params string[] args)
		{
			string newName = args [2];
			foreach (Node temp in con.subNodesList) 
			{
				if(temp.name == args[1])
				{
					temp.name = newName;
					mod.Save(temp);
					con.currentNode = temp;
					return;
				}
			}
			View.WriteMessage("nie ma takiego podwezla");
		}
	}

	public class CommandCS : Command
	{
		private Model mod;
		private Controller con;
		public CommandCS (Model mod, Controller con) : base("cs", "zmiana stanu podwezla: cs <nazwa zmienianego> <uncompleted/completed>", 2)
		{
			this.mod = mod;
			this.con = con;
		}
		public override void realizeCommand (params string[] args)
		{
			foreach (Node temp in con.subNodesList) 
			{
				if(temp.name == args[1])
				{
					switch(args[2])
					{
						case "completed" : temp.state= StateOfNode.completed; break;
						case "uncompleted" : temp.state = StateOfNode.uncompleted; break;
						default: View.WriteMessage("błędny stan!"); break;
					}
					mod.Save(temp);
					con.currentNode = temp;
					return;
				}
			}
			View.WriteMessage("nie ma takiego podwęzła!");
		}
	}

	public class CommandCD : Command
	{
		public Model mod;
		public Controller con;


		public CommandCD (Model mod, Controller con) : base("cd", "przechodzenie do kolejnego wezla: cd <nazwa>",1)
		{
			this.mod = mod;
			this.con = con;
		}
		public override void realizeCommand (params string[] args)
		{
			if (args [1] == "..") 
			{
				if(con.prevSubNodesList.Count<1)
				{
					con.currentLvl = 0;
					con.currentNode = null;
					con.prevSubNodesList.Clear();
					con.subNodesList = mod.GetSubNodesList(0);
				}
				else
				{
					con.currentLvl--;
					int lastNode = con.prevSubNodesList.Count-1;
					con.currentNode = con.prevSubNodesList[lastNode];
					con.prevSubNodesList = mod.GetPrevNodesList(con.currentNode.GetID());
					con.subNodesList = mod.GetSubNodesList(con.currentNode.GetID());
				}
			}
			else
			{
				if(con.subNodesList.Count==0) return;
				foreach(Node temp in con.subNodesList)
				{			

					if(temp.name == args[1])
					{
						con.currentNode = temp;
						con.currentLvl++;
						con.subNodesList=mod.GetSubNodesList(temp.GetID());
						con.prevSubNodesList = mod.GetPrevNodesList(temp.GetID());
						break;
					}
				}
			}
		}
	}

	public class CommandLS : Command
	{
		public Controller conductor;
		public CommandLS (Controller conductor) : base("ls", "wyswietlanie podwezlow", 0)
		{
			this.conductor = conductor;
		}
		public override void realizeCommand (params string[] args)
		{
			if(conductor.subNodesList.Count!=0)
				View.DisplaySubNodesList(conductor.subNodesList);
			else 
				View.WriteMessage("brak podcelow");
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
			View.WriteMessage(temp);
		}
	}
}

