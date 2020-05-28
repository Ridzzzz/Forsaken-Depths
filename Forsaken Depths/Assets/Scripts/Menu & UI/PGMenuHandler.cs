using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PGMenuHandler : MonoBehaviour {


	Joystick joystick;
	public Button actionButton;
	Vector2 input;


	public GameObject playGameobject;
	public Button playButton;
	public GameObject setupGameobject;
	public Button setupButton;
	public GameObject exitGameobject;
	public Button exitButton;


	public GameObject battleGameobject;
	public Button battleButton;
	public GameObject pReturnGameobject;
	public Button pReturnButton;


	public GameObject xUpGameobject;
	public Button xUpButton;
	public GameObject yUpGameobject;
	public Button yUpButton;
	public GameObject zUpGameobject;
	public Button zUpButton;
	public GameObject xDownGameobject;
	public Button xDownButton;
	public GameObject yDownGameobject;
	public Button yDownButton;
	public GameObject zDownGameobject;
	public Button zDownButton;
	public GameObject ScanGameobject;
	public Button ScanButton;
	public GameObject ResetGameobject;
	public Button ResetButton;
	public GameObject sReturnGameobject;
	public Button sReturnButton;


	GameObject activeGameobject;
	Button activeButton;
	float nextInput;
	float inputDelay;


	public void SetplayButton()
	{
		StartCoroutine(StartplayButton());
	}

	public IEnumerator StartplayButton()
	{
		yield return new WaitForSeconds(1);
		EventSystem.current.SetSelectedGameObject(playGameobject);
		activeButton = playButton;	
		yield break;
	}

	public void SetbattleButton()
	{
		StartCoroutine(StartbattleButton());
	}

	public IEnumerator StartbattleButton()
	{
		yield return new WaitForSeconds(1);
		EventSystem.current.SetSelectedGameObject(battleGameobject);
		activeButton = battleButton;	
		yield break;
	}

	public void SetsReturnButton()
	{
		StartCoroutine(StartsReturnButton());
	}

	public IEnumerator StartsReturnButton()
	{
		yield return new WaitForSeconds(1);
		EventSystem.current.SetSelectedGameObject(sReturnGameobject);
		activeButton = sReturnButton;				
		yield break;
	}

	void Start () 
	{
		joystick = FindObjectOfType<Joystick>();
		activeGameobject = null;
		activeButton = null;
		inputDelay = 0.5f;
		EventSystem.current.SetSelectedGameObject(playGameobject);
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
					case "Play":
						activeButton = playButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(setupGameobject);
						}							 
						break;
					case "Setup":
						activeButton = setupButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(playGameobject);
						}							
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
							EventSystem.current.SetSelectedGameObject(setupGameobject);
						}							
						break;



					case "Battle":
						activeButton = battleButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(pReturnGameobject);
						}							 
						break;
					case "pReturn":
						activeButton = pReturnButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(battleGameobject);
						}							
						break;



					case "xUp":
						activeButton = xUpButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(yUpGameobject);
						}
						if (input.x >= 0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(xDownGameobject);
						}							 
						break;
					case "yUp":
						activeButton = yUpButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(xUpGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(zUpGameobject);
						}		
						if (input.x >= 0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(yDownGameobject);
						}							
						break;
					case "zUp":
						activeButton = zUpButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(yUpGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(ScanGameobject);
						}		
						if (input.x >= 0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(zDownGameobject);
						}							
						break;
					case "xDown":
						activeButton = xDownButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(yDownGameobject);
						}
						if (input.x <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(xUpGameobject);
						}							
						break;
					case "yDown":
						activeButton = yDownButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(xDownGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(zDownGameobject);
						}		
						if (input.x <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(yUpGameobject);
						}							
						break;
					case "zDown":
						activeButton = zDownButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(yDownGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(ScanGameobject);
						}		
						if (input.x <= -0.25)
						{
							nextInput = Time.time + inputDelay;
							EventSystem.current.SetSelectedGameObject(zUpGameobject);
						}								
						break;
					case "Scan":
						activeButton = ScanButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(zDownGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(ResetGameobject);
						}							
						break;
					case "Reset":
						activeButton = ResetButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(ScanGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(sReturnGameobject);
						}							
						break;
					case "sReturn":
						activeButton = sReturnButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.time + inputDelay; 
							EventSystem.current.SetSelectedGameObject(ResetGameobject);
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
