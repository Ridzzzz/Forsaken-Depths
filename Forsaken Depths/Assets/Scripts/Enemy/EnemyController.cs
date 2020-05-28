using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour, IPoolable
{
    public Transform target;
    
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;
    float enemySpeed;
    float turnSpeed = 5;
    public float turnDst = 5;
    Path path;

    Collider[] targetReached, chaseTarget;
    float enemyRadius;
    Quaternion freezeEnemyXZRotation;
    Vector3 updateEnemyYPos;

    Ray enemyTerrainRay;
    RaycastHit enemyTerrainRayInfo;

    bool enemyIsIdle, enemyIsChasing, enemyIsAttacking, shardSpawned;
    public bool enemyIsDead, chasePlayer;
    GameObject HpShard, ExpShard;
    Animator enemyAnimatorController;

    Coroutine IdleStateRunning, ChaseStateRunning, AttackStateRunning, DeathStateRunning;

    LayerMask playerLayer;

    public Action<GameObject> OnReturnToPool{get; set;}

    float enemyChaseRadius = 7f;

    float enemyAttackRadius = 1.5f;

    EnemyStat enemyStat;

    void OnEnable ()
    {
        enemyAnimatorController = GetComponent<Animator>();
        enemyStat = GetComponent<EnemyStat>();
        target = GameObject.FindWithTag("Player").transform;
        
        // The size of the enemy overlapsphere
        enemyRadius = 1.5f;
        enemySpeed = enemyStat.Speed.GetValue();

        enemyIsIdle = false;
        enemyIsChasing = false;
        enemyIsAttacking = false;
        enemyIsDead = false;
        shardSpawned = false;

        chasePlayer = false;

        IdleStateRunning = StartCoroutine(IdleState());
        ChaseStateRunning = null;
        AttackStateRunning = null;
        DeathStateRunning = null;

        playerLayer = LayerMask.GetMask("Player");

        if (enemySpeed <= 5)
        {
            enemyAnimatorController.SetFloat("ChaseIndex", 0);
        }

        else if ((enemySpeed > 5) && (enemySpeed < 11))
        {
            enemyAnimatorController.SetFloat("ChaseIndex", 1);
        }

        else
        {
            enemyAnimatorController.SetFloat("ChaseIndex", 2);
        }
    }
    
    void FixedUpdate()
    {
        EnemyMovement();  

        if (!enemyIsDead)
        {
            if (((transform.position-target.position).sqrMagnitude <= enemyChaseRadius * enemyChaseRadius) && ((transform.position-target.position).sqrMagnitude > enemyAttackRadius * enemyAttackRadius))
            {  
                if (!chasePlayer)
                {
                    chasePlayer = true;
                }

                if (IdleStateRunning != null)
                {
                    StopCoroutine(IdleStateRunning);
                    IdleStateRunning = null;
                }
                
                if (AttackStateRunning != null)
                {
                    StopCoroutine(AttackStateRunning);
                    AttackStateRunning = null;
                }                    

                if (ChaseStateRunning == null)
                {
                    ChaseStateRunning = StartCoroutine(UpdatePath());
                }                                                   
            }

            else if ((transform.position-target.position).sqrMagnitude <= enemyAttackRadius * enemyAttackRadius)
            {
                if (chasePlayer)
                {
                    chasePlayer = false;
                }

                if (ChaseStateRunning != null)
                {
                    StopCoroutine(ChaseStateRunning);
                    ChaseStateRunning = null;
                }                            

                if (AttackStateRunning == null)
                {
                    AttackStateRunning = StartCoroutine(AttackState());
                }                            
            }
            
            else
            {
                if (chasePlayer)
                {
                    chasePlayer = false;
                }

                if (ChaseStateRunning != null)
                {
                    StopCoroutine(ChaseStateRunning);
                    ChaseStateRunning = null;
                } 

                if (IdleStateRunning == null)
                {
                    IdleStateRunning = StartCoroutine(IdleState());
                }            
            }
        }

        else
        {
            if (IdleStateRunning != null)
            {
                StopCoroutine(IdleStateRunning);
                IdleStateRunning = null;
            }

            if (ChaseStateRunning != null)
            {
                StopCoroutine(ChaseStateRunning);
                ChaseStateRunning = null;
            }

            if (AttackStateRunning != null)
            {
                StopCoroutine(AttackStateRunning);
                AttackStateRunning = null;
            }

            if (DeathStateRunning == null)
            {
                DeathStateRunning = StartCoroutine(DeathState());
            }
        }           
    }

    public void PlayerIsDead()
    {
        if (!enemyIsDead)
        {
            target = target.transform.GetChild(7).transform;
            if (ChaseStateRunning != null)
            {
                StopCoroutine(ChaseStateRunning);
                ChaseStateRunning = null;
            }

            if (AttackStateRunning != null)
            {
                StopCoroutine(AttackStateRunning);
                AttackStateRunning = null;
            }

            if (IdleStateRunning == null)
            {
                IdleStateRunning = StartCoroutine(IdleState());
            }            
        }
    }

    public void EnemyMovement()
    {
        // updateEnemyYPos to keep enemy above terrain
        updateEnemyYPos = transform.position;
        freezeEnemyXZRotation = transform.rotation;
        freezeEnemyXZRotation.x = 0f;
        freezeEnemyXZRotation.z = 0f;
        transform.rotation = freezeEnemyXZRotation;

        // A Ray to make sure that enemies stay above terrain
        enemyTerrainRay = new Ray (transform.position, Vector3.down);
        if (Physics.Raycast(enemyTerrainRay, out enemyTerrainRayInfo, 20f))
        {
            Debug.DrawLine(enemyTerrainRay.origin, enemyTerrainRayInfo.point, Color.red);

            if (enemyTerrainRayInfo.collider.gameObject.tag == "Terrain")
            {
                updateEnemyYPos.y = enemyTerrainRayInfo.point.y;
                updateEnemyYPos.y += 0.1f;
                transform.position = updateEnemyYPos;
            }
        }
        else
        {
            Debug.DrawLine(enemyTerrainRay.origin, enemyTerrainRay.origin + enemyTerrainRay.direction, Color.green);
        }
    }

    IEnumerator IdleState()
    {
        if (!enemyIsIdle)
        {
            enemyIsIdle = true;
            enemyIsChasing = false;
            enemyIsAttacking = false;
            enemyAnimatorController.SetBool("isIdle", true);
            enemyAnimatorController.SetBool("isChasing", false);
            enemyAnimatorController.SetBool("isAttacking", false);
        }
        
        yield return null;
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

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.25f)
        {
            yield return new WaitForSeconds(0.25f);
        }

        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float squareMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (chasePlayer)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position-targetPosOld).sqrMagnitude > squareMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        while (chasePlayer)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (chasePlayer)
            {
                if (!enemyIsChasing)
                {                    
                    enemyIsIdle = false;
                    enemyIsChasing = true;
                    enemyIsAttacking = false;
                    enemyAnimatorController.SetBool("isIdle", false);
                    enemyAnimatorController.SetBool("isChasing", true);
                    enemyAnimatorController.SetBool("isAttacking", false);
                }          

                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * enemySpeed, Space.Self);
            }

            yield return null;
        }
    }

    IEnumerator AttackState()
    {
        if (!enemyIsAttacking)
        {            
            enemyIsIdle = false;
            enemyIsChasing = false;
            enemyIsAttacking = true;
            enemyAnimatorController.SetBool("isIdle", false);
            enemyAnimatorController.SetBool("isChasing", false);
            enemyAnimatorController.SetBool("isAttacking", true);
        }


        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);    

        yield return null;
    }

    IEnumerator DeathState()
    {
        enemyAnimatorController.SetBool("isIdle", false);
        enemyAnimatorController.SetBool("isChasing", false);
        enemyAnimatorController.SetBool("isAttacking", false);
        enemyAnimatorController.SetBool("isDead", true);

        if (!shardSpawned)
        {
            StartCoroutine(SpawnShards());
        }

        GetComponent<Collider>().isTrigger = true;

        yield return new WaitForSeconds(5f);

        ObjectPooler.Instance.ReturnToPool(gameObject, gameObject.tag);
        OnReturnToPool(gameObject);  

        yield return null;
    }

    IEnumerator SpawnShards()
    {
        shardSpawned = true;

        int hpShardSpawnChance =  UnityEngine.Random.Range(1,6);
        int hpShardSpawnRoll =  UnityEngine.Random.Range(1,6);

        int expShardSpawnChance = UnityEngine.Random.Range(1,5);
        int expShardSpawnRoll = UnityEngine.Random.Range(1,5);


        if (hpShardSpawnChance == hpShardSpawnRoll)
        {
            ObjectPooler.Instance.SpawnFromPool("HpShard", transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }

        if (expShardSpawnChance == expShardSpawnRoll)
        {
            ObjectPooler.Instance.SpawnFromPool("ExpShard", transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity); 
        }      

        yield return null;
    }

    void OnCollisionEnter(Collision other) 
    {
        if ((other.gameObject.tag == "TopGen") || (other.gameObject.tag == "RightGen") || (other.gameObject.tag == "BottomGen") || (other.gameObject.tag == "LeftGen"))
        {
            ObjectPooler.Instance.ReturnToPool(gameObject, gameObject.tag);
            OnReturnToPool(gameObject); 
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, enemyChaseRadius);
        
        /*
        if (path != null)
        {
            path.DrawWithGizmos();
        }
        */
    }
}