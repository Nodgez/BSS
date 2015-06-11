﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EmotionListFromGraph : MonoBehaviour {

	public Button emotionButton;
	public Image emptySlotPrefab;

	public void AddEmotions(List<Emotion> emotions)
	{
		EPMenu epMenu = GameObject.Find ("EPMenu").GetComponent<EPMenu> ();

		if (transform.childCount > 0)
			for (int c = 0; c < transform.childCount; c++)
				Destroy(transform.GetChild (c).gameObject);

		for(int i = 0;i < 5;i++)
		{
			if(i > emotions.Count - 1)
			{
				Image emptySlot = Instantiate(emptySlotPrefab) as Image;
				emptySlot.transform.SetParent(transform);
				emptySlot.transform.localScale = Vector3.one;
				continue;
			}
			Button newButton = Instantiate(emotionButton) as Button;
			newButton.transform.SetParent(transform);
			newButton.transform.localScale = Vector3.one;
			newButton.GetComponentInChildren<Text>().text = emotions[i].emotionName;
			newButton.onClick.AddListener(delegate {
				epMenu.OpenPropertiesPanel ();
			});
		}
	}
}