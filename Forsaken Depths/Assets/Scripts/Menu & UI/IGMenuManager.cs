using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGMenuManager : MonoBehaviour {

	public InGameMenu Menu;

	public void Start () {
        //AudioManager.Instance.ToggleMusic(true, AudioManager.MusicType.InGameBG);
        ShowMenu (Menu);
    }

	public void ShowMenu (InGameMenu _Menu) 
	{
		if (Menu != null) 
		{
			Menu.InGameOpen = false;
		}

		Menu = _Menu;
		Menu.InGameOpen = true;
	}
}
