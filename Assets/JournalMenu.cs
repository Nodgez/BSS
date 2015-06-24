using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class JournalMenu : MonoBehaviour {

	public SaveHistory history;
	List<JournalEntry> journalEntries = new List<JournalEntry	>();
	public JournalEntry journalTabPrefab;
	public GameObject journalEntriesContainer;

	// Use this for initialization
	void Start () 
	{
		history = new SaveHistory (Application.persistentDataPath + "//Graph.xml");
		List<SaveData> data = history.Load ();
		foreach (SaveData sd in data) 
		{
			SaveData currentdata = sd;
			JournalEntry entry = Instantiate (journalTabPrefab) as JournalEntry;
			Text[] texts = entry.GetComponentsInChildren<Text>();
			texts[0].text = sd.journalEntry;
			texts[1].text = sd.date.ToShortDateString();
			entry.transform.SetParent (journalEntriesContainer.transform);
			entry.transform.localScale = Vector3.one;
			entry.GetComponentInChildren<Button>().onClick.AddListener(delegate {
				entry.OpenInput(currentdata);
			});
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnInfoEdit()
	{
		SaveData data = history.GetTodayData ();
	}
}
