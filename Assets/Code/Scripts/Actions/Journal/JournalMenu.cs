using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class JournalMenu : MonoBehaviour {

	public SaveHistory history;
	List<JournalEntry> journalEntries = new List<JournalEntry	>();
	public JournalEntry journalTabPrefab;
	public GameObject journalEntriesContainer;

	public void RefreshData ()
	{
		int childCount = journalEntriesContainer.transform.childCount - 1;
		for(int i = childCount; i > -1; i--)
			Destroy(journalEntriesContainer.transform.GetChild(i).gameObject);

		List<SaveData> data = history.Load ();
		foreach (SaveData sd in data) {
//			if (sd.journalEntry == null && sd.date != DateTime.Today)
//				continue;
			JournalEntry entry = Instantiate (journalTabPrefab) as JournalEntry;
			Text[] texts = entry.GetComponentsInChildren<Text> ();
			texts [0].text = sd.journalEntry;
			texts [1].text = sd.date.ToShortDateString ();
			entry.transform.SetParent (journalEntriesContainer.transform);
			entry.transform.localScale = Vector3.one;
			SaveData currentdata = sd;
			entry.GetComponentInChildren<Button> ().onClick.AddListener (delegate {
				entry.OpenInput (currentdata);
			});
		}
	}

	// Use this for initialization
	void Start () 
	{
		history = new SaveHistory (Application.persistentDataPath + "//Graph.xml");
		RefreshData ();
	}
}
