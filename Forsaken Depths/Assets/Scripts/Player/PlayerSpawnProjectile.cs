using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnProjectile : MonoBehaviour
{
    public ParticleSystem projectile;
    public ParticleSystem collisionEvent;
    Vector3 projectilePos;
    bool projectileTouchdown, spawnRayCheck;

    public GameObject Player;
    Vector3 spawnPosition;

    Rigidbody projectileRB;
    Collider projectileCollider;

    void Start()
    {
        projectileRB = GetComponent<Rigidbody>();
        projectileCollider = GetComponent<Collider>();
        collisionEvent.Stop();
        projectilePos = transform.position;
        projectileTouchdown = false;   
    }

    void FixedUpdate()
    {
        if (!projectileTouchdown)
        {
            projectilePos.y -= 0.1f;
            transform.position = projectilePos;
        }

        if (!spawnRayCheck)
        {
            PlayerSpawnPosition();
            spawnRayCheck = true;
        }        
    }

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Terrain")
        {
            StartCoroutine(ProjectileCollision());
        }
    }

    IEnumerator ProjectileCollision()
    {
        projectileRB.isKinematic = true;
        projectileCollider.isTrigger = true;
        projectileTouchdown = true;
        projectile.Stop();
        collisionEvent.Play();        

        Player.transform.position = spawnPosition;
        Player.SetActive(true);               

        yield return new WaitForSeconds(1f);

        collisionEvent.Stop();        

        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);

        yield return null;
    }

    void PlayerSpawnPosition()
    {        
        Ray checkGround;
        RaycastHit spawnPositionInfo;

        checkGround = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(checkGround, out spawnPositionInfo, 50f))
        {            
            if (spawnPositionInfo.collider.tag == "Terrain")
            {
                Debug.DrawLine(transform.position, spawnPositionInfo.point, Color.red);

                spawnPosition = spawnPositionInfo.point;
                spawnPosition.y += 1.05f;
            }
        }
    }
}
