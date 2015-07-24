using UnityEngine;
using System.Collections;

public enum SortMethod
{
	Reverse,
	Alpahbet
}

public class ChildrenSorter : MonoBehaviour {

	public SortMethod sortingMethod = SortMethod.Reverse;

	void Start () 
	{
		switch (sortingMethod) {
		
		case SortMethod.Reverse:
			for(int i = 0; i < transform.childCount;i ++)
			{
				if(i >= transform.childCount / 2)
					break;

				Transform left = transform.GetChild(i);
				Transform right = transform.GetChild((transform.childCount -1) - i);

				left.SetSiblingIndex(transform.childCount - i);
				right.SetSiblingIndex(i);
			}
			break;
		case SortMethod.Alpahbet:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
