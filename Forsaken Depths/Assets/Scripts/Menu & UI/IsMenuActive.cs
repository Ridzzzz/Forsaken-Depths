using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMenuActive : MonoBehaviour {

	public bool isMenuActivated;
	void Start () 
	{
		isMenuActivated = false;
	}
	
	public void MenuOpen()
	{
		isMenuActivated = true;
		Time.timeScale = 0;
	}
	
	public void MenuClosed()
	{
		isMenuActivated = false;
		Time.timeScale = 1;
	}
}
