using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : StatSystem {

    protected float enemyBaseHealth = 10f;
    protected float enemyBaseSpeed = 10f;
    protected float enemyBaseAttackDamage = 5f;
    protected float enemyBaseResistance = 5f;

    protected Rigidbody enemyRigidBody;
    protected Transform enemyTransform;

    public Transform target;

    public float turnDst = 5;
    Path path;

    void Start () {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        enemyRigidBody = GetComponent<Rigidbody>();
        enemyTransform = transform;
        Speed = enemyBaseSpeed;
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {


        while (true)
        {
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
