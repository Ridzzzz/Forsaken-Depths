using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DSMenuHandler : MonoBehaviour {

	Joystick joystick;
	public Button actionButton;
	Vector2 input;


	public GameObject retryGameobject;
	public Button retryButton;
	public GameObject exitGameobject;
	public Button exitButton;


	GameObject activeGameobject;
	Button activeButton;
	float nextInput;
	float inputDelay;


	void Start () 
	{
		joystick = FindObjectOfType<Joystick>();
		activeGameobject = null;
		activeButton = null;
		inputDelay = 0.5f;
		EventSystem.current.SetSelectedGameObject(retryGameobject);
		actionButton.onClick.AddListener(GetButtonInput);
	}
	
	void Update () 
	{
		GetJoystickInput();
		MenuNavigation();
	}

	void GetJoystickInput()
    {
		input = new Vector2(Mathf.Round(joystick.Horizontal * 100f)/100f, Mathf.Round(joystick.Vertical * 100f)/100f);
    }

	public void RemoveSelectionListener()
	{
		actionButton.onClick.RemoveListener(GetButtonInput);
	}

	void GetButtonInput()
	{
		var pEventData = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(activeButton.gameObject, pEventData, ExecuteEvents.submitHandler);
	}

	void MenuNavigation()
	{
		if(nextInput > Time.time) return;

		if (EventSystem.current.currentSelectedGameObject != null)
		{
			if (EventSystem.current.currentSelectedGameObject.tag == "MenuButton")
			{
				activeGameobject = EventSystem.current.currentSelectedGameObject;

				switch (activeGameobject.name)
				{					
					case "Retry":
						activeButton = retryButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(exitGameobject);
						}							 
						break;
					case "Exit":
						activeButton = exitButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(retryGameobject);
						}							
						break;

		

					default:
						break;
				}
			}
		}

		else
		{
			EventSystem.current.SetSelectedGameObject(activeGameobject);
		}	
	}
}
