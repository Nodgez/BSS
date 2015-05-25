using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ELMenu : MonoBehaviour {

	delegate void MenuEvent(); 

	public EmotionDisplay emotionDisplay;
	public EmotionalCollection availableEmotions;
//	public SlidingPanel positiveEmotionPanel;
//	public SlidingPanel emergencyEmotionPanel;
	public SlidingPanel[] slidingPanels;
	public Graph visibleGraph;
	private Emotion selectedEmotion;
	private List<Button> emotionButtons = new List<Button> ();
	private List<SlidingPanel> openPanels = new List<SlidingPanel> ();

	void Start()
	{
		Button[] tmpArray;
		foreach (SlidingPanel panel in slidingPanels)
		{
			tmpArray = panel.GetComponentsInChildren<Button> ();
			foreach(Button b in tmpArray)
				emotionButtons.Add(b);
		}
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0) && selectedEmotion != null)
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
		displayPrefab.transform.SetParent (visibleGraph.transform.GetChild(0));
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
	}

	public void RemoveEmotionFromGraph(string name)
	{
		visibleGraph.RemoveEmotion (name);
		foreach (Button b in emotionButtons)
			if (b.gameObject.name == name)
				b.interactable = transform;
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
}
