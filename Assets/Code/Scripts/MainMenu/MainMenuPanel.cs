using UnityEngine;
using System.Collections;

public class MainMenuPanel : SlidingPanel {

	private RectTransform rectTransform;

	void Start () 
	{
		rectTransform = transform as RectTransform;
		rectTransform.offsetMax = new Vector2 (Screen.width, 0);
		onSlideComplete += delegate {
			ButtonLayout layout = GetComponentInChildren<ButtonLayout>();
			layout.enabled = !layout.enabled;
		};
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public void QuitApp()
	{
		Application.Quit ();
	}
}
