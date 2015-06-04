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

//	public void AddEmotion(EmotionDisplay emotion)
//	{
//		emotions.Add (emotion);
//	}

	public void AddEmotion(EmotionDisplay emotion)
	{
		sliding = true;
		emotions.Add (emotion);
		RePosition ();
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
		RePosition ();
	}

	public void ResetGraph()
	{
		ClearAllEmotions ();
		_scrollRect.normalizedPosition = new Vector2 (0.5f, 0.5f);
	}

	public void ClearAllEmotions()
	{
		for(int i = 0; i < emotions.Count;i++)
		{
			EmotionDisplay display = emotions[i];
			Destroy(display.gameObject);
		}

		emotions.Clear ();
	}

	void RePosition()
	{
		Vector2 collectiveDirection = Vector2.zero;
		foreach (EmotionDisplay emo in emotions) {
			Vector3 worldToScreen = Camera.main.WorldToScreenPoint (emo.transform.position);//get position on screen
			Vector2 screenTop = new Vector2 (Screen.width * 0.5f, 0);						//get top as max distance
			Vector2 emoScreenPoint = new Vector2 (worldToScreen.x, worldToScreen.y);		//convert emotion pos to vector2
			Vector2 screenCenter = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);	//set the screen center
			float maxEmotionWeight = Vector2.Distance (screenTop, screenCenter);			//get the longest distance allowed
			Vector2 direction = emoScreenPoint - screenCenter;								//get the direction from center to emotion
			float emotionWeight = Vector2.Distance (emoScreenPoint, screenCenter);			//get distance from emotion to center	
			emotionWeight /= maxEmotionWeight;												//percentage
			emotionWeight = 1 - emotionWeight;
			collectiveDirection += direction;
			Debug.DrawRay(emo.transform.position,direction.normalized,Color.red, 10.0f);
		}
		if (emotions.Count > 0)
			lerpEnd = _scrollRect.normalizedPosition + (collectiveDirection.normalized * 0.2f);
		else
			lerpEnd = new Vector2 (0.5f, 0.5f);
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

	public Vector2 ShiftedPosition
	{
		get{return _scrollRect.normalizedPosition;}
		set{ _scrollRect.normalizedPosition = value;}
	}
}

[Serializable]
public class GraphData
{
	public DateTime date;
	public List<Emotion> emotions;
	public Vector2 normailzedGraphPosition;

	public GraphData()
	{
		this.date = DateTime.Today;
		this.emotions = null;
		this.normailzedGraphPosition = new Vector2 (0.5f, 0.5f);
	}

	public GraphData(List<Emotion> emotions, Vector2 normailzedGraphPosition)
	{
		this.date = DateTime.Today;
		this.emotions = emotions;
		this.normailzedGraphPosition = normailzedGraphPosition;
	}
}
