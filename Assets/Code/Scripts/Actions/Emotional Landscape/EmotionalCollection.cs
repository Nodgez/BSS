using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EmotionalCollection : MonoBehaviour {

	public List<Emotion> emotions;

	public Emotion GetEmotion(string name)
	{
		foreach(Emotion emo in emotions)
		{
			if(emo.emotionName == name)
				return emo;
		}

		Emotion defaultEmotion = new Emotion ();
		defaultEmotion.emotionName = "Null Emotion";
		return defaultEmotion;
	}

	public Emotion GetEmotion(int index)
	{
		if (index < emotions.Count && index > -1)
			return emotions [index];

		Emotion defaultEmotion = new Emotion ();
		defaultEmotion.emotionName = "Null Emotion";
		return defaultEmotion;
	}

	public int IndexOf(Emotion emotion)
	{
		for(int i = 0; i < emotions.Count;i++)
		{
			if(emotions[i] == emotion)
				return i;
		}

		return -1;
	}

	public void AddEmotion(string name, EmotionType emoType)
	{
		Emotion emotion = new Emotion ();
		emotion.emotionName = name;
		emotion.emotionType = emoType;
		emotions.Add (emotion);
	}
}
