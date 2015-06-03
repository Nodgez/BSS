using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ELMenu : Menu {
	
	public EmotionDisplay emotionDisplay;
	public HistoryButtonLayout historyDisplay;
	public Button historyButton;
	public SlidingPanel[] emotionsPanels;
	public EmotionalCollection availableEmotions;
	public Graph visibleGraph;
	public List<GraphData> graphCollection;
	public GameObject addEmotionForm;
	private Emotion selectedEmotion;
	private List<Button> emotionButtons = new List<Button> ();
	private GraphHistory graphHistory;
	private DateTime dateOnDisplay;
	private HistoryButtonLayout historyLayoutInstance;

	protected override void Start()
	{
		//create a new graph history pointing to the location where the data is stroed
		graphHistory = new GraphHistory (Application.persistentDataPath + "//Graph.xml");
		graphCollection = graphHistory.Load();

		bool hasDataForToday = false;
		foreach(GraphData gd in graphCollection){
			if(gd.date == DateTime.Today){
				hasDataForToday = true;
				break;
			}
		}

		if(!hasDataForToday){
			GraphData data = new GraphData(new List<Emotion>(), Vector2.one * 0.5f);
			graphHistory.Save(data);
		}

		//Find the Main sliding panel that slide from main menu to the Skill
		GameObject masterMenuSlider = GameObject.Find ("MasterMenuSlider");
		if(masterMenuSlider != null)
		{
			//Get the slding event and add toggle  scrips to trigger on it's completion
			SlidingPanel silder = masterMenuSlider.GetComponent<SlidingPanel> ();
			silder.onSlideComplete += ToggleDisabledObjects;
			silder.onSlideComplete += delegate {
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

		base.Start ();
	}

	protected override void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			visibleGraph.ResetGraph ();

		if(Input.GetMouseButtonDown(0) && selectedEmotion != null)
//		if(Input.touchCount > 0)
//			if(Input.GetTouch(0).phase == TouchPhase.Began && selectedEmotion != null)
			{
				foreach (SlidingPanel panel in openPanels)
					panel.SlideView ();
				AddEmotionToGraph ();
				selectedEmotion = null;
			Debug.Log ("Emotion Nulled");
			}

		base.Update ();
	}

	private void AddEmotionToGraph()
	{
		if (dateOnDisplay != DateTime.Today)
			return;
		EmotionDisplay displayPrefab = Instantiate (emotionDisplay) as EmotionDisplay;
		displayPrefab.transform.SetParent (visibleGraph.transform);
		displayPrefab.transform.localScale = Vector3.one;
		displayPrefab.GetComponentInChildren<Text> ().text = selectedEmotion.emotionName;
		displayPrefab.name = selectedEmotion.emotionName;
		displayPrefab.emotion = selectedEmotion;
		Vector3 position = Input.mousePosition;//GetTouch(0).position;

		if (position.x > Screen.width * 0.5f && selectedEmotion.emotionType == EmotionType.Positive)
			position = new Vector3 (Screen.width * 0.5f, position.y, 0);
		else if(position.x < Screen.width * 0.5f && selectedEmotion.emotionType == EmotionType.Emergency)
			position = new Vector3 (Screen.width * 0.5f, position.y, 0);

		position = Camera.main.ScreenToWorldPoint (position);
		position = new Vector3 (position.x,
		                        position.y,
		                        0);
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
		
		//SaveCurrentEmotionalState ();
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

	public void SelectEmotion(int i)
	{
		selectedEmotion = availableEmotions.GetEmotion (i);
		SaveScreenState ();
		CloseOpenPanels ();
	}

	public void OpenHistory()
	{
		if (historyLayoutInstance)
			return;
		historyLayoutInstance = Instantiate(historyDisplay) as HistoryButtonLayout;
		historyLayoutInstance.gameObject.SetActive (true);
		historyLayoutInstance.transform.SetParent(this.transform);
		historyLayoutInstance.transform.localScale = Vector3.one;
		RectTransform rectTrans = historyLayoutInstance.transform as RectTransform;
		rectTrans.offsetMax = Vector2.zero;
		rectTrans.offsetMin = Vector2.zero;
	}

	public void SwapGraphInfo(DateTime date)
	{
		if(date != DateTime.Today)
			graphCollection = graphHistory.Load ();
		foreach(GraphData gd in graphCollection)
		{
			//if the date we want isn't this data's date move to next piece of data
			if(gd.date != date)
				continue;

			visibleGraph.ResetGraph();

			foreach(Emotion em in gd.emotions)
				AddSavedEmotionToGraph(em);
			dateOnDisplay = date;
			historyButton.GetComponentInChildren<Text>().text = dateOnDisplay.ToShortDateString();
			if(historyLayoutInstance)
				Destroy(historyLayoutInstance.gameObject);
			break;
		}
	}

	public void SaveCurrentEmotionalState ()
	{
		List<Emotion> emotionsOnDisplay = new List<Emotion> ();
		foreach (EmotionDisplay ed in visibleGraph.GetDisplayedEmotions)
			emotionsOnDisplay.Add (ed.emotion);
		GraphData data = new GraphData (emotionsOnDisplay, visibleGraph.ShiftedPosition);
		graphHistory.Save (data);
	}

	public void AddEmotionToList()
	{
		GameObject form = Instantiate (addEmotionForm) as GameObject;
		form.transform.SetParent (this.transform);
		form.transform.localScale = Vector3.one;
	}
}
