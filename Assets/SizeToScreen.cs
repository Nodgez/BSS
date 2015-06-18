using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SizeToScreen : MonoBehaviour {
	private RectTransform _rectTransform;
	public float factor = 0.8f;

	// Use this for initialization
	void Start () {
		_rectTransform = transform as RectTransform;
		_rectTransform.offsetMin = new Vector2 (_rectTransform.offsetMin.x, -Screen.height * factor);
	}
}
