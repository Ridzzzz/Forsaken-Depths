﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPoolable
{
    Action<GameObject> OnReturnToPool {get; set;}
}

public class ObjectPooler : MonoBehaviour 
{
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public static ObjectPooler Instance;

	private void Awake() 
	{
		Instance = this;
	}

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;
	
	void Start () 
	{
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach(Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			GameObject parentObj = new GameObject(pool.tag);
			parentObj.transform.parent = transform;

			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.transform.parent = parentObj.transform;
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
	}
	
	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " is not found.");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[tag].Dequeue();

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		return objectToSpawn;
	}

	public void ReturnToPool(GameObject gObjectToPool, string tag)
	{
		gObjectToPool.SetActive(false);
		poolDictionary[tag].Enqueue(gObjectToPool);
	}
}
