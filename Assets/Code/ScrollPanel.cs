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
	}

	void Update () 
	{
		transform.position = Vector3.Lerp (startingPosition, endPosition, scrollBar.value);
	}
}
