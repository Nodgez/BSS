using UnityEngine;
using System.Collections;

public class CloseSkillButton : MonoBehaviour {

	public void FindAndClose()
	{
		Menu openMenu = GameObject.FindObjectOfType<Menu> ();
		if (openMenu != null)
			openMenu.CloseOpenPanels ();
	}
}
