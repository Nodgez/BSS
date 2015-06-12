using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class PropertiesForm : MonoBehaviour {

	public InputField feelInput;
	public InputField sizeInput;
	public InputField soundInput;
	public InputField temperatureInput;
	public InputField intensityInput;
	public InputField colorInput;
	public InputField moveThroughInput;
	public InputField moveOffInput;
	public EPMenu epMemu;
	public SlidingPanel slidingPanel;
	public Emotion currentEmotionForEdit;

	private EventSystem eventSystem;


	void Start()
	{
		slidingPanel = GetComponent<SlidingPanel> ();
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();

		feelInput.text = "";
		sizeInput.text = "";
		soundInput.text = "";
		temperatureInput.text = "";
		intensityInput.text = "";
		colorInput.text = "";
		moveThroughInput.text = "";
		moveOffInput.text = "";
	}

	public void SwapInfo(Emotion emotion)
	{
		if(epMemu.dateOnDisplay != DateTime.Today)
		{
			feelInput.interactable = false;
			sizeInput.interactable = false;
			soundInput.interactable = false;
			temperatureInput.interactable = false;
			intensityInput.interactable = false;
			colorInput.interactable = false;
			moveThroughInput.interactable = false;
			moveOffInput.interactable = false;
		}
		
		else
		{
			feelInput.interactable = true;
			sizeInput.interactable = true;
			soundInput.interactable = true;
			temperatureInput.interactable = true;
			intensityInput.interactable = true;
			colorInput.interactable = true;
			moveThroughInput.interactable = true;
			moveOffInput.interactable = true;
		}

		if (currentEmotionForEdit.emotionName == emotion.emotionName
		    || currentEmotionForEdit.emotionName == "")
			slidingPanel.SlideView ();

		currentEmotionForEdit = emotion;
		EmotionalProperty properties = currentEmotionForEdit.properties;

		if (properties == null)
			properties = new EmotionalProperty();

		feelInput.text = properties.emotionLocation;
		sizeInput.text = properties.emotionSize;
		soundInput.text = properties.emotionSound;
		temperatureInput.text = properties.emotionTemperature;
		intensityInput.text = properties.emotionIntensity;
		colorInput.text = properties.emotionColor;
		moveThroughInput.text = properties.emotionMoveThrough;
		moveOffInput.text = properties.emotionMoveOff;
	}

	public void OnEditProperty()
	{
		GameObject selectedObject = eventSystem.currentSelectedGameObject;
		Debug.Log (selectedObject.name);
		EmotionalProperty property = currentEmotionForEdit.properties;
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
		currentEmotionForEdit.properties = property;
		epMemu.SaveProperties ();
	}
}
