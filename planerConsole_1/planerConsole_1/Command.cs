using System;
using System.Collections.Generic;

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

	public class CommandLS : Command
	{
		public CommandLS () : base("ls", "wyswietlanie podwezlow", 0)
		{

		}
		public override void realizeCommand(params string[] args)
		{
			//wywołanie funkcji ls na obiekcie klasy widok - view???
			Console.WriteLine("tymczasowo dziala ls");
		}
	}
	public class CommandCD : Command
	{
		public CommandCD () : base("cd", "przechodzenie do kolejnego wezla",1)
		{}
		public override void realizeCommand(params string[] args)
		{
			Console.WriteLine("przechodze do wezla o nazwie {0}", args[1]);
		}
	}
	public class CommandHELP : Command
	{
		public Controller con;
		public CommandHELP (Controller con) : base("help", "wyswietlanie wsyzstkich komend", 0)
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

