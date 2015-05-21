using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuText : MonoBehaviour {

	Text txt;
	Text childText;
	void Start () 
	{
		txt = GetComponent<Text> ();
		childText = transform.GetChild(0).GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		string buttonName = transform.parent.GetChild (6).name;
		txt.text = buttonName;
		childText.text = buttonName;
	}
}
