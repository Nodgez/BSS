using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;

public class GraphHistory {

	private List<GraphData> graphData = new List<GraphData> ();
	public string directory;

	public GraphHistory(string location)
	{
		this.directory = Directory.GetCurrentDirectory () + location;
		if (!File.Exists (directory))
			File.Create(directory);
	}

	public void Save(GraphData data)
	{
		FileStream fs = new FileStream (directory, FileMode.Create);
		XmlSerializer serializer = new XmlSerializer (typeof(GraphData));
		serializer.Serialize (fs, data);
		fs.Close ();
	}

	public List<GraphData> Load()
	{
		List<GraphData> allGraphs = new List<GraphData> ();
		string[] files = Directory.GetFiles (Directory.GetCurrentDirectory (), "*.XML");
		for(int i = 0; i < files.Length;i++)
		{
			string path = files[i];
			FileStream fs = new FileStream (path,FileMode.Open);
			XmlSerializer serializer = new XmlSerializer (typeof(GraphData));
			GraphData data = serializer.Deserialize (fs) as GraphData;
			allGraphs.Add(data);
		}
		return allGraphs;
	}
}
