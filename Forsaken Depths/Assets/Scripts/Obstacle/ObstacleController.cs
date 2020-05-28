using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObstacleController : MonoBehaviour, IPoolable
{
    Coroutine InflictDamageRunning;
    public Action<GameObject> OnReturnToPool{get; set;}

    void OnEnable() 
    {
        InflictDamageRunning = null;
    }

    void OnCollisionEnter(Collision other) 
    {
        if ((other.gameObject.tag == "TopGen") || (other.gameObject.tag == "RightGen") || (other.gameObject.tag == "BottomGen") || (other.gameObject.tag == "LeftGen"))
        {
            ObjectPooler.Instance.ReturnToPool(gameObject, gameObject.tag);
            OnReturnToPool(gameObject); 
        }

        if (other.gameObject.tag == "Player")
        {
            if (InflictDamageRunning == null)
            {
                InflictDamageRunning = StartCoroutine(InflictDamage(other.gameObject));
            }
        }
    }

    void OnCollisionStay(Collision other) 
    {
        if (other.gameObject.tag == "Player")
        {
            if (InflictDamageRunning == null)
            {
                InflictDamageRunning = StartCoroutine(InflictDamage(other.gameObject));
            }
        }
    }

    IEnumerator InflictDamage(GameObject player)
    {
        player.GetComponent<PlayerStat>().TakeDamage(5, 0);

        yield return new WaitForSeconds(0.75f);

        InflictDamageRunning = null;
    }
}
