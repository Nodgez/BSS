using UnityEngine;
using System.Collections;

public class EmotionalCollection : MonoBehaviour {

	public Emotion[] emotions;

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
		if (index < emotions.Length && index > -1)
			return emotions [index];

		Emotion defaultEmotion = new Emotion ();
		defaultEmotion.emotionName = "Null Emotion";
		return defaultEmotion;
	}
}
