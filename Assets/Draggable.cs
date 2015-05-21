using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour,IPointerDownHandler, IDragHandler,IPointerUpHandler{

	private Vector2 pointerOffset;
	private RectTransform landscapeRectTransform;
	private RectTransform emotionRectTransform;

	public GameObject landscape;

	void Awake()
	{
		landscapeRectTransform = landscape.transform as RectTransform;
		emotionRectTransform = transform as RectTransform;
	}
	
	public void OnPointerDown(PointerEventData data)
	{
		emotionRectTransform.transform.SetParent (landscape.transform);
		RectTransformUtility.ScreenPointToLocalPointInRectangle (emotionRectTransform,
		                                                        data.position,
		                                                        data.pressEventCamera,
		                                                        out pointerOffset);
	}

	public void OnDrag(PointerEventData data)
	{
		if (emotionRectTransform == null)
			return;

		Vector2 localPointerPosition;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(landscapeRectTransform,
		                                                           data.position,
		                                                           data.pressEventCamera,
		                                                           out localPointerPosition))
	   	{

			emotionRectTransform.localPosition = localPointerPosition - pointerOffset;
		}
	}

	public void OnPointerUp(PointerEventData data)
	{}
}
