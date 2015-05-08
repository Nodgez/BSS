using UnityEngine;
using System.Collections;
//Tag: PlayerControls
public enum GestureState {None, Swiping, Tapping, Holding, Completed, SwipeCompleted};

public class GestureHandler : MonoBehaviour 
{
	public float tapTimeLimit = 0.25f;
	public float minSwipeDistance = 3;
	public static GestureState gestureState = GestureState.None;
	public static int tapCount;
	public static Vector3 touchDelta = Vector3.zero;
	private Vector3 touchstart = Vector3.zero;
	private Vector3 touchCurrent = Vector3.zero;
	private float tapTimer;
	public float myDeltaTime;

	void CalcDeltaTime()
	{
		if(Time.deltaTime == 0)
			return;

		myDeltaTime = Time.deltaTime;
	}

	void Update()
	{
		CalcDeltaTime ();

		if (gestureState == GestureState.Completed || gestureState == GestureState.SwipeCompleted)
			gestureState = GestureState.None;
#if UNITY_STANDALONE
 		CheckEventStateMouse ();
#endif

#if UNITY_WP8 || UNITY_ANDROID
		CheckEventStateTouch();
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
			gestureState = GestureState.SwipeCompleted;
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
		tapTimer = 0;
		tapCount++;
		touchstart = Input.GetTouch (0).position;
		gestureState = GestureState.Tapping;
	}
	
	void CheckGestureMoveTouch()
	{	
		if (touchDelta.sqrMagnitude > minSwipeDistance)
			gestureState = GestureState.SwipeCompleted;
	}
	
	void EndGestureTouch ()
	{
		if(Input.GetTouch(0).phase == TouchPhase.Ended)
			gestureState = GestureState.Completed;
	}
	
	void CheakHold()
	{
		if(touchDelta.sqrMagnitude < minSwipeDistance &&
		   tapTimer > tapTimeLimit)
			gestureState = GestureState.Holding;
	}

	void UpdateTimer ()
	{
		if (tapCount == 0)
			return;

		tapTimer += Time.deltaTime;
		if (tapTimer > tapTimeLimit)
			tapCount = 0;
	}

	void CheckEventStateTouch()
	{
		UpdateTimer ();

		if (Input.touchCount == 0)
			return;

		Vector3 deltaPos = new Vector3 (Input.GetTouch (0).deltaPosition.x,
		                                Input.GetTouch (0).deltaPosition.y,
		                                0);
		touchDelta = deltaPos;

		if (Input.GetTouch(0).phase == TouchPhase.Began)
			StartGestureTouch ();

		if(Input.GetTouch(0).phase == TouchPhase.Moved)
			CheckGestureMoveTouch();
	
			if(Input.GetTouch(0).phase == TouchPhase.Stationary)
				CheakHold();

			EndGestureTouch();
	}
	#endregion

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
