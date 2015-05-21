using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour {

	private ScrollRect _scrollRect;
	private List<Emotion> emotions = new List<Emotion>();
	private bool sliding = false;
	private Vector2 lerpEnd;
	private float lerpValue;

	public GameObject baseEmoObject;

	void Start () 
	{
		_scrollRect = GetComponent<ScrollRect> ();
		_scrollRect.normalizedPosition = new Vector2 (0.5f, 0.5f);
	}

	void Update()
	{
		lerpValue += Time.deltaTime;
		if (!sliding)
		{
			lerpValue = 0;
			return;
		}

		_scrollRect.normalizedPosition = Vector2.Lerp (_scrollRect.normalizedPosition, lerpEnd, lerpValue);
		if (lerpValue >= 1)
			sliding = false;
	}

	public void AddEmotion(Emotion emotion)
	{
		if (emotions.Contains (emotion))
			return;

		sliding = true;
		emotions.Add (emotion);
		lerpEnd = _scrollRect.normalizedPosition + emotion.directionalValue;
	}

	public void RemoveEmotion(Emotion emotion)
	{
		if (!emotions.Contains (emotion))
			return;

		_scrollRect.normalizedPosition -= emotion.directionalValue;
		emotions.Remove (emotion);
	}

	public List<Emotion> GetEmotions
	{
		get{ return emotions;}
	}
}

[Serializable]
public class GraphData : ScriptableObject
{
	public DateTime date;
	public List<Emotion> emotions;

	public GraphData(List<Emotion> emotions)
	{
		this.date = DateTime.Now;
		this.emotions = emotions;
	}
}
