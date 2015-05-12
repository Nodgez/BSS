using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour, IFade{

	private Vector3 sc_InterpStart;
	private Vector3 sc_InterpEnd;
	private float col_InterpStart;
	private float col_InterpEnd;
	private Vector3 pos_InterpStart;
	private Vector3 pos_InterpMiddle;
	private Vector3 pos_InterpEnd;
	private float _interpPercent = 1;
	private Image _image;				//the button Image

	public bool interpComplete = true;	//Used to check if all butons have finished interpolating

	void Start () 
	{
		//gets the image component
		_image = GetComponent<Image> ();
	}

	void Update () 
	{
		//check lerp complete
		if (_interpPercent >= 1)
		{
			interpComplete = true;
			return;
		}

		//increment interpolation and clamp it value
		_interpPercent += Time.deltaTime * 5;
		_interpPercent = Mathf.Clamp (_interpPercent, 0, 1);

		//calculate the position, scale and opacity of the button using interpolation
		transform.position = cerp(_interpPercent, pos_InterpStart,pos_InterpMiddle, pos_InterpEnd);
		transform.localScale = Vector3.Lerp (sc_InterpStart, sc_InterpEnd, _interpPercent);
		_image.color = new Color (1, 1, 1, Mathf.Lerp (col_InterpStart, col_InterpEnd, _interpPercent));
	}

	public void StartInterpolation(WheelPoint wheelPoint, bool left)
	{
		//reset interpolation values
		_interpPercent = 0;
		interpComplete = false;

		//set postition interp values
		pos_InterpEnd = wheelPoint.position;
		pos_InterpStart = transform.position;
		Vector3 difference = pos_InterpEnd - pos_InterpStart;
		Vector3 perpendicular;
		if (left)
			perpendicular = new Vector3 (-difference.y, difference.x, 0) * 0.2f;	//get the perpendicular line
		else
			perpendicular = new Vector3 (difference.y, -difference.x) * 0.2f;
		pos_InterpMiddle = pos_InterpStart + (difference * 0.5f) + perpendicular;

		//set scale interp values
		sc_InterpStart = transform.localScale;
		sc_InterpEnd = wheelPoint.scale;

		//set opactity interp values
		col_InterpEnd = wheelPoint.opactity;
		col_InterpStart = _image.color.a;

		transform.SetSiblingIndex (wheelPoint.siblingIndex);
		GetComponent<Button> ().interactable = wheelPoint.interactable;
	}

	//Cubic interpolation equation
	Vector3 cerp(float t,Vector3 p0, Vector3 p1, Vector3 p2)
	{
		return (1.0f - t) * (1.0f - t) * p0 
			+ 2.0f * (1.0f - t) * t * p1
				+ t * t * p2;
	}
	
	public void Fade()
	{}
}
