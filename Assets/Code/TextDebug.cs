using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextDebug : MonoBehaviour {

	public Text obj;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		obj.text = GestureHandler.gestureState.ToString();

	}
}
