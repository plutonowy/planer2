using System;

namespace planerConsole_1
{
	[Serializable]
	public class NodeNotFoundExcepion : Exception
	{
		public NodeNotFoundExcepion (string message) : base(message)
		{ }
	}

	[Serializable]
	public class CommandNotFoundException : Exception
	{
		public CommandNotFoundException (string message) : base(message)
		{ }
	}

	[Serializable]
	public class CommandCantBeCompletedException : Exception
	{
		public CommandCantBeCompletedException (string message) : base(message)
		{ }
	}


}
