using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour {

    protected float Health;
    protected float Speed;
    protected float AttackDamage;
    protected float Resistance;

    protected float attackRange;

    protected float upgradePointCounter;
    protected float masteryLevelCounter;
    protected float expCounter;

    protected float upgradePoint;
    protected float masteryLevelPoint;
    protected float expPoint;



    void Start() { }

    protected virtual void Move() { }

    protected virtual void Attack() { }

    protected virtual void Die() { }
}
