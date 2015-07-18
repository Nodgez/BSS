using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EmotionListItem : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler  {

	public Emotion emotion;
	public EmotionDisplay emoDisplayPrefab;
	private RectTransform canvasRectTransform;
	private RectTransform landscapeTransform;
	private ELMenu elMenu;
	private Vector2 pointerOffset;

	// Use this for initialization
	void Start () {
		canvasRectTransform = GameObject.Find ("Canvas").transform as RectTransform;
		landscapeTransform = GameObject.Find ("Landscape").transform as RectTransform;
		elMenu = GameObject.FindObjectOfType<ELMenu> ();
	}

	public void OnBeginDrag(PointerEventData data)
	{
		transform.SetParent (canvasRectTransform);
	}

	public void OnDrag (PointerEventData data) {
		RectTransform rectTransform = transform as RectTransform;
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
		///Maybe change this to elMenu for adding to the landscape
		elMenu.AddEmotionToGraph (transform.position, emotion);
		Destroy (this.gameObject);
		//create emotion display at mouse position or resolve for max and min distances
	}
}
