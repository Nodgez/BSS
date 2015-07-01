using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class JournalEntry : MonoBehaviour {

	public GameObject journalInput;
	public JournalMenu journalMenu;

	private GameObject journalInputInstance;

	public void Start()
	{
		journalMenu = GameObject.Find ("View").GetComponent<JournalMenu> ();
	}

	public void OpenInput(SaveData data)
	{
		if (journalInputInstance != null)
			return;

		string originalText = data.journalEntry;
		if (originalText == null)
			originalText = "";

		journalInputInstance = Instantiate (journalInput) as GameObject;
		journalInputInstance.transform.SetParent (journalMenu.transform);
		journalInputInstance.transform.localScale = Vector3.one;
		RectTransform inputRect = journalInputInstance.transform as RectTransform;
		inputRect.offsetMax = Vector2.zero;
		inputRect.offsetMin = Vector2.zero;

		InputField input = journalInputInstance.GetComponentInChildren<InputField> ();
		input.text = originalText;
		if (data.date != DateTime.Today) {
			input.interactable = false;
		}
		DateTime date = data.date;

		journalInputInstance.GetComponentInChildren<Button> ().onClick.AddListener (delegate {
			this.SaveEntry(date);
		});
	}

	public void SaveEntry(DateTime date)
	{
		if (journalInputInstance == null)
			return;
		if (date != DateTime.Today) {
			Destroy (journalInputInstance);
			return;
		}
		SaveData data = journalMenu.history.GetTodayData ();
		data.journalEntry = journalInputInstance.GetComponentInChildren<InputField> ().text;
		GetComponentsInChildren<Text> () [0].text = data.journalEntry;
		journalMenu.history.Save (data);
		journalMenu.RefreshData ();
		Destroy (journalInputInstance);
	}
}
