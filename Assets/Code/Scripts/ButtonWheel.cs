using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

//This class manages the buttons dissplayed by the main menu
public class ButtonWheel : MonoBehaviour {

	public MenuButton[] buttons;
	public int i_SelectedButton = 0;

	private WheelPoint[] _wheelPoints;
	private Vector3 _center;

	void Start () 
	{
		_wheelPoints = new WheelPoint[buttons.Length];
		_center = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
		SetUpWheel (_center, 2);
	}

	void Update () 
	{
		//checks for a finised swipe event
		if(GestureHandler.gestureState == GestureState.SwipeCompleted)
		{
			//checks all buttons have completed interpolating
			for(int a = 0; a < buttons.Length;a++)
			{
				if(!buttons[a].interpComplete)
					return;
			}

			//sets direction of wheel cycle
			int incrementIndex = 1;
			if(GestureHandler.touchDelta.x > 0)
				incrementIndex = -1;

			for(int i = 0;i < buttons.Length;i++)
			{
				buttons[i].GetComponent<Button>().interactable = false;	//disable button
				Vector3 buttonPos = buttons[i].transform.position;		//button's position
				for(int j = 0; j < _wheelPoints.Length;j++)
				{
					//ensure that both rounded to check accurately
					RoundVector(ref buttonPos);
					RoundVector(ref _wheelPoints[j].position);

					//if the button is not at this point in the wheel then continue to the next wheel point
					if(buttonPos != _wheelPoints[j].position)
						continue;

					//start interpolating button to the next wheel point values
					if(j + incrementIndex >= _wheelPoints.Length)
						buttons[i].StartInterpolation(_wheelPoints[0]);
					else if(j + incrementIndex < 0)
						buttons[i].StartInterpolation(_wheelPoints[_wheelPoints.Length -1]);
					else
						buttons[i].StartInterpolation(_wheelPoints[j + incrementIndex]);

					break;	//exit the search for wheel points
				}
			}
			//set the current button index
			i_SelectedButton += incrementIndex;
			if(i_SelectedButton < 0)
				i_SelectedButton = buttons.Length - 1;
			else if (i_SelectedButton >= buttons.Length)
				i_SelectedButton = 0;

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
		float distance = Vector3.Distance (start, delta);

		if (distance > 1)
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
