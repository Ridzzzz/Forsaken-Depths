using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour {

	private Animator AnimatorControl;
	private CanvasGroup CanvasGroupControl;

	public bool InGameOpen
	{
		get {return AnimatorControl.GetBool("InGameOpen");}
		set {AnimatorControl.SetBool("InGameOpen",value);}
	}

	public void Awake ()  
	{
		AnimatorControl = GetComponent<Animator>();
		CanvasGroupControl = GetComponent<CanvasGroup>();

		var rect = GetComponent<RectTransform>();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);
	}
	

	public void Update () {
		if (!AnimatorControl.GetCurrentAnimatorStateInfo (0).IsName ("IGMenuOpen")) {
			CanvasGroupControl.blocksRaycasts = CanvasGroupControl.interactable = false;
		} else {
			CanvasGroupControl.blocksRaycasts = CanvasGroupControl.interactable = true;
		}
	}
}
