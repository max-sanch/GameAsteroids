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
    float amplitude = 4;
    float speed = 10;
    float positionZ;
    float maxHeight;
    bool isRight;
    bool isUp = true;
    ObjectPooler objectPooler;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        timeLeftMoving = Random.Range(20f, 40f);
        timeLeftShoting = Random.Range(2f, 5f);
        maxHeight = pointBorder.position.z * 0.3f;
    }
    
    void Update()
    {
        if (timeLeftMoving < 0 && !ObjUFO.activeSelf)
        {
            Randomaze();
            ObjUFO.SetActive(true);
        }
        if ((ObjUFO.transform.position.z >= positionZ + amplitude && !isUp) ||
            (ObjUFO.transform.position.z <= positionZ - amplitude && isUp))
        {
            isUp = !isUp;
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
        float movingX = speed * Time.deltaTime;
        float movingZ = speed * Time.deltaTime;
        if (!isRight)
            movingX = -movingX;
        if (isUp)
            movingZ = -movingZ;

        if (timeLeftShoting < 0)
        {
            timeLeftShoting = Random.Range(2f, 5f);
            Shot();
        }
        ObjUFO.transform.Translate(movingX, 0, movingZ);
    }

    void Shot()
    {
        Vector3 difference = player.position - poolParent.transform.position;
        float rotateY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        objectPooler.SpawnFromPool("EnemyBullet", poolParent.transform.position, Quaternion.Euler(0f, rotateY, 0f));
    }

    void Randomaze()
    {
        positionZ = Random.Range(-maxHeight, maxHeight);
        ObjUFO.transform.position = new Vector3(-49.5f, 1, positionZ);
        isRight = Random.Range(0f, 1f) >= 0.5f;
    }
}
