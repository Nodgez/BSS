	using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BindMainMenuSlide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SlidingPanel panel = GameObject.Find ("MasterMenuSlider").GetComponent<SlidingPanel>();
		GetComponent<Button> ().onClick.AddListener (delegate {
			panel.SlideView ();
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
