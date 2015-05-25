using System;
using System.Collections.Generic;
using System.Collections;

public class GraphHistory {

	private Dictionary<DateTime, List<Emotion>> graphs = new Dictionary<DateTime, List<Emotion>>();

	public void AddGraph(GraphData data)
	{
		if (data.date == DateTime.Now)
			UpdateGraph (data);

		graphs.Add (data.date, data.emotions);
	}

	public void RemoveGraph()
	{
	}

	public void UpdateGraph(GraphData data)
	{
		graphs [data.date] = data.emotions;
	}

	public List<Emotion> GetGraphEmotions(DateTime date)
	{
		if(graphs.ContainsKey(date))
			return graphs[date];

		return null;

	}
}
