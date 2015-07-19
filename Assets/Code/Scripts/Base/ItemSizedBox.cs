using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum ExpansionDirection
{
	Left,
	Right
}

public class ItemSizedBox : MonoBehaviour {

	private RectTransform rectTransform;
	public int size;
	public bool sizeBasedOnChildren = true;
	int storedSize;

	void Start()
	{
		if (sizeBasedOnChildren)
			size = transform.childCount;
	}

	// Update is called once per frame
	void Update () {
		if (sizeBasedOnChildren)
			size = transform.childCount;

		if (size == storedSize)
			return;
		rectTransform = transform as RectTransform;
		float heightOfBox = size * 40;
		rectTransform.offsetMax = new Vector2 (rectTransform.offsetMax.x, 0);
		rectTransform.offsetMin = new Vector2 (rectTransform.offsetMin.x, -heightOfBox);

		storedSize = size;
	}
}
