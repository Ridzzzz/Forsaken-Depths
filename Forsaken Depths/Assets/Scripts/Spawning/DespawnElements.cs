using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnElements : MonoBehaviour 
{
	void OnCollisionEnter(Collision other) 
	{
		if ((other.gameObject.tag == "HpShard") || (other.gameObject.tag == "ExpShard"))
        {
			ObjectPooler.Instance.ReturnToPool(other.gameObject, other.gameObject.tag);
		}
	}
}
