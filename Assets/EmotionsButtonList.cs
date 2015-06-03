using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EmotionsButtonList : MonoBehaviour {

	public Button buttonPrefab;
	public EmotionalCollection emotionalCollection;
	public EmotionType typeToOrder;

	private ELMenu elMenu;
	private List<Emotion> emotionsToStore = new List<Emotion>();

	public void SetUpList ()
	{
		elMenu = GameObject.Find ("ELMenu").GetComponent<ELMenu> ();
		if (!elMenu)
			return;
		RectTransform rectTrans = transform as RectTransform;
		foreach (Emotion emo in emotionalCollection.emotions) {
			if (emo.emotionType == typeToOrder)
				emotionsToStore.Add (emo);
		}
		emotionsToStore.Sort ((x, y) => string.Compare (x.emotionName, y.emotionName));
		float heightOfContainer = emotionsToStore.Count * 50;
		rectTrans.offsetMin = new Vector2 (rectTrans.offsetMin.x, -heightOfContainer);
		for (int i = 0; i < emotionsToStore.Count; i++) {
			//create button and add event
			int index = emotionalCollection.IndexOf (emotionsToStore [i]);
			Button button = Instantiate (buttonPrefab) as Button;
			button.name = emotionsToStore [i].emotionName;
			button.GetComponentInChildren<Text> ().text = emotionsToStore [i].emotionName;
			button.onClick.AddListener (delegate {
				elMenu.SelectEmotion (index);
			});
			button.transform.SetParent (transform);
			button.transform.localScale = Vector3.one;
		}
	}
	
	void Start()
	{
		SetUpList ();
	}
}
