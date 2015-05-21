using UnityEngine;
using System.Collections;

public class RenderScripts : MonoBehaviour {

	public GameObject sphere;
	Texture2D texture;
	public bool done = false;

	// Use this for initialization
	void Start () 
	{
		texture = new Texture2D (Screen.width / 2,Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnPostRender()
	{
		if (done)
			return;
		
		texture.ReadPixels(new Rect(Screen.width * 0.25f,Screen.height * 0.25f,Screen.width * 0.5f,Screen.height * 0.5f), 0,0);
		texture.Apply();
		sphere.GetComponent<Renderer>().material.mainTexture = texture;
		sphere.transform.position = new Vector3 (0, 0, -8);
		done = true;
	}
}
