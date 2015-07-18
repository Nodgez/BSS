using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour{

	private Vector3 sc_InterpStart;
	private float col_InterpStart;
	private Vector3 pos_InterpStart;
	private float _interpPercent = 1;
	private Image _image;				//the button Image
	private float interpSpeed = 5;
	private Button _button;
	private ButtonPoint targetPoint;

	public ButtonPoint currentPoint;
	public bool _interactable;
	public bool interpComplete = true;	//Used to check if all butons have finished interpolating
	public Vector2 smallResSize = new Vector2(256,256);
	public Vector2 largeResSize =new Vector2(364, 364);

	void Start () 
	{
		_button = GetComponent<Button> ();
		RectTransform buttonTransform =_button.transform as RectTransform;
		//gets the image component
		_image = GetComponent<Image> ();
		int width = Screen.width;
		if(width < 1280)
			buttonTransform.sizeDelta = smallResSize;
		else
			buttonTransform.sizeDelta = largeResSize;
	}

	public void StartInterpolation(SwipeDirection swipeDir)
	{
		targetPoint = new ButtonPoint ();
		if (swipeDir == SwipeDirection.Left)
			targetPoint = currentPoint.leftPoint;
		else
			targetPoint = currentPoint.rightPoint;

		//reset interpolation values
		//Debug.Log ("Interp Started");
		_interpPercent = 0;
		interpComplete = false;

		pos_InterpStart = transform.position;
		sc_InterpStart = transform.localScale;
		col_InterpStart = _image.color.a;

		_interactable = targetPoint.interactable;
	}

	public void Interpolate(float amount)
	{
		if (interpComplete)
			return;
		//increment interpolation and clamp it value
		_interpPercent += amount;
		_interpPercent = Mathf.Clamp (_interpPercent, 0, 1);

		transform.position = Vector3.Lerp (pos_InterpStart, targetPoint.position, _interpPercent);
		transform.localScale = Vector3.Lerp (sc_InterpStart, targetPoint.scale, _interpPercent);
		_image.color = new Color (1, 1, 1, Mathf.Lerp (col_InterpStart, targetPoint.opacity, _interpPercent));

		if (_interpPercent == 1) {
			currentPoint = targetPoint;
			interpComplete = true;
			_interpPercent = 0;
			_button.interactable = _interactable;
			transform.SetSiblingIndex (targetPoint.siblingIndex);
		}

		else if (_interpPercent == 0f) 
		{
			interpComplete = true;
			//Debug.Log("InterpComplete");
		}
	}

	public void AutoInterpolate(float increment)
	{
		if(_interpPercent > 0.5f)
			Interpolate (increment);
		else
			Interpolate (-increment);

	}
}
