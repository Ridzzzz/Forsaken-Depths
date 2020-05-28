using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameMenu : MonoBehaviour {

	private Animator AnimatorControl;
	private CanvasGroup CanvasGroupControl;

	public bool PreGameOpen
	{
		get {return AnimatorControl.GetBool("PreGameOpen");}
		set {AnimatorControl.SetBool("PreGameOpen",value);}
	}

	public void Awake ()  
	{
		AnimatorControl = GetComponent<Animator>();
		CanvasGroupControl = GetComponent<CanvasGroup>();

		var rect = GetComponent<RectTransform>();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);
	}
	

	public void Update () {
		if (!AnimatorControl.GetCurrentAnimatorStateInfo (0).IsName ("PGMenuOpen")) {
			CanvasGroupControl.blocksRaycasts = CanvasGroupControl.interactable = false;
		} else {
			CanvasGroupControl.blocksRaycasts = CanvasGroupControl.interactable = true;
		}
	}
}
