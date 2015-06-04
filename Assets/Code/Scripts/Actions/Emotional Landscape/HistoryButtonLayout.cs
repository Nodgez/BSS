using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HistoryButtonLayout : LineLayout {

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
			buttons [i].transform.localScale = Vector3.one;
			buttons [i].GetComponentInChildren<Text> ().text = graphData [i].date.ToShortDateString ();
			Button clickableButton = buttons [i].GetComponent<Button> ();
			clickableButton.onClick.AddListener (delegate {
				elMenu.SwapGraphInfo (graphData [currentIndex].date);
			});
		}
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
	}
}
