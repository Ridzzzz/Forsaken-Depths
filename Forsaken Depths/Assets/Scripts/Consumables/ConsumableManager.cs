using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    public GameObject consumableBody;
    public ParticleSystem particleSystemToStop;
    bool shardCollected;
    GameObject playerGameObject;
    float despawnTimer = 25f;
    Material _myMaterial;
    Vector3 consumableScale;

    float upForce = 7f;
    float sideForce = 1.5f;

    PlayerStat consumableCollected;
    LayerMask playerLayer;
    Collider[] detectPlayer;
    float detectionRadius;
    bool _consumableCollected;

    void OnEnable()
    {
        shardCollected = false;
        _consumableCollected = false;
        playerLayer = LayerMask.GetMask("Player");
        detectionRadius = 2f;
        _myMaterial = GetComponentInChildren<Renderer>().material;
        consumableScale = new Vector3 (2f, 2f, 2f);

        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce/2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);
        Vector3 shardForce = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = shardForce;
    }

    void FixedUpdate() 
    {
        despawnTimer -= Time.deltaTime;
        
        if (shardCollected)
        {
            transform.position = Vector3.Lerp(transform.position, playerGameObject.transform.GetChild(1).GetComponent<Renderer>().bounds.center, 2.5f * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, consumableScale, 2.5f * Time.deltaTime);
            StartCoroutine(FadeTo(_myMaterial, 0f, 1f));
        }

        if (despawnTimer < 0f && !shardCollected)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;
            consumableBody.GetComponent<DespawnConsumable>().enabled = true;
        }

        if (!shardCollected)
        {
            detectPlayer = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
            if (detectPlayer.Length > 0)
            {
                foreach (Collider playerFound in detectPlayer)
                {
                    if (playerFound.tag == "Player")
                    {
                        GetComponent<Collider>().isTrigger = true;
                        GetComponent<Rigidbody>().isKinematic = true;
                        playerGameObject = playerFound.gameObject;
                        consumableCollected = playerGameObject.GetComponent<PlayerStat>();
                        shardCollected = true;
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        /*
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
            playerGameObject = other.gameObject;
            shardCollected = true;
        }
        */

        if (other.gameObject.tag == "TopGen" || other.gameObject.tag == "RightGen" || other.gameObject.tag == "BottomGen" || other.gameObject.tag == "LeftGen")
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    IEnumerator FadeTo(Material material, float targetOpacity, float duration) 
    {
        yield return new WaitForSeconds(0.5f);

        particleSystemToStop.Stop();

        yield return new WaitForSeconds(0.5f);

        if (gameObject.tag == "HpShard" && !_consumableCollected)
        {
            _consumableCollected = true;
            consumableCollected.HpShardCollected();
        }

        if (gameObject.tag == "ExpShard" && !_consumableCollected)
        {
            _consumableCollected = true;
            consumableCollected.ExpShardCollected();
        }

        gameObject.GetComponentInChildren<FloatEffects>().enabled = false;

        Color color = material.color;
        float startOpacity = color.a;
        float t = 0;

        while(t < duration) 
        {
            t += Time.deltaTime;

            float blend = Mathf.Clamp01(t / duration);

            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            material.color = color;

            yield return null;
        }

        ObjectPooler.Instance.ReturnToPool(gameObject, gameObject.tag);
    }
}
