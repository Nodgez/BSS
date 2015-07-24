using UnityEngine;
using System.Collections;

public class EscapeListener : MonoBehaviour {

	public SlidingPanel mainMenuPanel;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
		
			if(mainMenuPanel.IsSlid)
			{
				mainMenuPanel.SlideView();
				GetComponent<CloseSkillButton>().FindAndClose();
			}
			else
				Application.Quit ();
		}
	}
}
