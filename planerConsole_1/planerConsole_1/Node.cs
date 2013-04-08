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


		public Node ()
		{
			this.name = null;
			this.state = StateOfNode.uncompleted;
			this.nodeID = 0;
		}

		public Node (string newName, StateOfNode newState, UInt32 newNodeID)
		{
			this.name = newName;
			this.state = newState;
			this.nodeID = newNodeID;
		}

		public void SetNodeID(UInt32 newNodeID)
		{
			this.nodeID = newNodeID;
		}

	}
}

