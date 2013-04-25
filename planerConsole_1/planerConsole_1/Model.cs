using System;
using System.Collections.Generic;
using System.IO;

namespace planerConsole_1
{
	public class Model
	{
// variables
		//public:
		public Node currentNode; // przechowuje aktualny wezel
		public List<Node> prevNodesStack;  // przechowuje wezly przodkow (stos przodkow (pierwszy element = korzen | ostatni = ojciec))
		public List<Node> currentSubNodesList; // lista podcelow danego wezla np dla wezla DOM -> lista: okno, drzwi, dach
		public UInt16 currentLvl; // poziom zaglębienia w pliku
		public string currentPath; //sciezka do pliku

		//private:
		private StreamReader reader;
		//private StreamWriter writer;
		private UInt32 nodeMaxID; // aktualny najwyzszy ID node (potrzebne do tworzenia nowych wezlow i nadawania im ID)

// methods:
		//public:
		public Model ()
		{
			currentSubNodesList = new List<Node>();
			prevNodesStack = new List<Node>();
			currentLvl=0;
			currentPath=null;
			currentNode=null;
			LoadCurrentSubNodesList(0);
		}

		public Model (string path) // ustawia path odrazu przy wywołaniu;
		{
			currentSubNodesList = new List<Node>();
			prevNodesStack = new List<Node>();
			currentLvl=0;
			currentPath=path;
			currentNode=null;
			reader = new StreamReader(this.currentPath);
			LoadCurrentSubNodesList(0);
		}

		public void SetPath(string path)
		{
			this.currentPath = path;
			reader = new StreamReader(this.currentPath);
		}

		public void LoadCurrentSubNodesList () // PRZETESTOWAĆ!!!
		{
			string line;
			//line = reader.ReadLine ();
			bool first = true;
			int lvl=0;
			long remPosition = reader.BaseStream.Position; // zapamiętanie by pod koniec operacji ustawić reader spowrotem na tej samej pozycji

			currentSubNodesList.Clear();

			while ((line=reader.ReadLine()) != null) 
			{
				if(first)
				{
					lvl = CountChars(line, '-');
					first = false;
				}

				if(CountChars(line, '-') == lvl)
				{
					string newNodeName = GetName(line); //wczytuje nazwe z pliku
					UInt32 newNodeID = GetID(line); // wczytuje ID z pliku
					StateOfNode newNodeState = GetState(line); // wczytuje stan z pliku

					Node newNodeFromFile = new Node(newNodeName, newNodeState, newNodeID); // odtwarza w pamieci wezel zapisany w pliku 

					currentSubNodesList.Add(newNodeFromFile); //dodaje wezel do listy
				}
				else if(CountChars(line, '-') > lvl) continue;
				else if(CountChars(line,'-') < lvl) break;
			}
			reader.BaseStream.Position = remPosition;
		}

		public void LoadCurrentSubNodesList (int lvl) // PRZETESTOWAĆ!!||przeladowanie robi tosamo z tym ze wpisuje WSZYSTKIE nazwy podwezłow (z danego poziomu lvl) do currentSubNodesList
		{
			long remPosition = reader.BaseStream.Position;
			reader.BaseStream.Position = 0; // tutaj przeszukiwany jest caly plik

			string line;
			currentSubNodesList.Clear();

			while((line = reader.ReadLine()) != null)
			{
				if(CountChars(line,'-') == lvl)
				{
					string newNodeName = GetName(line); //wczytuje nazwe z pliku
					UInt32 newNodeID = GetID(line); // wczytuje ID z pliku
					StateOfNode newNodeState = GetState(line); // wczytuje stan z pliku

					Node newNodeFromFile = new Node(newNodeName, newNodeState, newNodeID); // odtwarza w pamieci wezel zapisany w pliku 

					currentSubNodesList.Add(newNodeFromFile); //dodaje wezel do listy
				}
			}

			reader.BaseStream.Position = remPosition; // ustawia reader tak jak był przed wywołaniem funkcji
		}

		public void LoadNode (UInt32 ID) // przetestować
		{
			string line;
			reader.BaseStream.Position = 0;

			while ((line = reader.ReadLine()) != null) 
			{
				if(ID == GetID(line))
				{
					Node node = new Node(GetName(line), GetState(line), ID);
					if(currentNode != null) prevNodesStack.Add(currentNode);
					currentNode = node;
					currentLvl = Convert.ToUInt16(CountChars(line,'-'));
					LoadCurrentSubNodesList();
					break;
				}
			}
			reader.BaseStream.Position = 0;
		}

		public void LoadNode (string NodeName)//przetestować // dla podanej nazwy wezla przeszukuje currentSubNodesList 
		{
			if (currentSubNodesList.Count <= 0)
				return;

			foreach (Node nod in currentSubNodesList) 
			{
				if(NodeName == nod.name)
				{
					LoadNode(nod.nodeID);
					break;
				}
			}
		}

		public void GoBack () // przetestować!!!
		{
			if (prevNodesStack.Count < 1) {
				currentLvl = 0;
				currentNode = null;
				prevNodesStack.Clear();
				LoadCurrentSubNodesList (0);
			} else {
				int indexOfLast = prevNodesStack.Count-1;
				currentNode = prevNodesStack[indexOfLast];
				prevNodesStack.RemoveAt(indexOfLast);
				SetReaderOn(currentNode.nodeID);
				currentLvl--;
				LoadCurrentSubNodesList();
				//LoadNode(currentNode.nodeID);
			}
		}

		//private:
		private int CountTabs(string strLine) // przypadek szczegolny metdoy CountChars
		{
			if(!strLine.Contains("[")) return -1; // jeśli linia nie zawiera znaku'[' to uznaje ze jest błędna albo pusta

			int count = 0;

			foreach(char c in strLine) 
			{
				if(c=='\t') count++;
			}
	
			return count;
		}

		private int CountChars (string strLine, char value) // alternatywa dla ContTabs
		{
			if(!strLine.Contains("[")) return -1; // jeśli linia nie zawiera znaku'[' to uznaje ze jest błędna albo pusta

			int count = 0;

			foreach (char c in strLine) 
			{
				if(c==value) count++;
			}

			return count;
		}

		private string GetName(string line) // zwraca nazwe węzła (parsuje linie dokopując się do nazwy ) (pomija zbedna znaki)
		{
			char[] parseChars = {'\t', '-', ' '}; // pomijane znaki
			line = line.Trim(parseChars);
			int start = line.IndexOf('[') + 1 ; // +1 bo chcemy od pierwszego znaku za [
			int end = line.IndexOf(',');

			if(start>0 && end>0)return line.Substring(start, (end-start));
			else return null;

		}

		private UInt32 GetID (string line)
		{
			char[] parseChars = {'\t', '-', ' '};
			line = line.Trim(parseChars);

			int end = line.IndexOf('[');

			if(end>0) return UInt32.Parse(line.Substring(0,end));
			else return 0;
		}

		private StateOfNode GetState (string line)
		{
			char[] parseChars = {'\t', '-', ' '};

			line = line.Trim (parseChars);
			line = line.Replace(" ", "");

			int start = line.IndexOf (',') + 1; // zaraz za znakiem ','
			int length = line.IndexOf (']') - start;

			if (start > 0 && length > 0) 
			{
				int wynik = int.Parse(line.Substring(start,length));

				switch (wynik)
				{
				case 0:
					return StateOfNode.uncompleted;
				case 1:
					return StateOfNode.completed;
				case 2:
					return StateOfNode.dontcare;
				default:
					return StateOfNode.uncompleted;
				}
			}

			return StateOfNode.uncompleted;
		}

		private void SetReaderOn (UInt32 ID)
		{
			reader.BaseStream.Position = 0;

			string line;

			while ((line = reader.ReadLine()) != null) 
			{
				if(GetID(line) == ID) return;
			}
		}

		private void SetReaderOn (Node nod) // overload, now you can send Node object like a argument
		{
			reader.BaseStream.Position = 0;

			string line;

			while ((line = reader.ReadLine()) != null) 
			{
				if(GetID(line) == nod.nodeID) return;
			}
		}

		private UInt32 FindNodeMaxID ()
		{
			reader.BaseStream.Position = 0;
			string line;
			UInt32 maxID = 0;

			while ((line = reader.ReadLine()) != null) 
			{
				if(GetID(line) >= maxID)
				{
					maxID = GetID(line);
				}
			}

			reader.BaseStream.Position = 0;

			return maxID;
		}

	}
}

