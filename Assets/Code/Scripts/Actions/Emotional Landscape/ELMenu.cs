using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ELMenu : HistoryMenu {
	
	public EmotionDisplay emotionDisplay;	
	public SlidingPanel[] emotionsPanels;
	public EmotionalCollection availableEmotions;
	public Graph visibleGraph;
	public GameObject addEmotionForm;
	private List<Button> emotionButtons = new List<Button> ();
	
	protected override void Start()
	{
		base.Start ();
		
		//Find the Main sliding panel that slide from main menu to the Skill
		GameObject masterMenuSlider = GameObject.Find ("MasterMenuSlider");
		if(masterMenuSlider != null)
		{
			//Get the slding event and add toggle  scrips to trigger on it's completion
			SlidingPanel silder = masterMenuSlider.GetComponent<SlidingPanel> ();
			silder.onSlideComplete += ToggleDisabledObjects;
			silder.onSlideTo += delegate {
				//If data exists for today then load it
				SwapGraphInfo (DateTime.Today);
			};
		}
		
		Button[] tmpArray;
		foreach (SlidingPanel panel in emotionsPanels)
		{
			//get all the buttons that have emotions
			tmpArray = panel.GetComponentsInChildren<Button> ();
			foreach(Button b in tmpArray)
				emotionButtons.Add(b);
		}
	}
	
	protected override void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			visibleGraph.ResetGraph ();
		
		//		if(Input.GetMouseButtonDown(0) && selectedEmotion != null)
		////		if(Input.touchCount > 0)
		////			if(Input.GetTouch(0).phase == TouchPhase.Began && selectedEmotion != null)
		//			{
		//				foreach (SlidingPanel panel in openPanels)
		//					panel.SlideView ();
		//				AddEmotionToGraph ();
		//				selectedEmotion = null;
		//			}
		
		base.Update ();
	}
	
	public void AddEmotionToGraph(Vector3 position, Emotion selectedEmotion)
	{
		Debug.Log ("Passed Position : " + position.ToString ());
		if (dateOnDisplay != DateTime.Today || visibleGraph.GetDisplayedEmotions.Count >= 5) {
			Debug.Log("Returned on Add Emotion, Date : " + dateOnDisplay.ToShortDateString() +
			          "Emotion count : " + visibleGraph.GetDisplayedEmotions.Count);
			if(selectedEmotion.emotionType == EmotionType.Emergency)
			{
				emotionsPanels[1].GetComponentInChildren<ELMenuEmotions>().ReturnEmotionToList(selectedEmotion);
			}
			else
			{
				emotionsPanels[0].GetComponentInChildren<ELMenuEmotions>().ReturnEmotionToList(selectedEmotion);
			}
			return;
		}
		Vector3 screenCenter = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 10);
		//Create a prefab for the display
		EmotionDisplay displayPrefab = Instantiate (emotionDisplay) as EmotionDisplay;
		displayPrefab.transform.SetParent (visibleGraph.transform);
		displayPrefab.transform.localScale = Vector3.one;
		displayPrefab.GetComponentInChildren<Text> ().text = selectedEmotion.emotionName;
		displayPrefab.name = selectedEmotion.emotionName;
		displayPrefab.emotion = selectedEmotion;
		position = Camera.main.WorldToScreenPoint (position);
		Debug.Log ("Position world to Screen : " + position.ToString ());

		//ensure correct positioning
		if (position.x > screenCenter.x && selectedEmotion.emotionType == EmotionType.Positive)
			position = new Vector3 (screenCenter.x, position.y, 0);
		else if(position.x < screenCenter.x && selectedEmotion.emotionType == EmotionType.Emergency)
			position = new Vector3 (screenCenter.x, position.y, 0);

		if(Vector2.Distance(position, screenCenter) > 400)
		{
			Vector3 direction = position - screenCenter;
			position = direction.normalized * 400;
			Debug.Log("defaulting to screen space : " + position.ToString());
		}

		position = Camera.main.ScreenToWorldPoint (position);
		Debug.Log ("Set Position : " + position.ToString ());
		displayPrefab.transform.position = position;
		displayPrefab.emotion.position = position;
		visibleGraph.AddEmotion (displayPrefab);
		
		foreach (Button b in emotionButtons)
			if (b.gameObject.name == selectedEmotion.emotionName)
				b.interactable = false;
		
		SaveCurrentEmotionalState ();
	}
	
	public void AddEmotionToGraph(Emotion newEmotion)
	{
		if (dateOnDisplay != DateTime.Today)
			return;
		EmotionDisplay displayPrefab = Instantiate (emotionDisplay) as EmotionDisplay;
		displayPrefab.transform.SetParent (visibleGraph.transform);
		displayPrefab.transform.localScale = Vector3.one;
		displayPrefab.GetComponentInChildren<Text> ().text = newEmotion.emotionName;
		displayPrefab.name = newEmotion.emotionName;
		displayPrefab.emotion = newEmotion;
		Vector3 position = newEmotion.position;// Input.mousePosition;//GetTouch(0).position;
		
		displayPrefab.transform.position = new Vector3 (position.x,
		                                                position.y,
		                                                0);
		visibleGraph.AddEmotion (displayPrefab);
		
		foreach (Button b in emotionButtons)
			if (b.gameObject.name == newEmotion.emotionName)
				b.interactable = false;
		
		SaveCurrentEmotionalState ();
	}
	
	private void AddSavedEmotionToGraph(Emotion newEmotion)
	{
		if (visibleGraph.GetDisplayedEmotions.Count >= 5)
			return;
		
		EmotionDisplay displayPrefab = Instantiate (emotionDisplay) as EmotionDisplay;
		displayPrefab.transform.SetParent (visibleGraph.transform);
		displayPrefab.transform.localScale = Vector3.one;
		displayPrefab.GetComponentInChildren<Text> ().text = newEmotion.emotionName;
		displayPrefab.name = newEmotion.emotionName;
		displayPrefab.emotion = newEmotion;
		Vector3 position = newEmotion.position;// Input.mousePosition;//GetTouch(0).position;
		
		displayPrefab.transform.position = new Vector3 (position.x,
		                                                position.y,
		                                                0);
		visibleGraph.AddEmotion (displayPrefab);
		
		foreach (Button b in emotionButtons)
			if (b.gameObject.name == newEmotion.emotionName)
				b.interactable = false;
	}
	
	public void RemoveEmotionFromGraph(string name)
	{
		if (dateOnDisplay != DateTime.Today)
			return;
		
		visibleGraph.RemoveEmotion (name);
		foreach (Button b in emotionButtons)
			if (b.gameObject.name == name)
				b.interactable = transform;
		
		SaveCurrentEmotionalState ();
	}
	
//	public void SelectEmotion(int i)
//	{
//		selectedEmotion = availableEmotions.GetEmotion (i);
//		SaveScreenState ();
//		CloseOpenPanels ();
//		AddEmotionToGraph ();
//	}
	
	public override void SwapGraphInfo (DateTime date)
	{
		if (visibleGraph == null)
			return;
		base.SwapGraphInfo (date);
		
		foreach (SaveData gd in graphCollection) {
			//if the date we want isn't this data's date move to next piece of data
			if (gd.date != date)
				continue;
			visibleGraph.ResetGraph ();
			foreach (Emotion em in gd.emotions)
				AddSavedEmotionToGraph (em);
		}
	}
	
	public void SaveCurrentEmotionalState ()
	{
		List<Emotion> emotionsOnDisplay = new List<Emotion> ();
		foreach (EmotionDisplay ed in visibleGraph.GetDisplayedEmotions)
			emotionsOnDisplay.Add (ed.emotion);
		SaveData data = new SaveData (emotionsOnDisplay, visibleGraph.ShiftedPosition);
		saveDataManager.Save (data);
	}
	
	public void AddEmotionToList()
	{
		if (visibleGraph.GetDisplayedEmotions.Count >= 5)
			return;
		GameObject form = Instantiate (addEmotionForm) as GameObject;
		form.transform.SetParent (this.transform);
		form.transform.localScale = Vector3.one;
		RectTransform formTransform = form.transform as RectTransform;
		formTransform.offsetMax = Vector2.zero;
		formTransform.offsetMin = Vector2.zero;
	}
}
