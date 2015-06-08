using UnityEngine;
using System.Collections;

public class TimedObject : MonoBehaviour {

	public float aliveTime = 1.0f;
	
	void Start () {
		Destroy (gameObject, aliveTime);
	}
}
