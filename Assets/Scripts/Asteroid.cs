using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 5f;
    public static bool isDespawnAsteroid = false;

    ObjectPooler objectPooler;

    protected void OnTriggerAsteroid(string tag, Collider other, GameObject obj)
    {
        objectPooler = ObjectPooler.Instance;
        if (other.CompareTag("Player") || other.CompareTag("UFO"))
        {
            objectPooler.PoolsDisappearance(tag, obj);
        }
        else if (other.CompareTag("Bullet") || other.CompareTag("EnemyBullet"))
        {
            Quaternion rotation = obj.transform.rotation;
            objectPooler.PoolsDisappearance(tag, obj);
            if (tag == "BigAsteroid")
                ShardSpawn("MiddleAsteroid", rotation);
            else if (tag == "MiddleAsteroid")
                ShardSpawn("SmallAsteroid", rotation);

            if (other.CompareTag("Bullet"))
            {
                if (tag == "BigAsteroid")
                    GameStatistic.score += 20;
                else if (tag == "MiddleAsteroid")
                    GameStatistic.score += 50;
                else if (tag == "SmallAsteroid")
                    GameStatistic.score += 100;
            }
        }
    }

    protected void ShardSpawn(string tag, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(0f, 45f, 0f);
        objectPooler.SpawnFromPool(tag, transform.position, rotation);
        rotation *= Quaternion.Euler(0f, -90f, 0f);
        objectPooler.SpawnFromPool(tag, transform.position, rotation);
    }

    public static void DespawnAsteroid()
    {
        List<string> tags = new List<string> { "BigAsteroid", "MiddleAsteroid", "SmallAsteroid" };
        ObjectPooler.Instance.AllPoolsDisappearance(tags);
    }
}