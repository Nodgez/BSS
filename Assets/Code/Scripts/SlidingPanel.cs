using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlidingPanel : MonoBehaviour {
	
	public Scrollbar controllingSlider;
	public bool sliding = false;
	public float sliderDirection = -1.0f;

	protected virtual void Update () 
	{
		if(sliding)
		{
			//Increment the Slider until it has maxxed out on either side
			controllingSlider.value += Time.deltaTime * sliderDirection;
			float value = controllingSlider.value;
			if(value >= 1 || value <= 0)
			{
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
