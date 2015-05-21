using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollRectDebug : MonoBehaviour {

	ScrollRect rect;
	Mask mask;
	Vector2 scrollPosition;

	void Start () 
	{
		rect = GetComponent<ScrollRect> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log (rect.horizontalNormalizedPosition);
		if(Input.GetKey(KeyCode.Space))
		{
			rect.horizontalNormalizedPosition = 0.5f;
			rect.verticalNormalizedPosition = 0.5f;
		}
	}

}
