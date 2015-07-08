using UnityEngine;
using System.Collections;

public class EmotionsPanel : MonoBehaviour {

	private ItemSizedBox sizeScript;
	private ELMenuEmotions containedEmotions;

	// Use this for initialization
	void Start () {
		sizeScript = GetComponent<ItemSizedBox> ();
		containedEmotions = GetComponent<ELMenuEmotions> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
