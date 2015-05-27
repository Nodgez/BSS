using UnityEngine;
using System;

public sealed class MathExt {
	
	public static void RoundVector(ref Vector3 vec)
	{
		float x = (float)Math.Round ((double)vec.x, 2);
		float y = (float)Math.Round ((double)vec.y, 2);
		float z = (float)Math.Round ((double)vec.z, 2);
		
		vec = new Vector3 (x, y, z);
	}

	public static Vector3 Qerp(float t,Vector3 p0, Vector3 p1, Vector3 p2)
	{
		return (1.0f - t) * (1.0f - t) * p0 
			+ 2.0f * (1.0f - t) * t * p1
				+ t * t * p2;
	}

	public static Vector2 GetParallel2D(Vector2 vector)
	{
		return new Vector2 (vector.y, -vector.x);
	}

	public static string ReplaceChar(string str, char ch, char newChar)
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ();

		for(int i = 0; i < str.Length;i++)
		{
			if(str[i] == ch)
				stringBuilder.Append(newChar);
			else
				stringBuilder.Append(str[i]);
		}

		return stringBuilder.ToString();
	}
}
