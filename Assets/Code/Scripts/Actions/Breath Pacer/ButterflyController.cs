using UnityEngine;
using System.Collections;

public enum BreathPacerState {
	Inhaling,
	Exhaling,
	Paused
}

public class ButterflyController : MonoBehaviour {
	
	public Animator animator;
	private float timer = 0;
	private float durationTimer = 0;
	
	public BreathPacerState state = BreathPacerState.Inhaling;
	public float pbTime = 1;
	public float timeBetweenBreaths = 1.0f;
	public GameObject audioEffect;

	public BreathPacerSetting Settings { get; set; }
	
	void Start () 
	{
		Settings = new BreathPacerSetting ();
		animator = GetComponent<Animator> ();
		animator.enabled = false;
		ResetButterFly ();
	}

	void Update () 
	{
		if (!animator.enabled)
		{
			//if we're paused then change for pause durations and change the currentState
			if(state == BreathPacerState.Paused)
			{
				timer += Time.deltaTime;
				if(timer >= timeBetweenBreaths)
				{
					timer = 0;
					if(Mathf.Sign(pbTime) > 0)
						ChangeState(BreathPacerState.Exhaling);
					else
						ChangeState(BreathPacerState.Inhaling);
					animator.enabled = true;
				}
			}
			return;
		}

		//update the time and calcualte the speeds
		timer += Time.deltaTime;
		durationTimer += Time.deltaTime;
		float[] speeds = Settings.GetBreathSpeeds ();
		float inhaleTime = speeds[0];
		float exhaleTime = speeds[1];
		switch(state)
		{
		case BreathPacerState.Inhaling:
			if(timer >= inhaleTime)
			{
				ChangeState(BreathPacerState.Paused);
				Instantiate (audioEffect);
				animator.enabled = false;
			}
			break;
		case BreathPacerState.Exhaling:
			if(timer >= exhaleTime)
			{
				ChangeState(BreathPacerState.Paused);
				Instantiate (audioEffect);
				animator.enabled = false;
			}
			break;
		}
		animator.speed = pbTime;

		if (durationTimer > Settings.duration * 60) {
			animator.enabled = false;
			Debug.Log("Done");
		}
	}
	//puts butterlfy back to starting point
	public void ResetButterFly ()
	{
		durationTimer = 0;
		animator.Play ("ButterFly", -1, 0f);
		ChangeState (BreathPacerState.Inhaling);
	}

	public void ToggleAnimation()
	{
		animator.enabled = !animator.enabled;
	}

	void ChangeState(BreathPacerState newState)
	{
		timer = 0;
		state = newState;

		if (newState == BreathPacerState.Paused)
			return;

		float[] speeds = Settings.GetBreathSpeeds ();
		if (newState == BreathPacerState.Exhaling)
			pbTime = (1.0f / speeds [1]) * -1;
		else if (newState == BreathPacerState.Inhaling)
			pbTime = 1.0f / speeds [0];

		Handheld.Vibrate ();
	}
}

public class BreathPacerSetting
{
	public int inhaleRate;
	public int exhaleRate;
	public int breathsPerMinute;
	public float duration;

	private int lastFrameInhale;
	private int lastFrameExhale;
	private int lastFrameBPM;

	public delegate void OnSettingsChanged ();
	public event OnSettingsChanged onSettingsChanged;

	public BreathPacerSetting()
	{
		inhaleRate = 5;
		exhaleRate = 5;
		breathsPerMinute = 10;
		duration = 1;
		lastFrameBPM = breathsPerMinute;
		lastFrameExhale = exhaleRate;
		lastFrameInhale = inhaleRate;
	}

	public float[] GetBreathSpeeds()
	{
		float breathTimespan = 60 / breathsPerMinute;
		int cumlitiveRatio = inhaleRate + exhaleRate;
		float breathFactor = breathTimespan / (float)cumlitiveRatio;

		return new float[2]
		{
			inhaleRate * breathFactor,
			exhaleRate * breathFactor
		};
	}

	public void Update()
	{
		if (lastFrameBPM != breathsPerMinute ||
			lastFrameInhale != inhaleRate ||
			lastFrameExhale != exhaleRate) 
		{
			if(onSettingsChanged != null)
				onSettingsChanged();

			lastFrameBPM = breathsPerMinute;
			lastFrameExhale = exhaleRate;
			lastFrameInhale = inhaleRate;
		}
	}
}

