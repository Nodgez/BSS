using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EPMenu : HistoryMenu {

	public EmotionListFromGraph emotionList;
	public GraphRepresentation graphRepresentation;

	protected override void Start ()
	{
		base.Start ();
		GraphData todayData = GetTodayData ();
		emotionList.AddEmotions (todayData.emotions);
		graphRepresentation.Shift (todayData.normailzedGraphPosition);
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public override void SwapGraphInfo (System.DateTime date)
	{
		base.SwapGraphInfo (date);

		GraphData data = GetDataAtDate (date);
		if (data == null)
			return;
		emotionList.AddEmotions (data.emotions);
		graphRepresentation.Shift (data.normailzedGraphPosition);
	}

	public void OpenPropertiesPanel()
	{

	}
}

public class EmotionalProperty
{
	public string emotionName;
	public string emotionLocation;
	public string emotionIntensity;
}
