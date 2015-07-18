using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EmotionDisplay : MonoBehaviour,IDragHandler, IEndDragHandler {
	
	public Button button;
	public Emotion emotion;
	private ELMenu menu;
	private UnityAction buttonAction;
	private RectTransform rectTransform;
	private RectTransform canvasRectTransform;
	private Vector2 pointerOffset;
	private Vector3 screenPos;
	
	void Start () 
	{
		menu = GameObject.Find ("ELMenu").GetComponent<ELMenu> ();
		buttonAction = delegate {
			menu.RemoveEmotionFromGraph (emotion.emotionName);
			if(emotion.emotionType == EmotionType.Emergency)
			{
				menu.emotionsPanels[1].GetComponentInChildren<ELMenuEmotions>().ReturnEmotionToList(emotion);
			}
			else
			{
				menu.emotionsPanels[0].GetComponentInChildren<ELMenuEmotions>().ReturnEmotionToList(emotion);
			}
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
		
		Vector2 screenCenter = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width * 0.5f,
		                                                                   Screen.height * 0.5f));
		Vector2 localPointerPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (
			canvasRectTransform, data.position, data.pressEventCamera, out localPointerPosition
			)) {
			if(emotion.emotionType == EmotionType.Emergency){
				if(localPointerPosition.x < 0)
					localPointerPosition = new Vector2(0,localPointerPosition.y);
			}
			else if(emotion.emotionType == EmotionType.Positive){
				if(localPointerPosition.x > 0)
					localPointerPosition = new Vector2(0,localPointerPosition.y);
			}
			
			if(Vector2.Distance(localPointerPosition, screenCenter) > 200)
			{
				Vector2 direction = localPointerPosition - screenCenter;
				localPointerPosition = direction.normalized * 200;
			}
			
			rectTransform.localPosition = localPointerPosition - pointerOffset;
			emotion.position = transform.position;
		}
	}
	
	public void OnEndDrag(PointerEventData data)
	{
		ELMenu elMenu = GameObject.Find ("ELMenu").GetComponent<ELMenu> ();
		elMenu.SaveCurrentEmotionalState ();
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
