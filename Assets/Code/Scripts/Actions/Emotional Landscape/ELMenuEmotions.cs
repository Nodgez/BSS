using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ELMenuEmotions : MonoBehaviour {

	public EmotionListItem listItemPrefab;
	public EmotionalCollection emotionalCollection;
	public EmotionType typeToOrder;

	private ELMenu elMenu;
	private List<Emotion> emotionsToStore = new List<Emotion>();

	public void SetUpList ()
	{
		//find the controlling menu item. this class cannot exist without a prior menu
		elMenu = GameObject.Find ("ELMenu").GetComponent<ELMenu> ();
		if (!elMenu)
			return;
		//for every emotion we have check the type we want in our list and store only those.
		foreach (Emotion emo in emotionalCollection.emotions) {
			if (emo.emotionType == typeToOrder)
				emotionsToStore.Add (emo);
		}
		//sort the list alphabetically
		emotionsToStore.Sort ((x, y) => string.Compare (x.emotionName, y.emotionName));

		//find and set the size of the itemsizedbox
		ItemSizedBox sizedBox = GetComponent<ItemSizedBox> ();
		sizedBox.size = emotionsToStore.Count;

		for (int i = 0; i < emotionsToStore.Count; i++) {
			//get the emotion I want from the overall list of emotions
			EmotionListItem listItem = Instantiate (listItemPrefab) as EmotionListItem;
			listItem.name = emotionsToStore [i].emotionName;
			listItem.GetComponentInChildren<Text> ().text = emotionsToStore [i].emotionName;
			listItem.emotion = emotionsToStore[i];
			listItem.transform.SetParent (transform);
			listItem.transform.localScale = Vector3.one;
		}
	}
	
	void Start()
	{
		SetUpList ();
	}

	public void ReturnEmotionToList(Emotion emotion)
	{
		EmotionListItem listItem = Instantiate (listItemPrefab) as EmotionListItem;
		listItem.name = emotion.emotionName;
		listItem.GetComponentInChildren<Text> ().text = emotion.emotionName;
		listItem.emotion = emotion;
		listItem.transform.SetParent (transform);
		listItem.transform.localScale = Vector3.one;

		int newIndex = 0;
		Transform[] transforms = GetComponentsInChildren<Transform> ();
		foreach (Transform t in transforms.Cast<Transform>().OrderBy (t=>t.name))
		{
			if (t != t.root)
			{
				t.SetSiblingIndex (newIndex);
				newIndex ++;
			}
		}
	}
}
