using System;
using System.Collections.Generic;
using System.IO;

namespace planerConsole_1
{
	public class Model
	{

// variables

		private string filePath;
		private readonly string tmpFilePath = @"tmpFile.txt";
		private StreamReader reader; // pozbyć się writera i readera -> tworzyć je za każdym razem gdy są operacje na plikach
		private StreamWriter writer; // porawność sciezki sprawdzać File.Exist(path)


// methods:
		public Model ()
		{
			filePath=null;
		}

		public Model (string path) // ustawia path odrazu przy wywołaniu;
		{
			if(!File.Exists(path)) throw new FileNotFoundException(path);

			filePath = path;
			this.reader = new StreamReader (this.filePath);
			this.writer = new StreamWriter (this.tmpFilePath, false);
		}

		public void SetFilePath(string path)
		{
			if(!File.Exists(path)) throw new FileNotFoundException(path);

			this.filePath = path;
			this.reader = new StreamReader(this.filePath);
			this.writer = new StreamWriter (this.tmpFilePath, false);
		}

		public string FilePath ()
		{
			return this.filePath;
		}

		public List<Node> GetSubNodesList (UInt32 ID) // PRZETESTOWAĆ!!!
		{
			long remPosition = reader.BaseStream.Position; // zapamiętanie by pod koniec operacji ustawić reader spowrotem na tej samej pozycji
			//zabespieczyć reader przed brakiem sciezki
			reader.DiscardBufferedData(); 
			SetReaderOn(ID); // zabezpieczyć przed wart null
			string line;
			List<Node> subNodesList = new List<Node>();

			int subLevel = GetNode(ID).Level+1; // level podwięzłów węzła ID

			while ((line=reader.ReadLine()) != null) 
			{
				if(ParseLevel(line) == subLevel)
				{
					Node newNode = ParseNode(line); // odtwarza w pamieci wezel zapisany w pliku 
					subNodesList.Add(newNode); //dodaje wezel do listy
				}
				else if(ParseLevel(line) > subLevel) continue; //currentLvl+1 chodzi o poziom niżej od węzła currentNode
				else if(ParseLevel(line) < subLevel) break;
			}

			reader.BaseStream.Position = remPosition;
			return subNodesList;
		}

		public List<Node> GetNodesList (int level) // PRZETESTOWAĆ!!||przeladowanie robi tosamo z tym ze wpisuje WSZYSTKIE nazwy podwezłow (z danego poziomu lvl) do currentSubNodesList
		{
			long remPosition = reader.BaseStream.Position; // zabezpieczyć przed reader bez sciezki
			reader.BaseStream.Position = 0; // tutaj przeszukiwany jest caly plik

			string line;
			List<Node> subNodesList = new List<Node>();
			reader.DiscardBufferedData();
			while((line = reader.ReadLine()) != null)
			{
				if(ParseLevel(line) == level)
				{
					Node newNode = ParseNode(line); // odtwarza w pamieci wezel zapisany w pliku 
					subNodesList.Add(newNode); //dodaje wezel do listy
				}
			}

			reader.BaseStream.Position = remPosition; // ustawia reader tak jak był przed wywołaniem funkcji
			return subNodesList;
		}

		public Node GetNode (UInt32 ID)
		{
			long remPosition = reader.BaseStream.Position;
			reader.BaseStream.Position = 0;

			string line;
			reader.DiscardBufferedData();
			while ((line = reader.ReadLine()) != null) 
			{
				if(ID == ParseID(line)){
					reader.BaseStream.Position = remPosition;
					return ParseNode(line);
				}
			}

			reader.BaseStream.Position = remPosition;
			return null;
		}

		public List<Node> GetPrevNodesList (UInt32 ID) //przetestowane wstepnie
		{
			Stack<Node> stackNode = new Stack<Node> ();

			long remPosition = reader.BaseStream.Position; //zabezpieczyc przed reader bez sciezki do pliku

			reader.DiscardBufferedData();
			reader.BaseStream.Position = 0;
			string prevLine = reader.ReadLine ();
			string line = prevLine;

			List<Node> prevNodesList = new List<Node> ();

			if (ParseID (line) == ID) {
				reader.BaseStream.Position = remPosition;
				return prevNodesList;
			}
			while ((line = reader.ReadLine()) != null) 
			{
				if (ParseLevel (line) > ParseLevel (prevLine)) 
				{
					stackNode.Push (ParseNode (prevLine));
				} 
				else if (ParseLevel (line) < ParseLevel (prevLine)) 
				{
					if (stackNode.Count >= 1)
						stackNode.Pop ();
				}
			
				if (ParseID (line) == ID) 
					break;
			
				prevLine = line; 
			}

			reader.BaseStream.Position = remPosition;

			while (stackNode.Count > 0) {
				prevNodesList.Add(stackNode.Pop());
			}

			prevNodesList.Reverse();

			reader.BaseStream.Position = remPosition;
			return prevNodesList;
		}

		public void Save (Node NodeToSave)
		{
			reader.BaseStream.Position = 0;

			string line;
			reader.DiscardBufferedData();
			while ((line = reader.ReadLine()) != null) 
			{
				if (ParseID(line) == NodeToSave.GetID()) 
				{
					line = NodeToSave.ToString ();
				}

				writer.WriteLine (line);
			}

			reader.BaseStream.Position=0;

			ReloadStreams();
		}

		public void Delete (UInt32 ID)
		{
			reader.BaseStream.Position = 0;

			string line;
			reader.DiscardBufferedData ();
			while ((line = reader.ReadLine()) != null) 
			{
				if(ParseID(line) != ID)
					writer.WriteLine(line);
			}

			ReloadStreams();
		}

		public void NewNode (UInt32 ParentID, Node NewNodeToSave)
		{
			reader.BaseStream.Position = 0;
			//writer.BaseStream.Position = 0;

			NewNodeToSave.SetID(AssignNewID());
			NewNodeToSave.Level = GetLevel(ParentID) + 1;
			string newLineToWrite = NewNodeToSave.ToString ();

			string line;

			reader.DiscardBufferedData();
			while ((line = reader.ReadLine()) != null) 
			{
				writer.WriteLine(line);

				if(ParseID(line) == ParentID)
				{
					writer.WriteLine(newLineToWrite);
				}
			}

			ReloadStreams();
		}

		public void NewNode (bool First, Node NewNodeToSave)
		{
			NewNodeToSave.Level=0;
			if(reader.BaseStream.Length != 0) NewNodeToSave.SetID(AssignNewID());
			else NewNodeToSave.SetID(0);

			string lineToWrite = NewNodeToSave.ToString();

			string line;
			reader.BaseStream.Position = 0;
			reader.DiscardBufferedData();
			writer.BaseStream.Position=0;

			writer.WriteLine(lineToWrite);

			while ((line = reader.ReadLine()) != null) 
			{
				writer.WriteLine(line);
			}

			ReloadStreams();
		}

		public float GetProgres(UInt32 ID)
		{
			float progress=0;
			int completed = CountSubTreeElements(ID, StateOfNode.completed);
			int dontcare = CountSubTreeElements(ID, StateOfNode.dontcare);
			int all = CountSubTreeElements(ID);
			progress = ( ( completed+dontcare )/all );
			return progress;
		}

		public void Close ()
		{
			File.Delete(this.tmpFilePath);
			reader.Close();
			writer.Close();
		}

	//private:
		//parsery:
		private int CountChars (string strLine, char value) // alternatywa dla ContTabs
		{
			if(!strLine.Contains("[")) return -1; // jeśli linia nie zawiera znaku'[' to uznaje ze jest błędna albo pusta

			int count = 0;

			foreach (char c in strLine) {
				if(c==value) count++;
			}

			return count;
		}
		private int ParseLevel (string line)
		{
			return CountChars(line,'-');
		}
		private Node ParseNode (string line)
		{
			Node newNode = new Node();

			newNode.name = ParseName(line);
			newNode.SetID(ParseID(line));
			newNode.state = ParseState(line);
			newNode.Level = ParseLevel(line);

			return newNode;
		}
		private string ParseName(string line) // zwraca nazwe węzła (parsuje linie dokopując się do nazwy ) (pomija zbedna znaki)
		{
			char[] parseChars = {'\t', '-', ' '}; // pomijane znaki
			line = line.Trim(parseChars);
			int start = line.IndexOf('[') + 1 ; // +1 bo chcemy od pierwszego znaku za [
			int end = line.IndexOf(',');

			if(start>0 && end>0)return line.Substring(start, (end-start));
			else return null;

		}
		private UInt32 ParseID (string line)
		{
			char[] parseChars = {'\t', '-', ' '};
			line = line.Trim(parseChars);

			int end = line.IndexOf('[');

			if(end>0) return UInt32.Parse(line.Substring(0,end));
			else return 0;
		}
		private StateOfNode ParseState (string line)
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
				if(ParseID(line) == ID) return;
			}

			reader.BaseStream.Position = 0;
		}

		private void SetReaderOn (Node nod) // overload, now you can send Node object like a argument
		{
			reader.BaseStream.Position = 0;//zabezpieczyc przed reader bez sciezki do pliku

			string line;

			while ((line = reader.ReadLine()) != null) 
			{
				if(ParseID(line) == nod.GetID()) return;
			}

			reader.BaseStream.Position = 0;
		}

		private int GetLevel (UInt32 ID)
		{
			long remPosition = reader.BaseStream.Position;

			reader.BaseStream.Position = 0;

			string line;
			while ((line = reader.ReadLine()) != null) 
			{
				if(ParseID(line) == ID)
				{
					reader.BaseStream.Position = remPosition;
					return ParseLevel(line);
				}
			}

			reader.BaseStream.Position = remPosition;
			return 0;
		}

		private UInt32 FindMaxNodeID ()
		{
			long remPosition = reader.BaseStream.Position; //zabezpieczyc przed reader bez sciezki do pliku
			
			reader.BaseStream.Position = 0;
			string line;
			UInt32 maxID = 0;

			while ((line = reader.ReadLine()) != null) 
			{
				if(ParseID(line) >= maxID)
				{
					maxID = ParseID(line);
				}
			}

			reader.BaseStream.Position = remPosition;
			return maxID;
		}

		private UInt32 AssignNewID ()
		{
			return FindMaxNodeID() + 1;
		}

		private void ReloadStreams ()
		{
			string line;

			reader.Close ();
			writer.Close ();
			reader = new StreamReader (this.tmpFilePath);
			writer = new StreamWriter (this.filePath, false);

			while ((line=reader.ReadLine()) != null) 
			{
				writer.WriteLine(line);
			}
			reader.Close();
			writer.Close();

			reader = new StreamReader(this.filePath);
			writer = new StreamWriter(this.tmpFilePath, false);
		}

		private int CountSubTreeElements (UInt32 headID)
		{
			int currentlevel = GetLevel (headID);

			long remPosition = reader.BaseStream.Position;
			SetReaderOn (headID);
			string line;

			int count=0;

			while ((line=reader.ReadLine()) != null) 
			{
				if(ParseLevel(line) > currentlevel)
				{
					count++;
				}
				else break;
			}

			reader.BaseStream.Position = remPosition;
			return count;
		}

		private int CountSubTreeElements(UInt32 headID, StateOfNode state)
		{
			int currentlevel = GetLevel (headID);

			long remPosition = reader.BaseStream.Position;
			SetReaderOn (headID);
			string line;

			int count=0;

			while ((line=reader.ReadLine()) != null) 
			{
				if((ParseLevel(line) > currentlevel) && ParseState(line) == state)
				{
					count++;
				}
				else break;
			}

			reader.BaseStream.Position = remPosition;
			return count;
		}

		public void ChangeStateOfAllSubNodes (UInt32 headID, StateOfNode fromState, StateOfNode toState)
		{
			int currentLevel = GetLevel (headID);
			long remPositoin = reader.BaseStream.Position;
			SetReaderOn (headID);
			string line;

			List<UInt32> lista = new List<uint>();

			while ((line = reader.ReadLine()) != null) 
			{
				if(ParseLevel(line) > currentLevel)
				{
					if(ParseState(line) == fromState)
						lista.Add(ParseID(line));
				}
				else break;
			}


			foreach(UInt32 currID in lista)
			{
				Node nod = GetNode(currID);
				nod.state = toState;
				Save(nod);
			}

			reader.BaseStream.Position = remPositoin;
		}
	}
}