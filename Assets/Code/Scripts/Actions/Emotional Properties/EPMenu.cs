using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class EPMenu : HistoryMenu {

	public EmotionListFromGraph emotionList;
	public GraphRepresentation graphRepresentation;
	public PropertiesForm propertiesForm;

	private SaveData dataOnDisplay;
	protected override void Start ()
	{
		base.Start ();
//		GraphData todayData = GetTodayData ();
//		emotionList.AddEmotions (todayData.emotions);
//		graphRepresentation.Shift (todayData.normailzedGraphPosition);
		dataOnDisplay = saveDataManager.GetTodayData ();

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
		if (emotionList)
			emotionList.AddEmotions (data.emotions);
		graphRepresentation.Shift (data.normailzedGraphPosition);
		dataOnDisplay = data;

		//propertiesForm.SwapInfo (data.emotions [0]);
	}

	public void OpenPropertiesForm(string name)
	{
		Emotion currentEmotionForEdit = null;
		foreach (Emotion emo in dataOnDisplay.emotions)
			if (emo.emotionName == name)
				currentEmotionForEdit = emo;

		if (currentEmotionForEdit != null) 
			propertiesForm.SwapInfo (currentEmotionForEdit);
	}

	public void SaveProperties()
	{
		saveDataManager.Save (graphCollection);
	}
}
