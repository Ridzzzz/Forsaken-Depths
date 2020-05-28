using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    PlayerStat pStat;

    bool shardSpawned;

    void Start()
    {
        shardSpawned = false;
        
        pStat = GetComponent<PlayerStat>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            pStat.TakeDamage(10, 0);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            pStat.GainHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            pStat.GainExp(15);            
        }
    }
}
