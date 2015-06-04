using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EmotionDisplay : MonoBehaviour,IDragHandler {

	public Button button;
	public Emotion emotion;
	private ELMenu menu;
	private UnityAction buttonAction;
	private RectTransform rectTransform;
	private RectTransform canvasRectTransform;
	private Vector2 pointerOffset;

	void Start () 
	{
		menu = GameObject.Find ("ELMenu").GetComponent<ELMenu> ();
		buttonAction = delegate {
			menu.RemoveEmotionFromGraph (emotion.emotionName);
		};

		button.onClick.AddListener(buttonAction);

		rectTransform = transform as RectTransform;
		canvasRectTransform = menu.transform as RectTransform;

		EmotionDisplay[] emoDisplayRects = GameObject.FindObjectsOfType<EmotionDisplay> ();
		foreach(EmotionDisplay emoDisplay in emoDisplayRects)
		{
			if(emoDisplay == this)
				continue;
			Rect thisRect = button.image.rectTransform.rect;
			Rect otherRect = emoDisplay.button.image.rectTransform.rect;

			if(thisRect.Overlaps(otherRect))
			{
				Debug.Log("OverlapDetected");
			}
		}
	}

	public void OnDrag (PointerEventData data) {
		if (rectTransform == null)
			return;

		//Vector2 pointerPostion = ClampToWindow (data);
		
		Vector2 localPointerPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (
			canvasRectTransform, data.position, data.pressEventCamera, out localPointerPosition
		)) {
			rectTransform.localPosition = localPointerPosition - pointerOffset;
		}
	}

	Vector2 ClampToWindow (PointerEventData data) {
		Vector2 rawPointerPosition = data.position;
		
		Vector3[] canvasCorners = new Vector3[4];
		canvasRectTransform.GetWorldCorners (canvasCorners);
		
		float clampedX = Mathf.Clamp (rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
		float clampedY = Mathf.Clamp (rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);
		
		Vector2 newPointerPosition = new Vector2 (clampedX, clampedY);
		return newPointerPosition;
	}

}
