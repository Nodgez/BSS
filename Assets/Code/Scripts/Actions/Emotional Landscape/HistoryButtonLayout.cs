using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HistoryButtonLayout : LineLayout {

	public Sprite[] moodColors = new Sprite[4];
	public List<SaveData> graphData;
	public MenuButton buttonPrefab;
	private HistoryMenu hisMenu;

	protected override void Start ()
	{
		hisMenu = GameObject.FindObjectOfType<HistoryMenu> ();
		graphData = hisMenu.graphCollection;		
		buttons = new MenuButton[graphData.Count];

		if(buttons.Length <= 0)
			return;

		for (int i = 0; i < buttons.Length; i++)
		{
			int currentIndex = i;

			buttons [i] = Instantiate<MenuButton> (buttonPrefab);
			buttons [i].transform.SetParent (this.transform);
			buttons [i].transform.SetAsLastSibling();
			buttons [i].transform.localScale = Vector3.one;
			buttons [i].GetComponentInChildren<Text> ().text = graphData [i].date.ToShortDateString ();
			Button clickableButton = buttons [i].GetComponent<Button> ();
			clickableButton.onClick.AddListener (delegate {
				hisMenu.SwapGraphInfo (graphData [currentIndex].date);
			});

			for(int j = 0; j < buttons[i].transform.childCount;j++)
			{
				Image img = buttons [i].transform.GetChild(j).GetComponent<Image> ();
				if(img)
					img.sprite = SolveImage(graphData[currentIndex]);
			}
		}
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	Sprite SolveImage(SaveData data)
	{
		Vector2 graphCoords = data.normailzedGraphPosition;
		if(graphCoords.x > 0.5f && graphCoords.y > 0.5f)	//Emergency high energy
			return moodColors[0];
		if(graphCoords.x < 0.5f && graphCoords.y > 0.5f)	//Positive high energy
			return moodColors[1];
		if(graphCoords.x > 0.5f && graphCoords.y < 0.5f)	//Emergency low energy
			return moodColors[2];
		if(graphCoords.x < 0.5f && graphCoords.y < 0.5f)	//Positive low energy
			return moodColors[3];

		return moodColors[0];
	}
}
