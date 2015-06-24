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
		string originalText = data.journalEntry;
		if (originalText == null)
			originalText = "";
		if (journalInputInstance != null)
			return;
		journalInputInstance = Instantiate (journalInput) as GameObject;
		journalInputInstance.transform.SetParent (journalMenu.transform);
		RectTransform inputRect = journalInputInstance.transform as RectTransform;
		inputRect.offsetMax = Vector2.zero;
		inputRect.offsetMin = Vector2.zero;

		InputField input = journalInputInstance.GetComponentInChildren<InputField> ();
		input.text = originalText;
		if (data.date != DateTime.Today) {
			input.interactable = false;
			return;
		}

		journalInputInstance.GetComponentInChildren<Button> ().onClick.AddListener (delegate {
			this.SaveEntry();
		});
	}

	public void SaveEntry()
	{
		if (journalInputInstance == null)
			return;
		SaveData data = journalMenu.history.GetTodayData ();
		data.journalEntry = journalInputInstance.GetComponentInChildren<InputField> ().text;
		journalMenu.history.Save (data);
		Destroy (journalInputInstance);
	}
}
