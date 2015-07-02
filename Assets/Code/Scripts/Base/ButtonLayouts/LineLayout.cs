using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LineLayout : ButtonLayout {

	private Vector3 endOfLine;
	private Vector3 frontOfLine;

	protected override void Start ()
	{
		endOfLine = new Vector3 (Screen.width * 0.5f, Screen.height * 0.8f, 10);
		endOfLine = Camera.main.ScreenToWorldPoint(endOfLine);
		
		frontOfLine = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 10);
		frontOfLine = Camera.main.ScreenToWorldPoint(frontOfLine);

		base.Start ();
		SetUpLine (buttons.Length);
	}

	protected override void Update ()
	{
		base.Update ();
	}

	void SetUpLine(int numberOfButtons)
	{
		float interpValue = 0;
		int index = 0;
		
		buttons[numberOfButtons -1]._interactable = true;
		while(index < numberOfButtons)
		{
			Vector3 pos = Vector3.Lerp(frontOfLine,endOfLine,interpValue);
			buttonPoints[index].position = pos;
			buttonPoints[(numberOfButtons - 1) - index].siblingIndex = index;
			interpValue += 1.0f / numberOfButtons;
			index ++;
		}
		buttonPoints [0].interactable = true;
		for (int j = 0; j < numberOfButtons ; j++) {
			buttonPoints [j].FindScale (buttonPoints [0].position.y, buttonPoints [buttonPoints.Length - 1].position.y, false);

			int rightIndex = j + 1 <= numberOfButtons -1 ? j + 1 : 0;
			int leftIndex = j - 1 > - 1 ? j - 1 : numberOfButtons -1;
			
			buttonPoints[j].leftPoint = buttonPoints[leftIndex];
			buttonPoints[j].rightPoint = buttonPoints[rightIndex];
		}
		
		for(int i = 0; i < numberOfButtons; i++)
		{
			int bpIndex = (numberOfButtons -1) - i;
			buttons[i].transform.position = buttonPoints[bpIndex].position + transform.position;
			buttons[i].transform.localScale = buttonPoints[bpIndex].scale;
			buttons[i].transform.SetSiblingIndex(buttonPoints[bpIndex].siblingIndex);
			buttons[i].GetComponent<Button>().image.color = new Color(1,1,1,buttonPoints[bpIndex].opacity);
			buttons[i].currentPoint = buttonPoints[bpIndex];
		}
	}
}
