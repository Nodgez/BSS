using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewController : MonoBehaviour {

	public Scrollbar controllingSlider;
	private bool sliding = false;
	private float sliderDirection = -1.0f;

	void Update () 
	{
		if(sliding)
		{
			//Increment the Slider until it has maxxed out on either side
			controllingSlider.value += Time.deltaTime * sliderDirection;
			float value = controllingSlider.value;
			if(value >= 1 || value <= 0)
			{
				value = Mathf.Round(value);
				sliding = !sliding;
				sliderDirection *= -1;
			}
		}
	}

	//Method to be attached to the button
	public void SlideView()
	{
		float value = controllingSlider.value;
		if (value <= 0 || value >= 1)
		{
			sliding = !sliding;
			sliderDirection *= -1;
		}
	}
}
