using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddEmotionForm : MonoBehaviour {

	public InputField emotionName;
	public Toggle posToggle;
	public Toggle emerToggle;

	private ELMenu elmenu;

	public void FinalizeAdd()
	{
		if (emotionName.text == "")
			return;
		elmenu = GameObject.Find ("ELMenu").GetComponent<ELMenu> ();
		EmotionType emoType = EmotionType.Positive;
		if (emerToggle.isOn)
			emoType = EmotionType.Emergency;

		Vector3 position = emoType == EmotionType.Emergency ? Vector3.right : Vector3.left;
		Emotion newEmotion = new Emotion (position, emoType, emotionName.text);

		elmenu.AddEmotionToGraph (newEmotion);
		Destroy (this.gameObject);
	}

	void Update()
	{
		//if(posToggle.
	}
}
