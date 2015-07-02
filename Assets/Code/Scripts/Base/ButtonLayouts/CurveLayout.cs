using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CurveLayout : ButtonLayout {

	private Vector3 _curveStart;
	private Vector3 _curveMiddle;
	private Vector3 _curveEnd;

	protected override void Start () 
	{
		base.Start ();
		_curveStart = new Vector3 (Screen.width * 0.1f, Screen.height * 0.6f, 10);
		_curveStart = Camera.main.ScreenToWorldPoint(_curveStart);

		_curveMiddle = new Vector3 (Screen.width * 0.5f, Screen.height * 0.4f, 10);
		_curveMiddle = Camera.main.ScreenToWorldPoint(_curveMiddle);

		_curveEnd = new Vector3 (Screen.width * 0.9f, Screen.height * 0.6f, 10);
		_curveEnd = Camera.main.ScreenToWorldPoint(_curveEnd);

		SetUpCurve (buttonPoints.Length -1);
	}
	
	protected override void Update ()
	{
		base.Update ();
	}

	void SetUpCurve(int numberOfButtons)
	{
		float interpValue = 0;
		int index = 0;
		int halfway = numberOfButtons / 2;

		while(index < numberOfButtons)
		{
			if(index == halfway)
			{
				buttonPoints[index].interactable = true;
				buttons[index]._interactable = true;
			}
			Vector3 pos = MathExt.Qerp(interpValue,
			                   _curveStart,
			                   _curveMiddle,
			                   _curveEnd);

			SolveSiblingIndex();

			buttonPoints[index].position = pos;
			interpValue += 1.0f / numberOfButtons;
			index ++;
		}
		buttonPoints [index].position = _curveEnd;	//set up last point

		for (int j = 0; j < buttonPoints.Length;j++)
		{
			buttonPoints[j].FindScale (buttonPoints [buttonPoints.Length / 2].position.x, buttonPoints [0].position.x);

			int rightIndex = j + 1 <= numberOfButtons ? j + 1 : 0;
			int leftIndex = j - 1 > - 1 ? j - 1 : numberOfButtons;

			buttonPoints[j].leftPoint = buttonPoints[leftIndex];
			buttonPoints[j].rightPoint = buttonPoints[rightIndex];
		}

		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i].transform.position = buttonPoints[i].position + transform.position;
			buttons[i].transform.localScale = buttonPoints[i].scale;
			buttons[i].transform.SetSiblingIndex(buttonPoints[i].siblingIndex);
			buttons[i].GetComponent<Button>().image.color = new Color(1,1,1,buttonPoints[i].opacity);
			buttons[i].currentPoint = buttonPoints[i];
		}
	}

	void SolveSiblingIndex()
	{
		int left = 0;
		int right = buttonPoints.Length - 1;
		int siblingIndex = 0;
		while(left != right)
		{
			buttonPoints[left].siblingIndex = siblingIndex++;
			buttonPoints[right].siblingIndex = siblingIndex++;
			left++;
			right--;

			if(siblingIndex > 10)
				return;
		}
		buttonPoints [left].siblingIndex = buttonPoints.Length - 1;
	}
}
