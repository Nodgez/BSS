using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ButtonWheel : MonoBehaviour {

	public MenuButton[] buttons;
	public int i_SelectedButton = 0;

	public WheelPoint[] _wheelPoints;
	Vector3 center;

	void Start () 
	{
		_wheelPoints = new WheelPoint[buttons.Length];
		center = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
		SetUpWheel (center, 100);
	}

	void Update () 
	{
		if(CheckDeltaSwipe())
		{
			for(int i = 0;i < buttons.Length;i++)
			{
				buttons[i].GetComponent<Button>().interactable = false;
				Vector3 buttonPos = buttons[i].transform.position;
				for(int j = 0; j < _wheelPoints.Length;j++)
				{
					RoundVector(ref buttonPos);
					RoundVector(ref _wheelPoints[j].position);

					if(buttonPos != _wheelPoints[j].position)
					{
						if(i == _wheelPoints.Length -1 && j == _wheelPoints.Length -1)
							Debug.Log("Last button Checked");
						continue;
					}

					if(j + 1 >= _wheelPoints.Length)
						buttons[i].StartInterpolation(_wheelPoints[0]);
					else if(j + 1 < _wheelPoints.Length)
						buttons[i].StartInterpolation(_wheelPoints[j + 1]);
					else
						Debug.Log(buttons[i].gameObject.name + " Failed to Start Interpolation");

					break;
				}
			}
			i_SelectedButton --;
			if(i_SelectedButton < 0)
				i_SelectedButton = buttons.Length - 1;

			buttons[i_SelectedButton].GetComponent<Button>().interactable = true;
		}
	}

	void SetUpWheel(Vector3 center, float size)
	{
		float yMax = 0;
		int halfway = _wheelPoints.Length / 2;
		float angle = 0;
		center = transform.position;
		for (int i = 0; i < _wheelPoints.Length; i++)
		{
			buttons[i].GetComponentInChildren<Text>().text = "Button : " + i;

			angle = (2 * Mathf.PI) / buttons.Length * i;	
			float x = center.x + (size * 1.5f) * Mathf.Cos (-angle - (90 * Mathf.Deg2Rad));	
			float y = center.y + size * Mathf.Sin (-angle - (90 * Mathf.Deg2Rad));
			y = (float)Math.Round((double)y, 2);
			Debug.Log("Y in SetUp : " + y);

			if(i == halfway)
			{
				yMax = y;
				_wheelPoints[i].siblingIndex = 0;
			}
			else if(i < halfway)
				_wheelPoints[i].siblingIndex = (_wheelPoints.Length - 1) - (i * 2);
			else
				_wheelPoints[i].siblingIndex = Mathf.Abs((_wheelPoints.Length - 1) - (i * 2)) - 1;
			
			Vector3 point = new Vector3 (x, y, 0);	
			_wheelPoints[i].position = point;
		}

		for(int o = 0; o < _wheelPoints.Length;o++)
		{
			_wheelPoints[o].FindScale(_wheelPoints[0].position.y, yMax);
		}

		for(int j = 0; j < buttons.Length;j++)
		{
			buttons[j].transform.position = _wheelPoints[j].position; 
			buttons[j].transform.localScale = _wheelPoints[j].scale;
			buttons[j].transform.SetSiblingIndex(_wheelPoints[j].siblingIndex);
			buttons[j].GetComponent<Image>().color = new Color(1,1,1,_wheelPoints[j].opactity);	
		}
	}

	void RoundVector(ref Vector3 vec)
	{
		float x = (float)Math.Round ((double)vec.x, 2);
		float y = (float)Math.Round ((double)vec.y, 2);
		float z = (float)Math.Round ((double)vec.z, 2);

		vec = new Vector3 (x, y, z);
	}

	bool CheckDeltaSwipe()
	{
		if (Input.touchCount < 1)
			return false;

		Vector3 delta = Input.GetTouch (0).deltaPosition;
		Vector3 start = Input.GetTouch (0).position;

		if (Vector3.Distance (start, delta) > 1)
			return true;
		else
			return false;
	}
}

[Serializable]
public struct WheelPoint
{
	public Vector3 position;
 	public Vector3 scale;
	public float opactity;
	public int siblingIndex;

	public void FindScale(float min, float max)
	{
		float difference = max - min;
		float percent = difference * 0.01f;
		float scalar = Mathf.Abs((position.y - min) / percent);
		scalar *= 0.01f;
		scalar = 1.0f - scalar;
		scalar = scalar * 0.75f + 0.25f;
		scale = new Vector3 (scalar, scalar, 1);
		opactity = scalar;
	}
}
