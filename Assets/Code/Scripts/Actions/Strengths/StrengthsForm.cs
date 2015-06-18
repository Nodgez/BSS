using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrengthsForm : MonoBehaviour {
	
	public InputField mentalityInput;
	public InputField physicalityInput;
	public InputField professionalityInput;
	public InputField emotalityInput;
	public InputField socialityInput;
	public InputField spiritualityInput;

	public StrengthsInfo Info
	{
		get
		{
			StrengthsInfo info = new StrengthsInfo();
			info.mentality = mentalityInput.text;
			info.physicality = physicalityInput.text;
			info.professionality = professionalityInput.text;
			info.emotality = emotalityInput.text;
			info.sociality = socialityInput.text;
			info.spirituality = spiritualityInput.text;

			return info;
		}
		set
		{
			if(value == null) return;

			mentalityInput.text = value.mentality;
			physicalityInput.text = value.physicality;
			professionalityInput.text = value.professionality;
			emotalityInput.text = value.emotality;
			socialityInput.text = value.sociality;
			spiritualityInput.text = value.spirituality;
		}
	}
}

public class StrengthsInfo
{
	public string mentality;
	public string physicality;
	public string professionality;
	public string emotality;
	public string sociality;
	public string spirituality;
	
	public StrengthsInfo()
	{
		//		enrgyGiver = "";
		//		enrgyTaker = "";
		//		giverLiklyness = "";
		//		takerLiklyness = "";
	}
}
