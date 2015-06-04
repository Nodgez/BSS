using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollPanel : MonoBehaviour {

	public Scrollbar scrollBar;
	private Vector3 startingPosition;
	public Vector3 endPosition;

	void Start () 
	{
		startingPosition = transform.position;
		endPosition = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width * endPosition.x,
		                                                          Screen.height * endPosition.y,
		                                                          10));
	}

	void Update () 
	{
		transform.position = Vector3.Lerp (startingPosition, endPosition, scrollBar.value);
	}
}
