using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSMenuManager : MonoBehaviour {

	public DeathScreenMenu Menu;

	public void Start () {
        //AudioManager.Instance.ToggleMusic(true, AudioManager.MusicType.DeathScreenBG);
        ShowMenu (Menu);
    }

	public void ShowMenu (DeathScreenMenu _Menu) 
	{
		if (Menu != null) 
		{
			Menu.DeathScreenOpen = false;
		}

		Menu = _Menu;
		Menu.DeathScreenOpen = true;
	}
}
