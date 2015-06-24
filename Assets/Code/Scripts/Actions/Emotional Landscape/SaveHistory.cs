using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;

public class SaveHistory {

	private List<SaveData> saveData = new List<SaveData> ();
	public string directory;
	private FileStream fs;

	public SaveHistory(string location)
	{
		this.directory = location;
		if (!File.Exists (directory))
		{
			File.Create(directory);
			Debug.Log ("XML Created in : " + directory);
		}
	}

	//Save the data displayed on the graph
	public void Save(SaveData data)
	{
		saveData = Load ();	//Load in the current set of emotions
		for(int i = 0; i < saveData.Count;i++)
		{
			SaveData testData = saveData[i];
			//if the data for a day alrady exists then swap it out
			if(testData.date == DateTime.Today)
				saveData[i] = data;
		}
		//If the data isn't recorded then record it
		if (!saveData.Contains (data))
			saveData.Add (data);

		//Open the file stream and save the data to the directory
		fs = new FileStream (directory, FileMode.Create);
		XmlSerializer serializer = new XmlSerializer (typeof(List<SaveData>));
		serializer.Serialize (fs, saveData);
		fs.Close ();
	}

	public void Save(List<SaveData> data)
	{
		//Open the file stream and save the data to the directory
		fs = new FileStream (directory, FileMode.Create);
		XmlSerializer serializer = new XmlSerializer (typeof(List<SaveData>));
		serializer.Serialize (fs, data);
		fs.Close ();
	}

	public List<SaveData> Load()
	{
		fs = new FileStream (directory, FileMode.Open);
		List<SaveData> allGraphs = new List<SaveData> ();

		try
		{
			XmlSerializer serializer = new XmlSerializer (typeof(List<SaveData>));
			List<SaveData> data = serializer.Deserialize (fs) as List<SaveData>;
			allGraphs = data;
		}

		catch
		{
			fs.Close();
			return new List<SaveData>();
		}
		fs.Close ();
		return allGraphs;
	}

	public SaveData GetTodayData()
	{
		foreach (var data in saveData) {
			if(data.date == DateTime.Today)
				return data;
		}
		return new SaveData(new List<Emotion>(), Vector2.one * 0.5f);
	}
	
	public SaveData GetDataAtDate(DateTime date)
	{
		foreach (var data in saveData) {
			if(data.date == date)
				return data;
		}
		return GetTodayData();
	}
}
