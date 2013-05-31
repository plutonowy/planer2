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

		public Controller (Model mod)
		{
			this.mod = mod;
			subNodesList = mod.GetNodesList(0);
			prevSubNodesList = new List<Node>();
			currentNode = null;		
		}
		public Controller (string path)
		{
			this.mod = new Model(path);
			subNodesList = mod.GetNodesList(0);
			prevSubNodesList = new List<Node>();
			currentNode = null;		
		}

		public void controllerInput (string input)
		{
			string[] inputTable = input.Split (' ');
			switch (inputTable [0]) 
			{
				case "cd": Cd(inputTable); break;
				case "chState": ChState(inputTable); break;
				case "chName": ChName(inputTable); break;
				case "add": AddNode(inputTable); break;
				case "del": DelNode(inputTable); break;
				default: break;
				}
		}
		
		private void Cd(params string[] args)
		{
			if (args [1] == "..") 
			{
				if(prevSubNodesList.Count<1)
				{		
					currentNode = null;
					prevSubNodesList.Clear();
					subNodesList = mod.GetNodesList(0);
				}
				else
				{				
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
				
						subNodesList=mod.GetSubNodesList(temp.GetID());
						prevSubNodesList = mod.GetPrevNodesList(temp.GetID());
						break;
					}
				}
			}
		}

		private void ChName(params string[] args)
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
			//View.WriteMessage("nie ma takiego podwezla");


		}
		private void ChState(params string[] args)
		{
			foreach (Node temp in subNodesList) 
			{
				if(temp.name == args[1])
				{
					switch(args[2])
					{
						case "completed" : temp.state= StateOfNode.completed; break;
						case "uncompleted" : temp.state = StateOfNode.uncompleted; break;
						default: break;
					}
					mod.Save(temp);
					currentNode = temp;
					return;
				}
			}

		}

		private void AddNode (params string[] args)
		{
			Node newNode = new Node (args [1]);
			if (currentNode != null) {
				mod.NewNode (currentNode.GetID (), newNode);
				subNodesList = mod.GetSubNodesList (currentNode.GetID ());
			} 
			else 
			{
				mod.NewNode (true, newNode);
				subNodesList=mod.GetNodesList(0);
			}
		}

		private void DelNode(params string[] args)
		{
			foreach (Node tmp in subNodesList) 
			{
				if(tmp.name == args[1])
				{
					DestroyChildren(tmp);
					if(currentNode.Level==0)
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
	}

}

