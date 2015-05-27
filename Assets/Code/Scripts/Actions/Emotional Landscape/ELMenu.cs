using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ELMenu : MonoBehaviour {
	
	public EmotionDisplay emotionDisplay;
	public EmotionalCollection availableEmotions;
	public SlidingPanel[] slidingPanels;
	public Graph visibleGraph;
	public GameObject[] disabledGameObjects;
	private Emotion selectedEmotion;
	private List<Button> emotionButtons = new List<Button> ();
	private List<SlidingPanel> openPanels = new List<SlidingPanel> ();
	private GraphHistory graphHistory;

	void Start()
	{
		GameObject masterMenuSlider = GameObject.Find ("MasterMenuSlider");
		if(masterMenuSlider != null)
		{
			SlidingPanel silder = masterMenuSlider.GetComponent<SlidingPanel> ();
			silder.onSlideComplete += ToggleDisabledObjects;
		}

		Button[] tmpArray;
		foreach (SlidingPanel panel in slidingPanels)
		{
			tmpArray = panel.GetComponentsInChildren<Button> ();
			foreach(Button b in tmpArray)
				emotionButtons.Add(b);
		}

		string date = MathExt.ReplaceChar(DateTime.Now.ToShortDateString(),'/','-');	
		graphHistory = new GraphHistory ("//Graph " + date + ".xml");
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0) && selectedEmotion != null)
//		if(Input.touchCount > 0)
//			if(Input.GetTouch(0).phase == TouchPhase.Began && selectedEmotion != null)
			{
				foreach (SlidingPanel panel in openPanels)
					panel.SlideView ();
				AddEmotionToGraph ();
				selectedEmotion = null;
			}
	}

	private void AddEmotionToGraph()
	{
		EmotionDisplay displayPrefab = Instantiate (emotionDisplay) as EmotionDisplay;
		displayPrefab.transform.SetParent (visibleGraph.transform);
		displayPrefab.transform.localScale = Vector3.one;
		displayPrefab.GetComponentInChildren<Text> ().text = selectedEmotion.emotionName;
		displayPrefab.name = selectedEmotion.emotionName;
		displayPrefab.emotion = selectedEmotion;

		displayPrefab.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		displayPrefab.transform.position = new Vector3 (displayPrefab.transform.position.x,
		                                                displayPrefab.transform.position.y,
		                                                0);
		visibleGraph.AddEmotion (displayPrefab,selectedEmotion.directionalValue);

		foreach (Button b in emotionButtons)
			if (b.gameObject.name == selectedEmotion.emotionName)
				b.interactable = false;

		SaveCurrentEmotionalState ();
	}

	public void RemoveEmotionFromGraph(string name)
	{
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

	public void CloseOpenPanels()
	{
		foreach (SlidingPanel panel in slidingPanels)
		{
			if(panel.IsSlid)
				panel.SlideView();
		}
	}

	void SaveCurrentEmotionalState ()
	{
		List<Emotion> emotionsOnDisplay = new List<Emotion> ();
		foreach (EmotionDisplay ed in visibleGraph.GetDisplayedEmotions)
			emotionsOnDisplay.Add (ed.emotion);
		GraphData data = new GraphData (emotionsOnDisplay);
		graphHistory.Save (data);
	}

	void SaveScreenState()
	{
		foreach (SlidingPanel panel in slidingPanels)
		{
			if(openPanels.Contains(panel))
			{
				if(!panel.IsSlid)
					openPanels.Remove(panel);
				continue;
			}

			if (panel.IsSlid)
				openPanels.Add (panel);
		}
	}

	void ToggleDisabledObjects()
	{
		foreach(GameObject go in disabledGameObjects)
		{
			if(go == null)
				continue;
			if(go.activeSelf)
				go.SetActive(false);
			else
				go.SetActive(true);
		}
	}
}
