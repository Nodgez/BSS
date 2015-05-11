using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewController : MonoBehaviour {

	public Scrollbar controllingSlider;
	private RectTransform rectTransform;
	public bool sliding = false;
	private float sliderDirection = -1.0f;

	void Start()
	{
		rectTransform = transform as RectTransform;
		rectTransform.offsetMax = new Vector2 (Screen.width, 0);
	}

	void Update () 
	{
		if(sliding)
		{
			//Increment the Slider until it has maxxed out on either side
			controllingSlider.value += Time.deltaTime * sliderDirection;
			float value = controllingSlider.value;
			if(value >= 1 || value <= 0)
			{
				controllingSlider.value = Mathf.Round(value);
				sliding = !sliding;
			}
		}
	}

	//Method to be attached to the button
	public void SlideView()
	{
		float value = controllingSlider.value;
		value = Mathf.Round (value);
		if (value <= 0 || value >= 1)
		{
			sliding = !sliding;
			sliderDirection *= -1;
		}
	}
}
