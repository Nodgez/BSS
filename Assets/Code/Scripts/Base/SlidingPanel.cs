using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlidingPanel : MonoBehaviour {

	public delegate void OnSlideComplete ();
	public event OnSlideComplete onSlideComplete;

	public delegate void OnSlideTo();
	public event OnSlideTo onSlideTo;

	public delegate void OnSlideBack();
	public event OnSlideTo onSlideBack;

	public Scrollbar controllingSlider;
	public bool sliding = false;
	public float sliderDirection = -1.0f;
	public float factor = 1.0f;

	protected virtual void Update () 
	{
		if(sliding)
		{
			//Increment the Slider until it has maxxed out on either side
			controllingSlider.value += (Time.deltaTime * factor) * sliderDirection;
			float value = controllingSlider.value;
			if(value >= 1 || value <= 0)
			{
				if(onSlideComplete != null)
					onSlideComplete();
				controllingSlider.value = Mathf.Round(value);
				sliding = false;
			}
		}
	}

	//Method to be attached to the button
	public virtual void SlideView()
	{
		if (sliding)
			return;
		float value = controllingSlider.value;
		value = Mathf.Round (value);
		if (value <= 0 || value >= 1)
		{
			sliding = !sliding;
			sliderDirection *= -1;
		}
	}

	public bool IsSlid
	{
		get
		{
			return controllingSlider.value == 1;
		}
	}
}
