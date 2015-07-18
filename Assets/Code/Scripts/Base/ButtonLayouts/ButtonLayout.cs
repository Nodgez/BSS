using UnityEngine;
using System;
using System.Collections;

public abstract class ButtonLayout : MonoBehaviour {

	public MenuButton[] buttons;
	protected ButtonPoint[] buttonPoints;
	GestureState previousState = GestureState.None;
	private SwipeDirection initialDirection;

	public ButtonPoint[] GetButtonPoints
	{
		get
		{
			return buttonPoints;
		}
	}

	protected virtual void Start()
	{
		buttonPoints = new ButtonPoint[buttons.Length];
		for(int i = 0; i < buttonPoints.Length;i++)
		{
			buttonPoints [i] = new ButtonPoint ();
		}
	}
	
	protected virtual void Update () 
	{
		//checks for a finised swipe event
		if (GestureHandler.gestureState == GestureState.Swiping) 
		{
			if (previousState != GestureState.Swiping)
			{
				initialDirection = GestureHandler.GetSwipeDirection;
				for (int i = 0; i < buttons.Length; i++)
					buttons [i].StartInterpolation (initialDirection);
			}

			else
			{
				for (int j = 0; j < buttons.Length; j++)
				{
					float interpAmount;
					if(initialDirection == SwipeDirection.Left)
						interpAmount = ((float)GestureHandler.GetSwipeDirection * 0.05f) * -1;
					else
						interpAmount = ((float)GestureHandler.GetSwipeDirection * 0.05f);
					buttons [j].Interpolate (interpAmount);
				}
			}
		}

		else
		{
			for (int k = 0; k < buttons.Length; k++)
			{
				if(!buttons[k].interpComplete)
				{
					buttons [k].AutoInterpolate (Time.deltaTime * 2);
				}
			}
		}
		previousState = GestureHandler.gestureState;
	}
}

public class ButtonPoint
{
	public Vector3 position;
	public Vector3 scale;
	public float opacity;
	public int siblingIndex;
	public bool interactable;

	public ButtonPoint leftPoint;
	public ButtonPoint rightPoint;
	
	public void FindScale(float min, float max, bool xRelative = true)
	{
		float difference = max - min;
		float percent = difference * 0.01f == 0  ? 1 : difference * 0.01f;
		float scalar = 0;
		if (xRelative)
			scalar = Mathf.Abs ((position.x - min) / percent);
		else
			scalar = Mathf.Abs ((position.y - min) / percent);
		scalar *= 0.01f; // normalize it
		scalar = 1.0f - scalar;
		opacity = scalar;
		scalar = scalar * 0.4f + 0.6f;
		scale = new Vector3 (scalar, scalar, 1);
	}
}
