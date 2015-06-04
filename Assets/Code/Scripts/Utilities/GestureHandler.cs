using UnityEngine;
using System.Collections;

public enum GestureState 
{
	None,
	Swiping,
	Tapping,
	Holding,
	Completed
}

public enum SwipeDirection {
	Left = -1,
	Right = 1,
	Up = -1,
	Down = 1,
	None = 0
}

public class GestureHandler : MonoBehaviour 
{
	public static GestureState gestureState = GestureState.None;
	public static int tapCount;
	public static Vector2 touchDelta = Vector3.zero;
	
	public float tapTimeLimit = 0.25f;
	public float minSwipeDistance = 3;

	private Vector3 touchstart = Vector3.zero;
	private Vector3 touchCurrent = Vector3.zero;
	private float tapTimer;

	void Update()
	{
		if (gestureState == GestureState.Completed)
			gestureState = GestureState.None;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
 		CheckEventStateMouse ();
#endif

#if UNITY_WP8 || UNITY_ANDROID
		CheckGestureStateTouch();
#endif
	}

	#region Mouse Code
	void StartGestureMouse()
	{
		touchCurrent = Input.mousePosition;		
		touchstart = touchCurrent;
		gestureState = GestureState.Tapping;
	}
	
	void CheckGestureMoveMouse()
	{
		touchCurrent = Input.mousePosition;
		touchDelta = touchCurrent - touchstart;
		
		if (touchDelta.sqrMagnitude > minSwipeDistance)
			gestureState = GestureState.Swiping;
	}
	
	void EndGestureMouse ()
	{
		if(gestureState == GestureState.Tapping)
		{
			tapCount ++;
			tapTimer = 0;
		}

		else
			gestureState = GestureState.Completed;
	}
	
	void ResolveTapMouse()
	{
		tapTimer += Time.deltaTime;
		
		if (tapTimer > tapTimeLimit)
		{
			tapCount = 0;
			tapTimer = 0;
			
			if(Input.GetMouseButton(0))
				gestureState = GestureState.Holding;
			
			else
				gestureState = GestureState.Completed;
		}
	}
	
	void CheckEventStateMouse()
	{
		if (Input.GetMouseButtonDown (0))
			StartGestureMouse ();
		
		if (gestureState == GestureState.None)
			return;
		
		if (Input.GetMouseButton (0))
			CheckGestureMoveMouse ();
		
		if (Input.GetMouseButtonUp (0)) 
			EndGestureMouse ();
		
		if (gestureState == GestureState.Tapping) 
			ResolveTapMouse ();
	}
	
	#endregion

	#region Touch Code
	void StartGestureTouch()
	{
		tapTimer = 0;								//tap timer reset
		tapCount++;									//tap count iterated
		touchstart = Input.GetTouch (0).position;	//get the touse starting position
		gestureState = GestureState.Tapping;		//set the gesture type to tapping
	}

	void UpdateTimer ()
	{
		if (tapCount == 0)
			return;

		tapTimer += Time.deltaTime;
		if (tapTimer > tapTimeLimit)
			tapCount = 0;
	}

	void CheckGestureStateTouch()
	{
		UpdateTimer ();

		if (Input.touchCount == 0)
			return;

		touchDelta = Input.GetTouch (0).deltaPosition;
		switch (Input.GetTouch (0).phase) 
		{
		case TouchPhase.Began:
			StartGestureTouch ();
			break;

		case TouchPhase.Moved:
				if (touchDelta.sqrMagnitude > minSwipeDistance)
					gestureState = GestureState.Swiping;
			break;
	
			case TouchPhase.Stationary:
				if (touchDelta.sqrMagnitude < minSwipeDistance && tapTimer > tapTimeLimit)
					gestureState = GestureState.Holding;
			break;

		case TouchPhase.Ended:
				gestureState = GestureState.Completed;
			break;
		}
	}
	#endregion

	public static SwipeDirection GetSwipeDirection
	{
		get
		{
//			if(touchDelta.x > touchDelta.y)
//			{
				if(touchDelta.x > 0)
					return SwipeDirection.Right;
				else if(touchDelta.x < 0)
					return SwipeDirection.Left;
			//}

//			else if(touchDelta.x < touchDelta.y)
//			{
//				if(touchDelta.y > 0)
//					return SwipeDirection.Up;
//				else if(touchDelta.y < 0)
//					return SwipeDirection.Down;
//			}

			return SwipeDirection.None;
		}
	}

	public static bool OnTouchOver(GameObject obj,float minDistance)
	{
		if(Input.touchCount < 1)
			return false;

		Vector3 objOnScreen = Camera.main.WorldToScreenPoint (obj.transform.position);
		float distance = Vector3.Distance (objOnScreen, Input.GetTouch (0).position);
		if (distance < minDistance)
			return true;
		else 
			return false;
	}

	public static bool OnCursorOver(GameObject obj,float minDistance)
	{
		Vector3 objOnScreen = Camera.main.WorldToScreenPoint (obj.transform.position);
		float distance = Vector3.Distance (objOnScreen, Input.mousePosition);
		if (distance < minDistance)
			return true;
		else 
			return false;
	}

	public static bool OnTouchDown(GameObject obj,float minDistance)
	{
		if(Input.touchCount < 1)
			return false;

		Vector3 objOnScreen = Camera.main.WorldToScreenPoint (obj.transform.position);
		float distance = Vector3.Distance (objOnScreen, Input.GetTouch (0).position);
		if (distance < minDistance && Input.GetTouch(0).phase == TouchPhase.Began)
			return true;
		else 
			return false;
	}
	
	public static bool OnCursorDown(GameObject obj,float minDistance)
	{
		Vector3 objOnScreen = Camera.main.WorldToScreenPoint (obj.transform.position);
		float distance = Vector3.Distance (objOnScreen, Input.mousePosition);
		if (distance < minDistance && Input.GetMouseButtonDown(0))
			return true;
		else 
			return false;
	}
}
