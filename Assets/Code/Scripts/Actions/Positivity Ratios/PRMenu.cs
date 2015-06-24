using UnityEngine;
using System.Collections;
using System;

public class PRMenu : HistoryMenu {

	public PositivityRatioForm prForm;

	protected override void Start ()
	{
		base.Start ();

		GameObject masterMenuSlider = GameObject.Find ("MasterMenuSlider");
		if(masterMenuSlider != null)
		{
			//Get the slding event and add toggle  scrips to trigger on it's completion
			SlidingPanel silder = masterMenuSlider.GetComponent<SlidingPanel> ();
			silder.onSlideTo += ToggleDisabledObjects;
			silder.onSlideTo += delegate {
				//If data exists for today then load it
				SwapGraphInfo (DateTime.Today);
			};
		}
	}
	
	protected override void Update ()
	{
		base.Update ();
	}

	public override void SwapGraphInfo (DateTime date)
	{
		base.SwapGraphInfo (date);

		SaveData data = saveDataManager.GetDataAtDate (date);
		prForm.Info = data.positivityRatio;
	}

	public void OnInfoEdit()
	{
		SaveData data = saveDataManager.GetTodayData ();
		data.positivityRatio = prForm.Info;
		saveDataManager.Save (data);
	}
}
