using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IGMenuHandler : MonoBehaviour {

	public IsMenuActive isMenuActive;
	Joystick joystick;
	public Button actionButton;
	Vector2 input;


	public GameObject hpUpGameobject;
	public Button hpUpButton;
	public GameObject speedUpGameobject;
	public Button speedUpButton;
	public GameObject strengthUpGameobject;
	public Button strengthUpButton;
	public GameObject resistanceUpGameobject;
	public Button resistanceUpButton;
	public GameObject bReturnGameobject;
	public Button bReturnButton;
	public GameObject setupGameobject;
	public Button setupButton;
	public GameObject exitGameobject;
	public Button exitButton;

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


	bool menuRunning;

	public void SetbReturnButton()
	{
		StartCoroutine(StartbReturnButton());
	}

	public IEnumerator StartbReturnButton()
	{
		yield return new WaitForSecondsRealtime(1);
		EventSystem.current.SetSelectedGameObject(bReturnGameobject);
		activeButton = bReturnButton;	
		yield break;
	}

	public void SetsReturnButton()
	{
		StartCoroutine(StartsReturnButton());
	}

	public IEnumerator StartsReturnButton()
	{
		yield return new WaitForSecondsRealtime(1);
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
	}
	
	void Update () 
	{
		if (isMenuActive.isMenuActivated && menuRunning)
		{
			GetJoystickInput();
			MenuNavigation();
		}
	}

	void GetJoystickInput()
    {
		input = new Vector2(Mathf.Round(joystick.Horizontal * 100f)/100f, Mathf.Round(joystick.Vertical * 100f)/100f);
    }

	public void AddSelectionListener()
	{
		menuRunning = true;
		actionButton.onClick.AddListener(GetButtonInput);
	}

	public void RemoveSelectionListener()
	{
		actionButton.onClick.RemoveListener(GetButtonInput);
		menuRunning = false;
	}

	void GetButtonInput()
	{
		var pEventData = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(activeButton.gameObject, pEventData, ExecuteEvents.submitHandler);
	}

	void MenuNavigation()
	{
		if(nextInput > Time.unscaledTime) return;

		if (EventSystem.current.currentSelectedGameObject != null)
		{
			if (EventSystem.current.currentSelectedGameObject.tag == "MenuButton")
			{
				activeGameobject = EventSystem.current.currentSelectedGameObject;

				switch (activeGameobject.name)
				{
					case "HpUp":
						activeButton = hpUpButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(speedUpGameobject);
						}							 
						break;
					case "SpdUp":
						activeButton = speedUpButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(hpUpGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(strengthUpGameobject);
						}							
						break;
					case "StrUp":
						activeButton = strengthUpButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(speedUpGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(resistanceUpGameobject);
						}							
						break;
					case "ResUp":
						activeButton = resistanceUpButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(strengthUpGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(bReturnGameobject);
						}							
						break;
					case "bReturn":
						activeButton = bReturnButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(resistanceUpGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(setupGameobject);
						}							
						break;
					case "Setup":
						activeButton = setupButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(bReturnGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(exitGameobject);
						}							
						break;
					case "Exit":
						activeButton = exitButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(setupGameobject);
						}							
						break;



					case "xUp":
						activeButton = xUpButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(yUpGameobject);
						}
						if (input.x >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(xDownGameobject);
						}							 
						break;
					case "yUp":
						activeButton = yUpButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(xUpGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(zUpGameobject);
						}		
						if (input.x >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(yDownGameobject);
						}							
						break;
					case "zUp":
						activeButton = zUpButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(yUpGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(ScanGameobject);
						}		
						if (input.x >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(zDownGameobject);
						}							
						break;
					case "xDown":
						activeButton = xDownButton;
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(yDownGameobject);
						}
						if (input.x <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(xUpGameobject);
						}							
						break;
					case "yDown":
						activeButton = yDownButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(xDownGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(zDownGameobject);
						}		
						if (input.x <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(yUpGameobject);
						}							
						break;
					case "zDown":
						activeButton = zDownButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(yDownGameobject);
						}
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(ScanGameobject);
						}		
						if (input.x <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay;
							EventSystem.current.SetSelectedGameObject(zUpGameobject);
						}								
						break;
					case "Scan":
						activeButton = ScanButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(zDownGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(ResetGameobject);
						}							
						break;
					case "Reset":
						activeButton = ResetButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(ScanGameobject);
						}							
						if (input.y <= -0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
							EventSystem.current.SetSelectedGameObject(sReturnGameobject);
						}							
						break;
					case "sReturn":
						activeButton = sReturnButton;
						if (input.y >= 0.25)
						{
							nextInput = Time.unscaledTime + inputDelay; 
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
