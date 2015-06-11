using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GraphRepresentation : MonoBehaviour {

	public ScrollRect _scrollRect;
	
	public void Shift(Vector2 directon)
	{
		_scrollRect.normalizedPosition = directon;
	}
}
