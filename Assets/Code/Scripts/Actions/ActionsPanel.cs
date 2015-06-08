using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionsPanel : MonoBehaviour {

	public GameObject[] actions;
	private GameObject _actionInstance;
	private SlidingPanel masterSlidingPanel;

	void Start()
	{
		//masterSlidingPanel = GameObject.Find ("MasterMenuSlider").GetComponent<SlidingPanel> ();
		///masterSlidingPanel.onSlideBack += CloseAction;
	}

	public void StartAction(int actionIndex)
	{
		if (_actionInstance != null)
			CloseAction ();

		_actionInstance = Instantiate <GameObject>(actions [actionIndex]);
		RectTransform actionTransform = _actionInstance.transform as RectTransform;
		actionTransform.SetParent(this.transform);
		actionTransform.localScale = Vector3.one;
		actionTransform.SetAsFirstSibling ();

		RectTransform childTransform = _actionInstance.GetComponentInChildren<RectTransform> ();
		childTransform.offsetMax = Vector2.zero;
		childTransform.offsetMin = Vector2.zero;
	}

	public void CloseAction()
	{
		Destroy (_actionInstance);
	}
}
