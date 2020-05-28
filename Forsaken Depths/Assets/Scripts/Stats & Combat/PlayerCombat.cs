using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStat))]
public class PlayerCombat : MonoBehaviour
{
    public IsMenuActive isMenuActive;
    public ParticleSystem playerAttackEffect;
    PlayerStat playerStat;
    LayerMask enemyLayer;
    Collider [] attackVicinity;
    float attackRadius = 1f;

    float attackDelay = 1f;
    float nextAttack;

    Coroutine attackRunning;
    
    void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        enemyLayer = LayerMask.GetMask("Enemy");
        attackRunning = null;
    }

    public void Attack()
    {
        if (attackRunning != null) return;

        if (!isMenuActive.isMenuActivated)
        {       
            playerAttackEffect.Play();
            attackRunning = StartCoroutine(StopAttackEffect());

            attackVicinity = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
            if(attackVicinity.Length > 0)
            {
                foreach (Collider enemies in attackVicinity)
                {
                    enemies.gameObject.GetComponent<EnemyStat>().TakeDamage(playerStat.Strength.GetValue(), 1);                  
                }
            }
        }        
    }

    IEnumerator StopAttackEffect()
    {
        yield return new WaitForSeconds(1f);

        playerAttackEffect.Stop();

        attackRunning = null;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
