using System;
using System.Collections.Generic;
using System.IO;

namespace planerConsole_1
{
	public class View
	{
		public View ()
		{
	
		}
		static public void WriteMessage(string txt)
		{
			Console.WriteLine(txt);
		}

		static public void DisplaySubNodesList (List<Node> subNodes)
		{
			foreach (Node temp in subNodes) 
			{
				Console.Write("{0}({1})\t",temp.name, temp.state);
			}
			Console.WriteLine();
		}

		static public void DisplayPrevSubNodesList(List<Node> prevNodes)
		{
			foreach (Node temp in prevNodes) 
			{
				Console.Write("{0}({1})\t",temp.name, temp.state);
			}
			Console.WriteLine();
		}

	}
}

