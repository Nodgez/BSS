using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour{

	private Vector3 sc_InterpStart;
	private Vector3 sc_InterpEnd;

	private float col_InterpStart;
	private float col_InterpEnd;
	private Image _image;

	private Vector3 pos_InterpStart;
	private Vector3 pos_InterpEnd;
	private float _interpPercent = 1;

	void Start () 
	{
		_image = GetComponent<Image> ();
	}

	void Update () 
	{
		if (_interpPercent >= 1)
			return;

		_interpPercent += Time.deltaTime * 5;
		_interpPercent = Mathf.Clamp (_interpPercent, 0, 1);
		transform.position = Vector3.Lerp (pos_InterpStart, pos_InterpEnd, _interpPercent);
		transform.localScale = Vector3.Lerp (sc_InterpStart, sc_InterpEnd, _interpPercent);
		_image.color = new Color (1, 1, 1, Mathf.Lerp (col_InterpStart, col_InterpEnd, _interpPercent));
	}

	public void StartInterpolation(WheelPoint wheelPoint)
	{
		if (_interpPercent < 1)
			return;

		_interpPercent = 0;
		pos_InterpEnd = wheelPoint.position;
		pos_InterpStart = transform.position;

		sc_InterpStart = transform.localScale;
		sc_InterpEnd = wheelPoint.scale;

		col_InterpEnd = wheelPoint.opactity;
		col_InterpStart = _image.color.a;

		transform.SetSiblingIndex (wheelPoint.siblingIndex);

		if (pos_InterpEnd == pos_InterpStart)
			Debug.Log ("Error Points the Same");
	}
}
