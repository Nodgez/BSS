using UnityEngine;
using System.Collections;

public class ELMenu : MonoBehaviour {

	delegate void MenuEvent(); 

	public Graph visibleGraph;
	public GraphHistory graphHistory;
	public Emotion[] allEmotions;
	private MenuEvent onSave;

	void Start()
	{
		onSave += delegate() {
			graphHistory.AddGraph (new GraphData (visibleGraph.GetEmotions));
		};
	}

	public void AddEmotionToGraph(int i)
	{
		visibleGraph.AddEmotion (allEmotions [i]);
	}
}
