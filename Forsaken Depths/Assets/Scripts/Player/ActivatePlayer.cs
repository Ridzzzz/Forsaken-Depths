using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatePlayer : MonoBehaviour
{
    public GameObject activateObjectManager;
    public GameObject activatePathFinding;
    public GameObject worldGenerator;
    public GameObject activatePlayerScripts;
    public GameObject activateFloating;
    public GameObject activateUiBar;
    public GameObject spawningRaycastBlocker;
    
    public Button optionButton;
    public Button actionButton;

    void Start()
    {
        StartCoroutine(EnableActivation());   
    }

    IEnumerator EnableActivation()
    {
        yield return new WaitForSeconds(0.5f);

        activateObjectManager.SetActive(true);
        worldGenerator.GetComponent<WorldGenerator>().playerHasSpawned = true;
        activateFloating.GetComponent<FloatEffects>().enabled = true;
        activatePlayerScripts.GetComponent<VoidStorm>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        activateUiBar.SetActive(true);
        activatePlayerScripts.GetComponent<PlayerStat>().enabled = true;
        activatePlayerScripts.GetComponent<PlayerUiBar>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        activatePathFinding.SetActive(true);
        activatePlayerScripts.GetComponent<PlayerController>().enabled = true;
        optionButton.interactable = true;
        actionButton.interactable = true;
        spawningRaycastBlocker.SetActive(false);
    }
}
