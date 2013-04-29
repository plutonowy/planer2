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
		public List<Node> subNodesList; // lista podcelow danego wezla np dla wezla DOM -> lista: okno, drzwi, dach
		public UInt16 level; // poziom zaglębienia w pliku
		public string filePath; //sciezka do pliku

		//private:
		private StreamReader reader;
		//private StreamWriter writer;
		private UInt32 maxNodeID; // aktualny najwyzszy ID node (potrzebne do tworzenia nowych wezlow i nadawania im ID)

// methods:
		//public:
		public Model ()
		{
			subNodesList = new List<Node>();
			prevNodesStack = new List<Node>();
			level=0;
			filePath=null;
			currentNode=null;
			LoadSubNodesList(0);
		}

		public Model (string path) // ustawia path odrazu przy wywołaniu;
		{
			subNodesList = new List<Node>();
			prevNodesStack = new List<Node>();
			level=0;
			filePath=path;
			currentNode=null;
			reader = new StreamReader(this.filePath);
			LoadSubNodesList(0);
		}

		public void SetFilePath(string path)
		{
			this.filePath = path;
			this.reader = new StreamReader(this.filePath);
			this.currentNode = null;
			this.prevNodesStack = new List<Node>();
			this.subNodesList = new List<Node>();
			this.level = 0;
			this.maxNodeID = FindMaxNodeID();
		}

		public void LoadSubNodesList () // PRZETESTOWAĆ!!!
		{
			string line;
			long remPosition = reader.BaseStream.Position; // zapamiętanie by pod koniec operacji ustawić reader spowrotem na tej samej pozycji
			//zabespieczyć reader przed brakiem sciezki
			SetReaderOn(currentNode.nodeID); // zabezpieczyć przed wart null

			subNodesList.Clear();

			while ((line=reader.ReadLine()) != null) 
			{

				if(CountChars(line, '-') == level+1)
				{
					string newNodeName = GetName(line); //wczytuje nazwe z pliku
					UInt32 newNodeID = GetID(line); // wczytuje ID z pliku
					StateOfNode newNodeState = GetState(line); // wczytuje stan z pliku

					Node newNodeFromFile = new Node(newNodeName, newNodeState, newNodeID); // odtwarza w pamieci wezel zapisany w pliku 

					subNodesList.Add(newNodeFromFile); //dodaje wezel do listy
				}
				else if(CountChars(line, '-') > level+1) continue; //currentLvl+1 chodzi o poziom niżej od węzła currentNode
				else if(CountChars(line,'-') < level+1) break;
			}
			reader.BaseStream.Position = remPosition;
		}

		public void LoadSubNodesList (int lvl) // PRZETESTOWAĆ!!||przeladowanie robi tosamo z tym ze wpisuje WSZYSTKIE nazwy podwezłow (z danego poziomu lvl) do currentSubNodesList
		{
			long remPosition = reader.BaseStream.Position; // zabezpieczyć przed reader bez sciezki
			reader.BaseStream.Position = 0; // tutaj przeszukiwany jest caly plik

			string line;
			subNodesList.Clear();

			while((line = reader.ReadLine()) != null)
			{
				if(CountChars(line,'-') == lvl)
				{
					string newNodeName = GetName(line); //wczytuje nazwe z pliku
					UInt32 newNodeID = GetID(line); // wczytuje ID z pliku
					StateOfNode newNodeState = GetState(line); // wczytuje stan z pliku

					Node newNodeFromFile = new Node(newNodeName, newNodeState, newNodeID); // odtwarza w pamieci wezel zapisany w pliku 

					subNodesList.Add(newNodeFromFile); //dodaje wezel do listy
				}
			}

			reader.BaseStream.Position = remPosition; // ustawia reader tak jak był przed wywołaniem funkcji
		}

		public void LoadNode (UInt32 ID) // przetestować
		{
			string line;
			reader.BaseStream.Position = 0; //zabezpieczyć przed reader bez sciezki do pliku
			bool found = false;

			foreach (Node n in subNodesList) { // sprawdzanie czy zadany id znajduje się na liscie podwęzlów
				if(ID == n.nodeID) {
					found = true;
					break;}}

			if(found) // jesli id znajduje sie na liscie to wyszukaj go w pliku
			{
				while ((line = reader.ReadLine()) != null) 
				{
					if (ID == GetID (line)) // jeśli znalazł do ładuje parametry węzła z pliku do pamięci operacyjnej jako currentNode
					{
						Node node = new Node (GetName (line), GetState (line), ID);
						if (currentNode != null)
						prevNodesStack.Add (currentNode);
						currentNode = node;
						level = Convert.ToUInt16 (CountChars (line, '-'));
						LoadSubNodesList ();
						break;
					}
				}
			}

			reader.BaseStream.Position = 0;
		}

		public void LoadNode (string NodeName)//przetestować // dla podanej nazwy wezla przeszukuje currentSubNodesList 
		{
			if (subNodesList.Count <= 0)
				return;

			foreach (Node nod in subNodesList) 
			{
				if(NodeName == nod.name)
				{
					LoadNode(nod.nodeID);
					break;
				}
			}
		}

		public void GoBack () //wstępne testy OK
		{
			if (prevNodesStack.Count < 1) {
				level = 0;
				currentNode = null;
				prevNodesStack.Clear();
				reader.BaseStream.Position = 0; // ustawienie readera na początek pliku (nie pytaj po co)
				LoadSubNodesList (0);
			} else {
				int indexOfLast = prevNodesStack.Count-1;
				currentNode = prevNodesStack[indexOfLast];
				prevNodesStack.RemoveAt(indexOfLast);
				level--;
				LoadSubNodesList();
				//LoadNode(currentNode.nodeID);
			}
		}

		public void HardLoadNode (UInt32 ID) // dokonczyc!!!!!!!
		{
			string line;
			long remPosition = reader.BaseStream.Position;
			reader.BaseStream.Position = 0;

			while ((line = reader.ReadLine()) != null) 
			{
				if(ID == GetID(line))
				{
					Stack<UInt32> idStack = new Stack<uint>();
					idStack = GetPrevNodesStack(ID);

					if(idStack.Count > 0)
					{
						prevNodesStack.Clear();


					}
				}
			}

			reader.BaseStream.Position = remPosition;
		}

		//private:
		private int CountTabs(string strLine) // przypadek szczegolny metody CountChars
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

		private UInt16 GetLvl (string line)
		{
			return Convert.ToUInt16(CountChars(line, '-'));
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

		private void SetReaderOn (UInt32 ID) // ustawia reader w pliku na koniec lini w kotrej z węzłem o podanym ID 
		{
			reader.BaseStream.Position = 0; //zabezpieczyc przed reader bez sciezki do pliku

			string line;

			while ((line = reader.ReadLine()) != null) 
			{
				if(GetID(line) == ID) return;
			}

			reader.BaseStream.Position = 0;
		}

		private void SetReaderOn (Node nod) // overload, now you can send Node object like a argument
		{
			reader.BaseStream.Position = 0;//zabezpieczyc przed reader bez sciezki do pliku

			string line;

			while ((line = reader.ReadLine()) != null) 
			{
				if(GetID(line) == nod.nodeID) return;
			}

			reader.BaseStream.Position = 0;
		}

		private UInt32 FindMaxNodeID ()
		{
			long remPosition = reader.BaseStream.Position;
			//zabezpieczyc przed reader bez sciezki do pliku
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

			reader.BaseStream.Position = remPosition;

			return maxID;
		}

		private Stack<UInt32> GetPrevNodesStack (UInt32 ID) //przetestowane wstepnie
		{
			Stack<UInt32> stackID = new Stack<UInt32> ();
			//zabezpieczyc przed reader bez sciezki do pliku
			long remPosition = reader.BaseStream.Position;
			reader.BaseStream.Position = 0;
			string prevLine = reader.ReadLine ();
			string line = prevLine;

			if (GetID (line) == ID) {
				reader.BaseStream.Position = remPosition;
				return stackID;}

			while ((line = reader.ReadLine()) != null) 
			{
				if(GetLvl(line) > GetLvl(prevLine))
				{
					stackID.Push(GetID(prevLine));
				}
				else if(GetLvl(line) < GetLvl(prevLine))
				{
					if(stackID.Count >= 1) stackID.Pop();
				}
			
				if(GetID(line) == ID){
					reader.BaseStream.Position = remPosition;
					return stackID;}
			
				prevLine = line; 
			}

			reader.BaseStream.Position = remPosition;
			return stackID;
		}
	}
}