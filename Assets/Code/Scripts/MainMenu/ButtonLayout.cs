using UnityEngine;
using System;
using System.Collections;

public class ButtonLayout : MonoBehaviour {

	public MenuButton[] buttons;
	protected ButtonPoint[] buttonPoints;

	protected virtual void Update () 
	{
		//checks for a finised swipe event
		if (GestureHandler.gestureState != GestureState.Swiping)
			return;
		//checks all buttons have completed interpolating
		for(int a = 0; a < buttons.Length;a++)
		{
			if(!buttons[a].interpComplete)
				return;
		}
		
		//sets direction of wheel cycle
		int incrementIndex = (int)GestureHandler.GetSwipeDirection;
		
		for(int i = 0;i < buttons.Length;i++)
		{
			Vector3 buttonPos = buttons[i].transform.position;		//button's position
			for(int j = 0; j < buttonPoints.Length;j++)
			{
				//ensure that both rounded to check accurately
				MathExt.RoundVector(ref buttonPos);
				MathExt.RoundVector(ref buttonPoints[j].position);
				
				//if the button is not at this point in the wheel then continue to the next wheel point
				if(buttonPos != buttonPoints[j].position)
					continue;
				
				bool movingLeft = GestureHandler.GetSwipeDirection == SwipeDirection.Left;
				
				//start interpolating button to the next wheel point values
				if(j + incrementIndex >= buttonPoints.Length)
					buttons[i].StartInterpolation(buttonPoints[0],movingLeft);
				else if(j + incrementIndex < 0)
					buttons[i].StartInterpolation(buttonPoints[buttonPoints.Length -1], movingLeft);
				else
					buttons[i].StartInterpolation(buttonPoints[j + incrementIndex], movingLeft);
				
				break;	//exit the search for wheel points
			}
		}
	}
}

public struct ButtonPoint
{
	public Vector3 position;
	public Vector3 scale;
	public float opacity;
	public int siblingIndex;
	public bool interactable;
	
	public void FindScale(float min, float max)
	{
		float difference = max - min;
		float percent = difference * 0.01f;
		float scalar = Mathf.Abs((position.x - min) / percent);
		scalar *= 0.01f; // normalize it
		scalar = 1.0f - scalar;
		opacity = scalar;
		scalar = scalar * 0.4f + 0.6f;
		scale = new Vector3 (scalar, scalar, 1);
	}
}
