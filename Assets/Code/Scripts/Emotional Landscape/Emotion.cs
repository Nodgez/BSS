using UnityEngine;
using System.Collections;
[System.Serializable]
public struct Emotion 
{
	public Vector3 position;
	public Vector2 directionalValue;
	public string name;

	public Emotion(Vector2 position, Vector2 directionalValue, string name)
	{
		this.position = position;
		this.directionalValue = directionalValue;
		this.name = name;
	}
}
