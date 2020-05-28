using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class VoidStorm : MonoBehaviour
{
    PlayerStat playerStat;
    public GameObject playerReference, vsBaseEffects;
    public ParticleSystem[] vsBaseEffect;
    public ParticleSystem vsLightningEffect, vsLightningSparks;

    public TMP_Text voidStormTimer;

    float vsTimer;

    Coroutine vsCoroutine;

    bool voidStormActive, voidStormAttack, voidStormPuzzleStarted;

    public GameObject enableEdgePoints;
    public GameObject[] edgePointTlFire;
    public GameObject[] edgePointTrFire;
    public GameObject[] edgePointBlFire;
    public GameObject[] edgePointBrFire;
    public GameObject[] edgePoints;

    void Start()
    {
        voidStormActive = false;
        vsCoroutine = null;
        vsBaseEffects.SetActive(true);

        for (int i = 0; i < 6; i++)
        {
            vsBaseEffect[i].Stop();
        }

        vsLightningEffect.Stop();
        vsLightningSparks.Stop();

        enableEdgePoints.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            edgePointTlFire[i].SetActive(false);
            edgePointTrFire[i].SetActive(false);
            edgePointBlFire[i].SetActive(false);
            edgePointBrFire[i].SetActive(false);
        }

        vsTimer = 150f;
        voidStormTimer.color = Color.white;

        playerStat = playerReference.GetComponent<PlayerStat>();

        for (int i = 0; i < 4; i++)
        {
            edgePoints[i].GetComponent<EdgePoint>().onEdgePointClicked += VoidStormPuzzle;
        }

        voidStormPuzzleStarted = false;         
    }

    void FixedUpdate()
    {
        vsTimer -= Time.deltaTime;

        if (vsCoroutine == null)
        {
            vsCoroutine = StartCoroutine(DamageTimer());
        }        
    }

    IEnumerator DamageTimer()
    {
        int minutes = Mathf.FloorToInt(vsTimer / 60F);
        int seconds = Mathf.FloorToInt(vsTimer - minutes * 60);
        voidStormTimer.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        if ((vsTimer <= 91) && (vsTimer > 30))
        {
            voidStormTimer.color = Color.yellow;
        }

        if ((vsTimer <= 31) && (vsTimer > 1))
        {
            voidStormTimer.color = Color.red;

            if (!voidStormActive)
            {
                voidStormActive = true;
                VoidStormPuzzle(State.NONE);
                for (int i = 0; i < 6; i++)
                {
                    vsBaseEffect[i].Play();
                }
            }
        }

        if (vsTimer <= 1)
        {
            float vsDamage;
            vsDamage = playerStat.Health.GetValue() * 0.25f;

            if (!voidStormAttack)
            {                
                voidStormAttack = true;

                foreach (GameObject EdgePoint in edgePoints)
                {
                    EdgePoint.GetComponent<EdgePoint>().SetState(State.NONE);
                }

                vsLightningEffect.Play();            
                playerStat.TakeDamage(vsDamage, 0);
            }

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < 6; i++)
            {
                vsBaseEffect[i].Stop();
            }

            vsLightningEffect.Stop();

            voidStormActive = false;

            vsTimer = 150f;
            voidStormTimer.color = Color.white;

            voidStormAttack = false;
        }

        vsCoroutine = null;
    }

    void VoidStormPuzzle(State stoneState)
    {
        List<int> edgePointsToShuffle = new List<int>();
        int edgePointSelected;

        switch (stoneState)
        {
            case State.NONE:
                edgePointSelected = Random.Range(0, edgePoints.Length);
                edgePoints[edgePointSelected].GetComponent<EdgePoint>().SetState(State.Blue);
                for (int i = 0; i < edgePoints.Length; i++)
                {
                    edgePoints[i].GetComponent<Collider>().enabled = true;
                    if (i != edgePointSelected)
                    {
                        edgePoints[i].GetComponent<EdgePoint>().SetState(State.Red);
                    }
                }
                break;
            case State.Red:
                foreach (GameObject EdgePoint in edgePoints)
                {
                    EdgePoint.GetComponent<EdgePoint>().SetState(State.NONE);
                    EdgePoint.GetComponent<Collider>().enabled = false;
                }
                break;
            case State.Green:
                break;
            case State.Blue:
                for (int i = 0; i < edgePoints.Length; i++)
                {
                    if (edgePoints[i].GetComponent<EdgePoint>().GetState() == State.Red)
                    {
                        edgePointsToShuffle.Add(i);                    
                    }
                }

                if (edgePointsToShuffle.Count > 1)
                {
                    edgePointSelected = Random.Range(0, edgePointsToShuffle.Count);
                    edgePoints[edgePointsToShuffle[edgePointSelected]].GetComponent<EdgePoint>().SetState(State.Blue);                    
                }                

                else
                {
                    foreach (GameObject EdgePoint in edgePoints)
                    {
                        EdgePoint.GetComponent<EdgePoint>().SetState(State.NONE);
                        EdgePoint.GetComponent<Collider>().enabled = false;
                    }

                    playerStat.GainExp(75f);
                    voidStormAttack = true;
                }
                break;
        }    
    }
}
