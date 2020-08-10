using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
	public float speed = 5f;
	public float timeBeforeDisappearance = 2f;
	protected float timeLeft;
	ObjectPooler objectPooler;

	void Start()
	{
		objectPooler = ObjectPooler.Instance;
		timeLeft = timeBeforeDisappearance;
	}

	void FixedUpdate()
	{
		timeLeft -= (Time.deltaTime);
		if (timeLeft < 0)
			objectPooler.PoolsDisappearance(gameObject.tag, gameObject);

		if (gameObject.activeSelf)
			transform.Translate(0, 0, speed * 10 * Time.deltaTime);
	}
	public void OnObjectSpawn()
    {
		timeLeft = timeBeforeDisappearance;
	}
	void OnTriggerStay(Collider other)
	{
		if (gameObject.CompareTag("Bullet"))
        {
			if (other.CompareTag("Asteroid") || other.CompareTag("UFO"))
				objectPooler.PoolsDisappearance("Bullet", gameObject);
		}
		else if (gameObject.CompareTag("EnemyBullet"))
		{
			if (other.CompareTag("Asteroid") || other.CompareTag("Player"))
				objectPooler.PoolsDisappearance("EnemyBullet", gameObject);
		}
	}
}
