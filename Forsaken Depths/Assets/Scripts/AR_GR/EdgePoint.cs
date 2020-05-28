using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePoint : MonoBehaviour
{
    public GameObject[] edgePointFire;
    State currentState;

    public event System.Action<State> onEdgePointClicked;

    void Start() 
    {
        currentState = State.NONE;
    }

    public void SetState(State stoneState)
    {
        switch (stoneState)
        {
            case State.Red:
                currentState = State.Red;
                if (!edgePointFire[0].activeInHierarchy)
                {
                    edgePointFire[0].SetActive(true);
                }

                if (edgePointFire[1].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[1]));
                }
                if (edgePointFire[2].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[2]));
                }            
                break;
            case State.Green:
                currentState = State.Green;
                if (!edgePointFire[1].activeInHierarchy)
                {
                    edgePointFire[1].SetActive(true);
                }
                
                 if (edgePointFire[0].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[0]));
                }
                if (edgePointFire[2].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[2]));
                } 
                break;
            case State.Blue:
                currentState = State.Blue;
                if (!edgePointFire[2].activeInHierarchy)
                {
                    edgePointFire[2].SetActive(true);
                } 

                if (edgePointFire[0].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[0]));
                }
                if (edgePointFire[1].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[1]));
                } 
                break;
            case State.NONE:
                currentState = State.NONE;

                if (edgePointFire[0].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[0]));
                }
                if (edgePointFire[1].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[1]));
                }
                if (edgePointFire[2].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[2]));
                }
                break;
        }
    }

    public State GetState()
    {
        return currentState;
    }
    
    void StateSequence(State checkCurrentState)
    {
        switch(checkCurrentState)
        {
            case State.Red:        
                break;
            case State.Green:              
                break;
            case State.Blue:
                currentState = State.Green;
                if (!edgePointFire[1].activeInHierarchy)
                {
                    edgePointFire[1].SetActive(true);
                } 

                if (edgePointFire[0].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[0]));
                }
                if (edgePointFire[2].activeInHierarchy)
                {
                    StartCoroutine(TurnOffFireParticles(edgePointFire[2]));
                } 
                break;
            case State.NONE:                
                break;
        }        
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "PlayerTest")
        {
            onEdgePointClicked(currentState); 
            StateSequence(currentState);                     
        }        
    }

    IEnumerator TurnOffFireParticles(GameObject fireToTurnOff)
    {
        fireToTurnOff.GetComponent<ParticleSystem>().Stop();
        fireToTurnOff.GetComponentInChildren<ParticleSystem>().Stop();

        yield return new WaitForSeconds(2f);

        fireToTurnOff.SetActive(false);

        yield return null;
    }
}
