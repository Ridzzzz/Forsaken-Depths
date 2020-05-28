using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Loader : MonoBehaviour {

	public void LoadPreGame()
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}

	public void LoadInGame()
	{
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	public void LoadDeathScreen()
	{
		SceneManager.LoadScene(2, LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
