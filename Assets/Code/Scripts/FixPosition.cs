using UnityEngine;
using System.Collections;

public class FixPosition : MonoBehaviour {

	public Vector3 offsetPosition;
	public bool plusScreenWidth;

	void Start () 
	{
		Vector3 newPosition = offsetPosition + new Vector3 (0, Screen.height * 0.5f, 0);
		if (plusScreenWidth)
			newPosition += new Vector3(Screen.width, 0,0);
		transform.position = Camera.main.ScreenToWorldPoint (newPosition);
	}
}
