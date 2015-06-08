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

//		SlidingPanel masterSlidingPanel = GameObject.Find ("MasterMenuSlider").GetComponent<SlidingPanel> ();
//		masterSlidingPanel.onSlideBack += ResetGraph;
	}

	void Update()
	{
		lerpValue += Time.deltaTime;
		if (!sliding)
		{
			_scrollRect.normalizedPosition = Reposition ();
			lerpValue = 0;
			return;
		}

		_scrollRect.normalizedPosition = Vector2.Lerp (_scrollRect.normalizedPosition, lerpEnd, lerpValue);
		if (lerpValue >= 1)
			sliding = false;
	}

	public void AddEmotion(EmotionDisplay emotion)
	{
		sliding = true;
		emotions.Add (emotion);
		lerpEnd = Reposition ();
	}

	public void RemoveEmotion(string name)
	{
		EmotionDisplay display;
		for(int i = 0; i < emotions.Count;i++)
		{
			if (emotions[i].emotion.emotionName == name)
			{
				display = emotions[i];
				Destroy(display.gameObject);
				emotions.RemoveAt(i);
				sliding = true;
				break;
			}
		}
		lerpEnd = Reposition ();
	}

	public void ResetGraph()
	{
		for(int i = 0; i < emotions.Count;i++)
		{
			EmotionDisplay display = emotions[i];
			Destroy(display.gameObject);
		}
		emotions.Clear ();
		_scrollRect.normalizedPosition = new Vector2 (0.5f, 0.5f);
	}

	Vector2 Reposition()
	{
		Vector2 collectiveDirection = Vector2.zero;
		foreach (EmotionDisplay emo in emotions) {
			Vector3 worldToViewport = Camera.main.WorldToViewportPoint (emo.transform.position);//get position on screen
			Vector2 screenTop = new Vector2 (0.5f, 0);						//get top as max distance
			Vector2 emoScreenPoint = new Vector2 (worldToViewport.x, worldToViewport.y);		//convert emotion pos to vector2
			Vector2 screenCenter = new Vector2 (0.5f, 0.5f);									//set the screen center
			float maxEmotionWeight = Vector2.Distance (screenTop, screenCenter);			//get the longest distance allowed
			Vector2 direction = emoScreenPoint - screenCenter;								//get the direction from center to emotion
			float emotionWeight = Vector2.Distance (emoScreenPoint, screenCenter);			//get distance from emotion to center	
			emotionWeight = Mathf.Clamp(maxEmotionWeight - emotionWeight, 0.01f , maxEmotionWeight) * 0.5f;
			collectiveDirection += direction.normalized * emotionWeight;
			Debug.DrawRay(emo.transform.position,direction.normalized,Color.red, 1.0f);
		}
		if(emotions.Count > 0)
			return new Vector2(0.5f,0.5f) + collectiveDirection;
		else
			return new Vector2 (0.5f, 0.5f);
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
