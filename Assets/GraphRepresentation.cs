using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GraphRepresentation : MonoBehaviour {

	private ScrollRect _scrollRect;

	void Start()
	{
		_scrollRect = GetComponent<ScrollRect> ();
	}

	public void Shift(Vector2 directon)
	{
		_scrollRect.normalizedPosition = directon;
	}
}
