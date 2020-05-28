using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
	public float baseValue;
	
	public float GetValue()
	{
		return baseValue;
	}

	public void IncreaseStat(float Value)
	{
		baseValue += Value;
	}

	public void DecreaseStat(float Value)
	{
		baseValue -= Value;
	}

	public void CopyStatValue(float Value)
	{
		baseValue = Value;
	}
}
