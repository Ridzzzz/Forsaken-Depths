using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    Animator animator;

    EnemyStat eStat;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        eStat = GetComponent<EnemyStat>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Alpha1))
        {
            enemyIsIdle();
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            enemyIsChasing();
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            enemyIsAttacking();
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            enemyIsDead();
        }
        */

        if (Input.GetKeyDown(KeyCode.Space))
        {
            eStat.TakeDamage(5, 1);
            Debug.Log(eStat.currentHealth);
        }
    }

    void enemyIsIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);
    }

    void enemyIsChasing()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);
    }

    void enemyIsAttacking()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", true);
        animator.SetBool("isDead", false);
    }

    void enemyIsDead()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
    }
}
