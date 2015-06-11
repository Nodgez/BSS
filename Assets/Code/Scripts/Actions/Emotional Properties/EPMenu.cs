using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EPMenu : HistoryMenu {

	public EmotionListFromGraph emotionList;
	public GraphRepresentation graphRepresentation;
	public SlidingPanel propertiesPanel;

	private GraphData dataOnDisplay;
	private Emotion currentEmotionforEdit;
	private EventSystem eventSystem;

	protected override void Start ()
	{
		base.Start ();
		GraphData todayData = GetTodayData ();
		emotionList.AddEmotions (todayData.emotions);
		graphRepresentation.Shift (todayData.normailzedGraphPosition);
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		dataOnDisplay = GetTodayData ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public override void SwapGraphInfo (System.DateTime date)
	{
		base.SwapGraphInfo (date);

		GraphData data = GetDataAtDate (date);
		if (data == null)
			return;
		emotionList.AddEmotions (data.emotions);
		graphRepresentation.Shift (data.normailzedGraphPosition);
		dataOnDisplay = data;
	}

	public void OpenPropertiesPanel(string name)
	{
		if (currentEmotionforEdit != null) {
			if (currentEmotionforEdit.emotionName == name)
				propertiesPanel.SlideView ();
		}
		foreach (Emotion emo in dataOnDisplay.emotions)
			if (emo.emotionName == name)
				currentEmotionforEdit = emo;
	}

	public void OnEditProperty()
	{
		GameObject selectedObject = eventSystem.currentSelectedGameObject;
		Debug.Log (selectedObject.name);
		EmotionalProperty property = currentEmotionforEdit.properties;
		if (property == null)
			property = new EmotionalProperty ();
		InputField input = selectedObject.GetComponent<InputField> ();
		switch(selectedObject.name)
		{
		case "Feel Input":
			property.emotionLocation = input.text;
			break;
		case "Size Input":
			property.emotionSize = input.text;
			break;
		case "Sound Input":
			property.emotionSound = input.text;
			break;
		case "Temperature Input":
			property.emotionTemperature = input.text;
			break;
		case "Intensity Input":
			property.emotionIntensity = input.text;
			break;
		case "Colour Input":
			property.emotionColor = input.text;
			break;
		case "Through Body Input":
			property.emotionMoveThrough = input.text;
			break;
		case "Off Body Input":
			property.emotionMoveOff = input.text;
			break;
		}
		currentEmotionforEdit.properties = property;
		graphHistory.Save (graphCollection);
	}
}

public class EmotionalProperty
{
	public string emotionLocation;
	public string emotionIntensity;
	public string emotionColor;
	public string emotionTemperature;
	public string emotionSound;
	public string emotionSize;
	public string emotionMoveThrough;
	public string emotionMoveOff;
}
