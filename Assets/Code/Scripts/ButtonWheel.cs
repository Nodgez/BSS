using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

//This class manages the buttons dissplayed by the main menu
public class ButtonWheel : MonoBehaviour {

	public MenuButton[] buttons;

	private WheelPoint[] _wheelPoints;
	private Vector3 _center;

	void Start () 
	{
		_wheelPoints = new WheelPoint[buttons.Length];
		_center = Vector3.zero;
		SetUpWheel (_center, 2);
		RectTransform trans = transform as RectTransform;
		trans.offsetMin = new Vector2 (0, Screen.height * 0.5f);
	}

	void Update () 
	{
		//checks for a finised swipe event
		if (GestureHandler.gestureState != GestureState.SwipeCompleted)
			return;
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
			Vector3 buttonPos = buttons[i].transform.position;		//button's position
			for(int j = 0; j < _wheelPoints.Length;j++)
			{
				//ensure that both rounded to check accurately
				RoundVector(ref buttonPos);
				RoundVector(ref _wheelPoints[j].position);

				//if the button is not at this point in the wheel then continue to the next wheel point
				if(buttonPos != _wheelPoints[j].position)
					continue;

				bool movingLeft =  incrementIndex == 1;

				//start interpolating button to the next wheel point values
				if(j + incrementIndex >= _wheelPoints.Length)
					buttons[i].StartInterpolation(_wheelPoints[0],movingLeft);
				else if(j + incrementIndex < 0)
					buttons[i].StartInterpolation(_wheelPoints[_wheelPoints.Length -1], movingLeft);
				else
					buttons[i].StartInterpolation(_wheelPoints[j + incrementIndex], movingLeft);

				break;	//exit the search for wheel points
			}
		}
	}

	void SetUpWheel(Vector3 center, float size)
	{
		float yMax = 0;
		int halfway = _wheelPoints.Length / 2;
		float angle = 0;

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
			if(i == 0)
				_wheelPoints[i].interactable = true;
//			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			go.transform.position = point;
		}

		for(int o = 0; o < _wheelPoints.Length;o++)
		{
			_wheelPoints[o].FindScale(_wheelPoints[0].position.y, yMax);
		}

		for(int j = 0; j < buttons.Length;j++)
		{
			buttons[j].transform.position = _wheelPoints[j].position + transform.position; 
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
}

[Serializable]
public struct WheelPoint
{
	public Vector3 position;
 	public Vector3 scale;
	public float opactity;
	public int siblingIndex;
	public bool interactable;

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
