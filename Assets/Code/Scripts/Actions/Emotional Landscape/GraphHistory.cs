using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;

public class GraphHistory {

	private List<GraphData> graphData = new List<GraphData> ();
	public string directory;
	private FileStream fs;

	public GraphHistory(string location)
	{
		this.directory = location;
		if (!File.Exists (directory))
		{
			File.Create(directory);
			Debug.Log ("XML Created in : " + directory);
		}
	}

	//Save the data displayed on the graph
	public void Save(GraphData data)
	{
		graphData = Load ();	//Load in the current set of emotions
		for(int i = 0; i < graphData.Count;i++)
		{
			GraphData testData = graphData[i];
			//needs to be changed to test D-M-Y
			//if the data for a day alrady exists then swap it out
			if(testData.date == DateTime.Today)
				graphData[i] = data;
		}
		//If the data isn't recorded then record it
		if (!graphData.Contains (data))
			graphData.Add (data);

		//Open the file stream and save the data to the directory
		fs = new FileStream (directory, FileMode.Create);
		XmlSerializer serializer = new XmlSerializer (typeof(List<GraphData>));
		serializer.Serialize (fs, graphData);
		fs.Close ();
	}

	public List<GraphData> Load()
	{
		fs = new FileStream (directory, FileMode.Open);
		List<GraphData> allGraphs = new List<GraphData> ();

		try
		{
			XmlSerializer serializer = new XmlSerializer (typeof(List<GraphData>));
			List<GraphData> data = serializer.Deserialize (fs) as List<GraphData>;
			allGraphs = data;
		}

		catch
		{
			fs.Close();
			return new List<GraphData>();
		}
		fs.Close ();
		return allGraphs;
	}
}
