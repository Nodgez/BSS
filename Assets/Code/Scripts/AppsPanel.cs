using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum AppActions{EmoScape = 0, Pacer = 1, Journal = 2}
public class AppsPanel : MonoBehaviour {

	public GameObject[] actions;

	private GameObject _actionInstance;
	private AppActions e_CurrentAction;

	void Awake () 
	{
	}

	void Update () 
	{
	}

	public void CloseAction()
	{
		Destroy (_actionInstance);
	}

	public void StartAction(int actionIndex)
	{
		if (_actionInstance != null)
			CloseAction ();

		e_CurrentAction = (AppActions)actionIndex;
		_actionInstance = Instantiate <GameObject>(actions [actionIndex]);
		RectTransform actionTransform = _actionInstance.transform as RectTransform;
		actionTransform.SetParent(this.transform);
		actionTransform.localScale = Vector3.one;

		RectTransform childTransform = _actionInstance.GetComponentInChildren<RectTransform> ();
		childTransform.offsetMax = Vector2.zero;
		childTransform.offsetMin = Vector2.zero;
	}
}
