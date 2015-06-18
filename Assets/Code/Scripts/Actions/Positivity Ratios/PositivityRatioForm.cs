using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PositivityRatioForm : MonoBehaviour {

	public InputField energyGiverInput;
	public InputField energyTakerInput;
	public InputField giverLiklinessInput;
	public InputField takerLiklinessInput;

	public PositivityRatioInfo Info
	{
		get
		{
			PositivityRatioInfo info = new PositivityRatioInfo();
			info.enrgyGiver = energyGiverInput.text;
			info.enrgyTaker = energyTakerInput.text;
			info.giverLiklyness = giverLiklinessInput.text;
			info.takerLiklyness = takerLiklinessInput.text;

			return info;
		}

		set 
		{
			if(value == null)
				return;
			energyGiverInput.text = value.enrgyGiver;
			energyTakerInput.text = value.enrgyTaker;
			giverLiklinessInput.text = value.giverLiklyness;
			takerLiklinessInput.text = value.takerLiklyness;
		}
	}
}

public class PositivityRatioInfo
{
	public string enrgyGiver;
	public string enrgyTaker;
	public string giverLiklyness;
	public string takerLiklyness;

	public PositivityRatioInfo()
	{
//		enrgyGiver = "";
//		enrgyTaker = "";
//		giverLiklyness = "";
//		takerLiklyness = "";
	}
}
