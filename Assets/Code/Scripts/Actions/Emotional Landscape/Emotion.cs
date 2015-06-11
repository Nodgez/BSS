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
