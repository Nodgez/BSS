using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LineLayout : ButtonLayout {

	private Vector3 endOfLine;
	private Vector3 frontOfLine;

	protected override void Start ()
	{
		endOfLine = new Vector3 (Screen.width * 0.5f, Screen.height * 0.6f, 10);
		endOfLine = Camera.main.ScreenToWorldPoint(endOfLine);
		
		frontOfLine = new Vector3 (Screen.width * 0.5f, Screen.height * 0.45f, 10);
		frontOfLine = Camera.main.ScreenToWorldPoint(frontOfLine);

		base.Start ();
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
		
		buttons[0]._interactable = true;
		while(index < numberOfButtons)
		{
			Vector3 pos = Vector3.Lerp(frontOfLine,endOfLine,interpValue);
			buttonPoints[index].position = pos;
			buttonPoints[(numberOfButtons - 1) - index].siblingIndex = index;
			interpValue += 1.0f / numberOfButtons;
			index ++;
		}
		buttonPoints [0].interactable = true;
		for (int j = 0; j < buttonPoints.Length;j++)
			buttonPoints[j].FindScale (buttonPoints [0].position.y, buttonPoints [buttonPoints.Length - 1].position.y,false);
		
		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i].transform.position = buttonPoints[i].position + transform.position;
			buttons[i].transform.localScale = buttonPoints[i].scale;
			buttons[i].transform.SetSiblingIndex(buttonPoints[i].siblingIndex);
			buttons[i].GetComponent<Button>().image.color = new Color(1,1,1,buttonPoints[i].opacity);
		}
	}
}
