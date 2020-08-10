using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOManager : MonoBehaviour
{
    public Transform poolParent;
    public GameObject ObjUFO;
    public Transform player;
    public Transform pointBorder;
    public static float timeLeftMoving;
    float timeLeftShoting;
    bool isRight;
    ObjectPooler objectPooler;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        timeLeftMoving = Random.Range(20f, 40f);
        timeLeftShoting = Random.Range(2f, 5f);
    }
    
    void Update()
    {
        if (timeLeftMoving < 0 && !ObjUFO.activeSelf)
        {
            Randomaze();
            ObjUFO.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (!ObjUFO.activeSelf)
            timeLeftMoving -= Time.deltaTime;
        else
        {
            timeLeftShoting -= Time.deltaTime;
            Moving();
        }
    }

    void Moving()
    {
        if (isRight)
            ObjUFO.transform.Translate(10 * Time.deltaTime, 0, 0);
        else
            ObjUFO.transform.Translate(-10 * Time.deltaTime, 0, 0);
        if (timeLeftShoting < 0)
        {
            timeLeftShoting = Random.Range(2f, 5f);
            Shot();
        }

    }

    void Shot()
    {
        Vector3 difference = player.position - poolParent.transform.position;
        float rotateY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        objectPooler.SpawnFromPool("EnemyBullet", poolParent.transform.position, Quaternion.Euler(0f, rotateY, 0f));
    }

    void Randomaze()
    {
        float maxHeight = pointBorder.position.z * 0.3f;
        ObjUFO.transform.position = new Vector3(-49.5f, 1, Random.Range(-maxHeight, maxHeight));
        isRight = Random.Range(0f, 1f) >= 0.5f;
    }
}
