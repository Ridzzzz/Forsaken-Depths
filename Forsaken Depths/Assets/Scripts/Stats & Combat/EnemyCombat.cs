using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStat))]
public class EnemyCombat : MonoBehaviour
{
    EnemyStat enemyStat;
    PlayerStat playerStat;
    
    void OnEnable()
    {
        enemyStat = GetComponent<EnemyStat>();
        playerStat = GetComponent<EnemyController>().target.GetComponent<PlayerStat>();
    }

    public void Attack()
    {
        playerStat.TakeDamage(enemyStat.Strength.GetValue(), 0);
    }
}
