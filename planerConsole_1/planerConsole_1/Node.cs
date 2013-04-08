using System;

namespace planerConsole_1
{
	public enum StateOfNode
	{
		uncompleted = 0,
		completed = 1,
		dontcare = 2
	};

	public class Node
	{
		public string name;
		public	StateOfNode state;
		public  UInt32 nodeID;

		//public static UInt32 GlobalID = 0; nie potrzebne

		public Node ()
		{
			this.name = null;
			this.state = StateOfNode.uncompleted;
			//this.nodeID = GlobalID + 1;
			//GlobalID++;
		}

		public Node (string newName, StateOfNode newState)
		{
			this.name = newName;
			this.state = newState;
			//this.nodeID = GlobalID + 1;
			//GlobalID++;
		}

		public void SetNodeID(UInt32 newNodeID)
		{
			this.nodeID = newNodeID;
		}

		//public static void SetGlobalID(UInt32 newGlobalID)
		//{
		//	GlobalID = newGlobalID;
		//}

	}
}

