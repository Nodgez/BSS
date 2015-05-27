using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;

public class GraphHistory {

	private Dictionary<DateTime, List<EmotionDisplay>> graphs = new Dictionary<DateTime, List<EmotionDisplay>>();
	public string directory;

	public GraphHistory(string location)
	{
		this.directory = Directory.GetCurrentDirectory () + location;
		if (!File.Exists (directory))
			File.Create(directory);
	}

	public void AddGraph(GraphData data)
	{
		if (data.date == DateTime.Now)
			UpdateGraph (data);

		//graphs.Add (data.date, data.emotions);
	}

	public void RemoveGraph()
	{
	}

	public void UpdateGraph(GraphData data)
	{
		//graphs [data.date] = data.emotions;
	}

	public void Save(GraphData data)
	{
		FileStream fs = new FileStream (directory, FileMode.Create);
		XmlSerializer serializer = new XmlSerializer (typeof(GraphData));
		serializer.Serialize (fs, data);
		fs.Close ();
	}

	public GraphData Load(string location)
	{
		string path = Directory.GetCurrentDirectory () + location;
		FileStream fs = new FileStream (path,FileMode.Open);
		XmlSerializer serializer = new XmlSerializer (typeof(GraphData));
		GraphData data = serializer.Deserialize (fs) as GraphData;
		return data;
	}

	public List<EmotionDisplay> GetGraphEmotions(DateTime date)
	{
		if(graphs.ContainsKey(date))
			return graphs[date];

		return null;

	}
}
