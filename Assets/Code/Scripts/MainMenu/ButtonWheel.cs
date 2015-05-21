using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

//This class manages the buttons dissplayed by the main menu
public class ButtonWheel : ButtonLayout {
	
	private Vector3 _center;

	void Start () 
	{
		buttonPoints = new ButtonPoint[buttons.Length];
		_center = Vector3.zero;
		SetUpWheel (_center, 2);
		RectTransform trans = transform as RectTransform;
		trans.offsetMin = new Vector2 (0, Screen.height * 0.5f);
	}

	protected override void Update () 
	{
		base.Update ();
	}

	void SetUpWheel(Vector3 center, float size)
	{
		float yMax = 0;
		int halfway = buttonPoints.Length / 2;
		float angle = 0;

		for (int i = 0; i < buttonPoints.Length; i++)
		{
			buttons[i].GetComponentInChildren<Text>().text = "Button : " + i;

			angle = (2 * Mathf.PI) / buttons.Length * i;	
			float x = center.x + (size * 1.5f) * Mathf.Cos (-angle - (90 * Mathf.Deg2Rad));	
			float y = center.y + size * Mathf.Sin (-angle - (90 * Mathf.Deg2Rad));
			y = (float)Math.Round((double)y, 2);

			if(i == halfway)
			{
				yMax = y;
				buttonPoints[i].siblingIndex = 0;
			}
			else if(i < halfway)
				buttonPoints[i].siblingIndex = (buttonPoints.Length - 1) - (i * 2);
			else
				buttonPoints[i].siblingIndex = Mathf.Abs((buttonPoints.Length - 1) - (i * 2)) - 1;
			
			Vector3 point = new Vector3 (x, y, 0);	
			buttonPoints[i].position = point;
			if(i == 0)
				buttonPoints[i].interactable = true;
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.position = point;
		}

		for(int o = 0; o < buttonPoints.Length;o++)
		{
			buttonPoints[o].FindScale(buttonPoints[0].position.y, yMax);
		}

		for(int j = 0; j < buttons.Length;j++)
		{
			buttons[j].transform.position = buttonPoints[j].position + transform.position; 
			buttons[j].transform.localScale = buttonPoints[j].scale;
			buttons[j].transform.SetSiblingIndex(buttonPoints[j].siblingIndex);
			buttons[j].GetComponent<Image>().color = new Color(1,1,1,buttonPoints[j].opacity);	
		}
	}

	void RoundVector(ref Vector3 vec)
	{
		float x = (float)Math.Round ((double)vec.x, 2);
		float y = (float)Math.Round ((double)vec.y, 2);
		float z = (float)Math.Round ((double)vec.z, 2);

		vec = new Vector3 (x, y, z);
	}
}
