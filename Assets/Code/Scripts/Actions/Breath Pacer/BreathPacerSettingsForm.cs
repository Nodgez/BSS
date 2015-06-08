using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BreathPacerSettingsForm : MonoBehaviour {

	public Slider breathRatioSlider;
	public Slider bpmSlider;
	public Slider durationSlider;
	public Text inhalesText;
	public Text exhalesText;
	public Text bpmText;
	public Text durationText;
	public Toggle audioOnToggle;
	public Toggle audioOffToggle;
	public Toggle vibrateOnToggle;
	public Toggle vibrateOffToggle;
	
	public BreathPacerSetting settings;
	public ButterflyController butterfly;

	private SlidingPanel slidingPanel;
	private bool audioIsOn = true;

	void Start () 
	{
		slidingPanel = GetComponent<SlidingPanel> ();
		slidingPanel.onSlideTo += delegate {
			if(butterfly.animator.enabled)
				butterfly.animator.enabled = false;
		};
		slidingPanel.onSlideBack += butterfly.ResetButterFly;
		settings = new BreathPacerSetting ();
		butterfly.Settings = settings;
	}
	
	void Update () 
	{
		settings.breathsPerMinute = (int)bpmSlider.value;
		settings.exhaleRate = (int)breathRatioSlider.value;
		settings.inhaleRate = (int)(breathRatioSlider.maxValue + 1) - (int)breathRatioSlider.value;
		settings.duration = durationSlider.value;

		exhalesText.text = settings.exhaleRate.ToString();
		inhalesText.text = settings.inhaleRate.ToString();
		bpmText.text = settings.breathsPerMinute.ToString ();
		durationText.text = settings.duration.ToString ();

		butterfly.Settings = settings;
	}
	
	public void ToggleAudio()
	{
		if(audioOnToggle.isOn)
		{

		}
		//Debug.Log (audioOnToggle.isOn);
	}
}
