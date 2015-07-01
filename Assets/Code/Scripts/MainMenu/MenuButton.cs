using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour{

	private Vector3 sc_InterpStart;
	private Vector3 sc_InterpEnd;
	private float col_InterpStart;
	private float col_InterpEnd;
	private Vector3 pos_InterpStart;
	private Vector3 pos_InterpEnd;
	private float _interpPercent = 1;
	private Image _image;				//the button Image
	private float interpSpeed = 5;
	public bool _interactable;
	private Button _button;

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

	void Update () 
	{
		//check lerp complete
		if (_interpPercent >= 1)
		{
			_button.interactable = _interactable;
			interpComplete = true;
			return;
		}

		//increment interpolation and clamp it value
		_interpPercent += Time.deltaTime * interpSpeed;
		_interpPercent = Mathf.Clamp (_interpPercent, 0, 1);

		//calculate the position, scale and opacity of the button using interpolation
		transform.position = Vector3.Lerp (pos_InterpStart, pos_InterpEnd, _interpPercent);
		transform.localScale = Vector3.Lerp (sc_InterpStart, sc_InterpEnd, _interpPercent);
		_image.color = new Color (1, 1, 1, Mathf.Lerp (col_InterpStart, col_InterpEnd, _interpPercent));
	}

	public void StartInterpolation(ButtonPoint buttonPoint, bool left)
	{
		//reset interpolation values
		_interpPercent = 0;
		interpComplete = false;
		_button.interactable = false;

		//set postition interp values
		pos_InterpEnd = buttonPoint.position;
		pos_InterpStart = transform.position;

		//set scale interp values
		sc_InterpStart = transform.localScale;
		sc_InterpEnd = buttonPoint.scale;

		//set opactity interp values
		col_InterpEnd = buttonPoint.opacity;
		col_InterpStart = _image.color.a;

		transform.SetSiblingIndex (buttonPoint.siblingIndex);
		_interactable = buttonPoint.interactable;
	}

	public void Interpolate(float amount)
	{
		//check lerp complete
		if (_interpPercent >= 1)
		{
			_button.interactable = _interactable;
			interpComplete = true;
			return;
		}
		
		//increment interpolation and clamp it value
		_interpPercent = (amount * 0.1f) % 1f;
		_interpPercent = Mathf.Clamp (_interpPercent, 0, 1);

		transform.position = Vector3.Lerp (pos_InterpStart, pos_InterpEnd, _interpPercent);
		transform.localScale = Vector3.Lerp (sc_InterpStart, sc_InterpEnd, _interpPercent);
		_image.color = new Color (1, 1, 1, Mathf.Lerp (col_InterpStart, col_InterpEnd, _interpPercent));
	}
}
