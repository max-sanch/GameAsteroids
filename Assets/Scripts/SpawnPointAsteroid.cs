using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPointAsteroid : MonoBehaviour
{
    public int startCountAsteroid = 2;
    public static int countAsteroid;
    public static bool isStartCountAsteroid = false;
    static GameObject obj;
    static ObjectPooler objectPooler;
    static int spawnSide = 0;
    float timeLeftSpawn = 2f;

    void Start()
    {
        countAsteroid = startCountAsteroid;
        obj = gameObject;
        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        if (isStartCountAsteroid)
        {
            countAsteroid = startCountAsteroid;
            isStartCountAsteroid = false;
            RandomazeAsteroid();
        }
        if (timeLeftSpawn < 0)
        {
            countAsteroid += 1;
            RandomazeAsteroid();
            timeLeftSpawn = 2f;
        }
    }

    void FixedUpdate()
    {
        if (objectPooler.IsEveryoneDisappeared() && !PauseMenu.gameIsPause && !isStartCountAsteroid)
        {
            timeLeftSpawn -= Time.deltaTime;
        }
    }

    public static void RandomazeAsteroid()
    {
        for (int i = 0; i < countAsteroid; i++)
        {
            Quaternion rotation;
            Vector3 position = new Vector3();
            rotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
            if (spawnSide == 0)
            {
                position = new Vector3(Random.Range(-obj.transform.position.x, obj.transform.position.x), 1 , obj.transform.position.z);
                spawnSide = 1;
            }
            else if (spawnSide == 1)
            {
                position = new Vector3(-obj.transform.position.x, 1, Random.Range(-obj.transform.position.z, obj.transform.position.z));
                spawnSide = 2;
            }
            else if (spawnSide == 2)
            {
                position = new Vector3(Random.Range(-obj.transform.position.x, obj.transform.position.x), 1, -obj.transform.position.z);
                spawnSide = 3;
            }
            else if (spawnSide == 3)
            {
                position = new Vector3(obj.transform.position.x, 1, Random.Range(-obj.transform.position.z, obj.transform.position.z));
                spawnSide = 0;
            }
            objectPooler.SpawnFromPool("BigAsteroid", position, rotation);
        }
    }
}
