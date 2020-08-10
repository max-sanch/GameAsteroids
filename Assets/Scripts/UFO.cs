using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public static bool isDespawnUFO = false;

    void Update()
    {
        if (isDespawnUFO)
        {
            DespawnUFO();
            isDespawnUFO = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Asteroid") || other.CompareTag("Bullet") || other.CompareTag("Player"))
        {
            DespawnUFO();
            if (other.CompareTag("Bullet"))
                GameStatistic.score += 200;
        }
    }

    void DespawnUFO()
    {
        gameObject.SetActive(false);
        UFOManager.timeLeftMoving = Random.Range(20f, 40f);
    }
}
