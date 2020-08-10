using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAsteroid : Asteroid
{
	protected float newSpeed;

    void Start()
    {
		newSpeed = Random.Range(speed - 2f, speed + 2f);
	}

	void FixedUpdate()
	{
		if (gameObject.activeSelf)
			transform.Translate(0, 0, newSpeed * Time.deltaTime);
	}

	void OnTriggerStay(Collider other)
	{
		OnTriggerAsteroid("BigAsteroid", other, gameObject);
		newSpeed = Random.Range(speed - 2f, speed + 2f);
	}
}
