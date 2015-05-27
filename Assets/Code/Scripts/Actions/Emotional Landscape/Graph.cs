using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour {

	private ScrollRect _scrollRect;
	private List<EmotionDisplay> emotions = new List<EmotionDisplay>();
	private bool sliding = false;
	private Vector2 lerpEnd;
	private float lerpValue;
	private GraphHistory graphHistory;

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


	public void AddEmotion(EmotionDisplay emotion, Vector2 direction)
	{
		sliding = true;
		emotions.Add (emotion);
		lerpEnd = _scrollRect.normalizedPosition + direction;
	}

	public void RemoveEmotion(string name)
	{
		EmotionDisplay display;
		for(int i = 0; i < emotions.Count;i++)
		{
			if (emotions[i].emotion.emotionName == name)
			{
				display = emotions[i];
				lerpEnd = _scrollRect.normalizedPosition - display.emotion.directionalValue;
				Destroy(display.gameObject);
				emotions.RemoveAt(i);
				sliding = true;
				break;
			}
		}
	}
	
	public bool HasEmotion(string name)
	{
		foreach(EmotionDisplay emotion in emotions)
		{
			if(emotion.name == name)
				return true;
		}

		return false;
	}

	public List<EmotionDisplay> GetDisplayedEmotions
	{
		get
		{
			return emotions;
		}
	}
}

[Serializable]
public class GraphData
{
	public DateTime date;
	public List<Emotion> emotions;

	public GraphData()
	{
		this.date = DateTime.Now;
		this.emotions = null;
	}

	public GraphData(List<Emotion> emotions)
	{
		this.date = DateTime.Now;
		this.emotions = emotions;
	}
}
