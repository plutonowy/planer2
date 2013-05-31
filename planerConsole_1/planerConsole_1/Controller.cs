using System;
using System.Collections.Generic;

namespace planerConsole_1
{
	public class Controller
	{
		public Model mod;
		public List<Node> subNodesList;
		public List<Node> prevSubNodesList;
		public Node currentNode;
		public int currentLvl;

		public Controller (Model mod)
		{
			this.mod = mod;
			subNodesList = mod.GetSubNodesList(0);
			prevSubNodesList = mod.GetPrevNodesList(0);
			currentNode = null;
			currentLvl = 0;

		}

		public void controllerInput (string input)
		{
			string[] inputTable = input.Split (' ');
			switch (inputTable [0]) 
			{
				case "cd": Cd(inputTable); break;
				case "ChState": ChState(inputTable); break;
				case "ChName": ChName(inputTable); break;
				case "AddNode": AddNode(inputTable); break;
				case "DelNode": DelNode(inputTable); break;
				case "Help": Help(); break;
				default: break;
				}
		}
		
		public void Cd(params string[] args)
		{
			if (args [1] == "..") 
			{
				if(prevSubNodesList.Count<1)
				{
					currentLvl = 0;
					currentNode = null;
					prevSubNodesList.Clear();
					subNodesList = mod.GetSubNodesList(0);
				}
				else
				{
					currentLvl--;
					int lastNode = prevSubNodesList.Count-1;
					currentNode = prevSubNodesList[lastNode];
					prevSubNodesList = mod.GetPrevNodesList(currentNode.GetID());
					subNodesList = mod.GetSubNodesList(currentNode.GetID());
				}
			}
			else
			{
				if(subNodesList.Count==0) return;
				foreach(Node temp in subNodesList)
				{			

					if(temp.name == args[1])
					{
						currentNode = temp;
						currentLvl++;
						subNodesList=mod.GetSubNodesList(temp.GetID());
						prevSubNodesList = mod.GetPrevNodesList(temp.GetID());
						break;
					}
				}
			}


		}
		public void ChName(params string[] args)
		{
			string newName = args [2];
			foreach (Node temp in subNodesList) 
			{
				if(temp.name == args[1])
				{
					temp.name = newName;
					mod.Save(temp);
					currentNode = temp;
					return;
				}
			}
			View.WriteMessage("nie ma takiego podwezla");


		}
		public void ChState(params string[] args)
		{
			foreach (Node temp in subNodesList) 
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
					currentNode = temp;
					return;
				}
			}
			View.WriteMessage("nie ma takiego podwęzła!");
		}

		public void AddNode(params string[] args)
		{
			UInt32 newID = 1000; //tymczasowa wartosc
			Node newNode = new Node(args[1],StateOfNode.uncompleted,newID,currentLvl+1);
			if(currentLvl!=0)
				mod.NewNode(currentNode.GetID(),newNode);
			else mod.NewNode(true,newNode);
			subNodesList.Add(newNode);

		}

		public void DelNode(params string[] args)
		{
			foreach (Node tmp in subNodesList) 
			{
				if(tmp.name == args[1])
				{
					DestroyChildren(tmp);
					if(currentLvl==0)
						subNodesList = mod.GetSubNodesList(0);
					else
						subNodesList = mod.GetSubNodesList(currentNode.GetID());
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


		public void Help()
		{

		}
	}

}

