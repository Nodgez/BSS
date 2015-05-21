using UnityEngine;
using System.Collections;

public class BeatController : MonoBehaviour {

	Animator anim;

	public float pbTime = 1;
	void Start () 
	{
		anim = GetComponent<Animator> ();
	}

	void Update () 
	{
		anim.speed = pbTime;
	}
}
