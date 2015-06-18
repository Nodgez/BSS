using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EmotionListFromGraph : MonoBehaviour {

	public Button emotionButton;
	public Image emptyLandscapeImage;
	public Image emptySlotPrefab;
	public EPMenu epMenu;

	public void AddEmotions(List<Emotion> emotions)
	{
		if (transform.childCount > 0)
			for (int c = 0; c < transform.childCount; c++)
				Destroy(transform.GetChild (c).gameObject);

		if(emotions.Count <= 0)
		{
			Image img = Instantiate(emptyLandscapeImage) as Image;
			img.transform.SetParent(transform);
			img.transform.localScale = Vector3.one;
			return;
		}

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
			int currentIndex = i;
			newButton.onClick.AddListener(delegate {
				epMenu.OpenPropertiesForm (emotions[currentIndex].emotionName);
			});
		}
	}
}
