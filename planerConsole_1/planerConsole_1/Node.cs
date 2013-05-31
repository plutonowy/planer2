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
		private UInt32 ID;
		public int Level;

		public Node ()
		{
			this.name = null;
			this.state = StateOfNode.uncompleted;
		}

		public Node (string newName, StateOfNode newState, UInt32 newID ,int newLevel)
		{
			this.name = newName;
			this.state = newState;
			this.Level = newLevel;
			this.ID = newID;
		}

		public Node (string newName, StateOfNode newState, int newLevel)
		{
			this.name = newName;
			this.state = newState;
			this.Level = newLevel;
		}

		public Node (string newName, StateOfNode newState)
		{
			this.name = newName;
			this.state = newState;
		}

		public Node (string newName)
		{
			this.name = newName;
			this.state = StateOfNode.uncompleted;
		}

		public void SetID(UInt32 newNodeID)
		{
			this.ID = newNodeID;
		}

		public UInt32 GetID ()
		{
			return this.ID;
		}

		public override string ToString ()
		{
			string NodeToString;

			string string_ID = this.ID.ToString ();
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
			//===================DO WYJEBANIA===================== tylko wizualny komfort pliku-> do NICZEGO więcej
			for (int i=0; i<this.Level; i++) {
				string_level += '\t';
			}
			//===================DO WYJEBANIA=====================
			for (int i=0; i<this.Level; i++) {
				string_level += '-';
			}

			NodeToString = (string_level + string_ID + '[' + this.name + ',' + char_state + ']');
			return NodeToString;
		}

	}
}

