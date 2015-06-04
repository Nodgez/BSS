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
	public Vector2 directionalValue;
	public Vector3 position;
	public EmotionType emotionType;
	public string emotionName;
}
