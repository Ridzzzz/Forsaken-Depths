using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGMenuManager : MonoBehaviour {

	public PreGameMenu Menu;

	public void Start () {
        //AudioManager.Instance.ToggleMusic(true, AudioManager.MusicType.PreGameBG);
        ShowMenu (Menu);
    }

	public void ShowMenu (PreGameMenu _Menu) 
	{
		if (Menu != null) 
		{
			Menu.PreGameOpen = false;
		}

		Menu = _Menu;
		Menu.PreGameOpen = true;
	}
}
