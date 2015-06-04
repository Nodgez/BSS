using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject[] disabledGameObjects;
	protected List<SlidingPanel> openPanels = new List<SlidingPanel> ();
	public SlidingPanel[] slidingPanels;

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	public void CloseOpenPanels()
	{
		foreach (SlidingPanel panel in slidingPanels)
		{
			if(panel.IsSlid)
				panel.SlideView();
		}
	}

	protected void SaveScreenState()
	{
		foreach (SlidingPanel panel in slidingPanels)
		{
			if(openPanels.Contains(panel))
			{
				if(!panel.IsSlid)
					openPanels.Remove(panel);
				continue;
			}
			
			if (panel.IsSlid)
				openPanels.Add (panel);
		}
	}

	protected void ToggleDisabledObjects()
	{
		foreach(GameObject go in disabledGameObjects)
		{
			if(go == null)
				continue;
			if(go.activeSelf)
				go.SetActive(false);
			else
				go.SetActive(true);
		}
	}
}
