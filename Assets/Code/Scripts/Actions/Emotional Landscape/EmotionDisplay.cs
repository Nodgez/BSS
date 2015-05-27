﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class EmotionDisplay : MonoBehaviour {

	public Button button;
	public Emotion emotion;
	private ELMenu menu;
	private UnityAction buttonAction;

	void Start () 
	{
		menu = GameObject.Find ("ELMenu").GetComponent<ELMenu> ();
		buttonAction = delegate {
			menu.RemoveEmotionFromGraph (emotion.emotionName);
		};

		button.onClick.AddListener(buttonAction);
	}
}
