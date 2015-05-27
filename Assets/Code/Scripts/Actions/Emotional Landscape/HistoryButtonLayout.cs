using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HistoryButtonLayout : ButtonLayout {

	public List<GraphData> graphData;
	public MenuButton buttonPrefab;
	private Vector3 endOfLine;
	private Vector3 frontOfLine;
	private ELMenu elMenu;

	protected override void Start ()
	{
		elMenu = GameObject.FindObjectOfType<ELMenu> ();
		graphData = elMenu.graphData;
		buttons = new MenuButton[graphData.Count];
		base.Start ();

		endOfLine = new Vector3 (Screen.width * 0.5f, Screen.height * 0.9f, 10);
		endOfLine = Camera.main.ScreenToWorldPoint(endOfLine);
		
		frontOfLine = new Vector3 (Screen.width * 0.5f, Screen.height * 0.1f, 10);
		frontOfLine = Camera.main.ScreenToWorldPoint(frontOfLine);

		for(int i = 0;i < buttons.Length;i++)
		{
			buttons[i] = Instantiate<MenuButton>(buttonPrefab);
			buttons[i].transform.SetParent(this.transform);
			buttons[i].transform.localScale = Vector3.one;
			buttons[i].GetComponentInChildren<Text>().text = graphData[i].date.ToShortDateString();
		}
		SetUpLine ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	void SetUpLine()
	{
		float interpValue = 0;
		int index = 0;
		int numberOfButtons = buttons.Length;

		while(index < numberOfButtons)
		{
			Vector3 pos = Vector3.Lerp(frontOfLine,endOfLine,interpValue);
			
			//SolveSiblingIndex();
			
			buttonPoints[index].position = pos;
			interpValue += 1.0f / numberOfButtons;
			index ++;
		}
		//buttonPoints [index].position = endOfLine;	//set up last point
		
		for (int j = 0; j < buttonPoints.Length;j++)
			buttonPoints[j].FindScale (buttonPoints [buttonPoints.Length / 2].position.x, buttonPoints [0].position.x);
		
		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i].transform.position = buttonPoints[i].position + transform.position;
			//buttons[i].transform.localScale = buttonPoints[i].scale;
			buttons[i].transform.SetSiblingIndex(buttonPoints[i].siblingIndex);
			buttons[i].GetComponent<Button>().image.color = new Color(1,1,1,buttonPoints[i].opacity);
		}
	}
}
