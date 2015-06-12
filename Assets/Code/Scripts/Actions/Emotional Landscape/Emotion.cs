using UnityEngine;
using System.Collections;

public enum EmotionType
{
	Positive,
	Emergency
}
[System.Serializable]
public class Emotion 
{
	[HideInInspector]
	public Vector3 position;
	public EmotionType emotionType;
	public string emotionName;
	public EmotionalProperty properties;

	public Emotion()
	{
	}

	public Emotion(Vector3 position, EmotionType emotionType, string emotionName)
	{
		this.position = position;
		this.emotionName = emotionName;
		this.emotionType = emotionType;
		this.properties = new EmotionalProperty ();
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

	public EmotionalProperty()
	{
		emotionLocation = "";
		emotionSize = "";
		emotionIntensity = "";
		emotionColor = "";
		emotionTemperature = "";
		emotionSound = "";
		emotionMoveThrough = "";
		emotionMoveOff = "";
	}
}
