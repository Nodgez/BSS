using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class HistoryMenu : Menu {

	public Button historyButton;
	public HistoryButtonLayout historyDisplay;
	public List<SaveData> graphCollection;
	public DateTime dateOnDisplay;
	
	protected static GraphHistory graphHistory;
	protected HistoryButtonLayout historyLayoutInstance;
	
	protected override void Start () 
	{
		//create a new graph history pointing to the location where the data is stroed
		if(graphHistory == null)
			graphHistory = new GraphHistory (Application.persistentDataPath + "//Graph.xml");
		graphCollection = graphHistory.Load();
		
		bool hasDataForToday = false;
		for(int i = graphCollection.Count -1; i > -1; i--)
		{
			if(graphCollection[i].date == DateTime.Today){
				hasDataForToday = true;
			}
			if(graphCollection[i].emotions.Count <= 0)
				graphCollection.RemoveAt(i);
		}
		if(!hasDataForToday){
			SaveData data = new SaveData(new List<Emotion>(), Vector2.one * 0.5f);
			graphCollection.Add(data);
		}
		graphHistory.Save (graphCollection);

		base.Start ();
	}
	
	protected override void Update ()
	{
		base.Update ();
	}
	
	public void OpenHistory()
	{
		if (historyLayoutInstance)
		{
			Destroy(historyLayoutInstance.gameObject);
			return;
		}
		historyLayoutInstance = Instantiate(historyDisplay) as HistoryButtonLayout;
		historyLayoutInstance.gameObject.SetActive (true);
		historyLayoutInstance.transform.SetParent(this.transform);
		historyLayoutInstance.transform.localScale = Vector3.one;
		RectTransform rectTrans = historyLayoutInstance.transform as RectTransform;
		rectTrans.offsetMax = Vector2.zero;
		rectTrans.offsetMin = Vector2.zero;
	}

	public virtual void SwapGraphInfo(DateTime date)
	{
		graphCollection = graphHistory.Load ();
		foreach (SaveData gd in graphCollection) {
			//if the date we want isn't this data's date move to next piece of data
			if (gd.date != date)
				continue;
		
			dateOnDisplay = date;
			if(historyButton)
				historyButton.GetComponentInChildren<Text> ().text = date.ToShortDateString ();
			if (historyLayoutInstance)
				Destroy (historyLayoutInstance.gameObject);
			break;
		}
	}

	public SaveData GetTodayData()
	{
		foreach (var data in graphCollection) {
			if(data.date == DateTime.Today)
				return data;
		}
		return new SaveData(new List<Emotion>(), Vector2.one * 0.5f);
	}

	public SaveData GetDataAtDate(DateTime date)
	{
		foreach (var data in graphCollection) {
			if(data.date == date)
				return data;
		}
		return GetTodayData();
	}
}
