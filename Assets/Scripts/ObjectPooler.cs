using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int startSize = 1;

	}

	#region Singleton

	public static ObjectPooler Instance;

	private void Awake()
	{
		Instance = this;
	}

	#endregion

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;
	Dictionary<string, List<GameObject>> dictionaryActivePools;

	void Start()
	{
		poolDictionary = new Dictionary<string, Queue<GameObject>>();
		dictionaryActivePools = new Dictionary<string, List<GameObject>>();

		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();
			dictionaryActivePools.Add(pool.tag, new List<GameObject>());

			for (int i = 0; i < pool.startSize; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}
			poolDictionary.Add(pool.tag, objectPool);
		}
	}

	public void AddPools(Pool pool)
	{
		for (int i = 0; i < pool.startSize; i++)
		{
			GameObject obj = Instantiate(pool.prefab);
			obj.SetActive(false);
			poolDictionary[pool.tag].Enqueue(obj);
		}
	}

	public void SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (poolDictionary[tag].Count > 0)
		{
			GameObject objectToSpawn = poolDictionary[tag].Dequeue();

			objectToSpawn.SetActive(true);
			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;

			IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

			if (pooledObj != null)
				pooledObj.OnObjectSpawn();

			dictionaryActivePools[tag].Add(objectToSpawn);
		}
		else
        {
			foreach (Pool pool in pools)
            {
				if (pool.tag == tag)
					AddPools(pool);
            }
			SpawnFromPool(tag, position, rotation);
		}
	}

	public void PoolsDisappearance(string tag, GameObject myObject)
	{
		myObject.SetActive(false);
		int index = dictionaryActivePools[tag].IndexOf(myObject);
		GameObject objectToSpawn = dictionaryActivePools[tag][index];
		dictionaryActivePools[tag].RemoveAt(index);
		poolDictionary[tag].Enqueue(objectToSpawn);
	}

	public void AllPoolsDisappearance(List<string> tags)
    {
		foreach (string tag in tags)
        {
			for(int i = dictionaryActivePools[tag].Count - 1; i >= 0; i--)
            {
				PoolsDisappearance(tag, dictionaryActivePools[tag][i]);
			}
        }
    }

	public bool IsEveryoneDisappeared()
    {
		bool result = false;

		if (dictionaryActivePools["BigAsteroid"].Count == 0 &&
			dictionaryActivePools["MiddleAsteroid"].Count == 0 &&
			dictionaryActivePools["SmallAsteroid"].Count == 0)
			result = true;

		return result;
	}
}
