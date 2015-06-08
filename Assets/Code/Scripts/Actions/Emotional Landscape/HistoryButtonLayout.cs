using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HistoryButtonLayout : LineLayout {

	public Sprite[] moodColors = new Sprite[4];
	public List<GraphData> graphData;
	public MenuButton buttonPrefab;
	private ELMenu elMenu;

	protected override void Start ()
	{
		elMenu = GameObject.FindObjectOfType<ELMenu> ();
		graphData = elMenu.graphCollection;		
		buttons = new MenuButton[graphData.Count];

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
				elMenu.SwapGraphInfo (graphData [currentIndex].date);
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

	Sprite SolveImage(GraphData data)
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
