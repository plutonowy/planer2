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
		public StateOfNode state;
		public UInt32 nodeID;
		public int Level;

		public Node ()
		{
			this.name = null;
			this.state = StateOfNode.uncompleted;
			this.nodeID = 0;
		}

		public Node (string newName, StateOfNode newState, UInt32 newNodeID, int newLevel)
		{
			this.name = newName;
			this.state = newState;
			this.nodeID = newNodeID;
			this.Level = newLevel;
		}

		public void SetNodeID(UInt32 newNodeID)
		{
			this.nodeID = newNodeID;
		}

		public override string ToString ()
		{
			string NodeToString;

			string string_ID = this.nodeID.ToString ();
			string string_level = "";

			char char_state;

			switch (this.state) {
			case StateOfNode.uncompleted:
				char_state = '0';
				break;
			case StateOfNode.completed:
				char_state = '1';
				break;
			case StateOfNode.dontcare:
				char_state = '2';
				break;
			default:
				char_state = '0';
				break;
			}

			for (int i=0; i<this.Level; i++) {
				string_level += '-';
			}

			NodeToString = (string_level + string_ID + '[' + this.name + ',' + char_state + ']');
			return NodeToString;
		}

	}
}

