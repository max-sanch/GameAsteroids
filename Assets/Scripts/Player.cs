using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float maxShipSpeed = 10f;
	public float acceleration = 2f;
	public float turnlSpeed = 2f;
	public Rigidbody rb;
	public GameObject ModelShip;
	public Transform poolParent;
	public AudioSource audioShot;
	public AudioSource audioMoving;
	public AudioSource audioExplosion;

	public static bool isRespawnPlayer = false;
	ObjectPooler objectPooler;
	float timeLeftBullet = 0.1f;
	float timeLeftInvul = 3f;
	float timer = 0.6f;
	bool isShot = true;
	bool isInvulnerability = true;
	bool isAudioMoving = false;

	void Start()
	{
		objectPooler = ObjectPooler.Instance;
	}

	void Update()
	{
		if (!PauseMenu.gameIsPause)
		{
			if (PauseMenu.isMouseAndKeyboard)
			{
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
					Shot();
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Space))
					Shot();
			}
		}
		else
			audioMoving.Stop();

		if (timeLeftBullet < 0)
			isShot = true;

		if (timeLeftInvul < 0)
		{
			isInvulnerability = false;
			ModelShip.SetActive(true);
		}

		if (isRespawnPlayer)
		{
			Respawn();
			DespawnBullet();
			isRespawnPlayer = false;
		}
	}

	void FixedUpdate()
	{
		Rotation();

		if (PauseMenu.isMouseAndKeyboard)
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0))
				Moving();
			else
            {
				audioMoving.Stop();
				isAudioMoving = false;
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				Moving();
			else
			{
				audioMoving.Stop();
				isAudioMoving = false;
			}
		}

		timeLeftBullet -= Time.deltaTime;
		if (isInvulnerability)
			Invulnerability();
	}

	void OnTriggerStay(Collider other)
	{
		if (!isInvulnerability)
		{
			if (other.CompareTag("Asteroid") || other.CompareTag("UFO") || other.CompareTag("EnemyBullet"))
			{
				GameStatistic.numberLives -= 1;
				audioExplosion.Play();
				Respawn();
			}
		}
	}

	void Invulnerability()
	{
		timeLeftInvul -= Time.deltaTime;
		timer -= Time.deltaTime;
		if (timer < 0)
		{
			ModelShip.SetActive(false);
			timer = 0.6f;
		}
		else if (timer < 0.45f)
		{
			ModelShip.SetActive(true);
		}

	}

	public void Respawn()
	{
		timeLeftInvul = 3f;
		isInvulnerability = true;
		transform.position = new Vector3(0, 1, 0);
		transform.rotation = new Quaternion(0, 0, 0, 0);
		rb.velocity *= 0;
	}

	void Shot()
	{
		if (isShot)
        {
			audioShot.Play();
			objectPooler.SpawnFromPool("Bullet", poolParent.transform.position, transform.rotation);
			timeLeftBullet = 0.1f;
			isShot = false;
		}
	}

	void Rotation()
	{
		if (PauseMenu.isMouseAndKeyboard)
		{ 
			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 99);
			Vector3 difference = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
			float rotateY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
			Quaternion toRotation = Quaternion.AngleAxis(rotateY, Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnlSpeed * 100 * Time.deltaTime);
		}
		else
        {
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				transform.Rotate(Vector3.up, -turnlSpeed * 100 * Time.deltaTime);


			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				transform.Rotate(Vector3.up, turnlSpeed * 100 * Time.deltaTime);
		}
	}

	void Moving()
	{
		if (rb.velocity.magnitude > maxShipSpeed)
			rb.velocity = rb.velocity.normalized * maxShipSpeed;
		else
			rb.AddForce(transform.forward * acceleration * 200 * Time.deltaTime);
		if (!isAudioMoving)
		{
			audioMoving.Play();
			isAudioMoving = true;
		}
	}

	void DespawnBullet()
	{
		List<string> tags = new List<string> { "Bullet", "EnemyBullet" };
		objectPooler.AllPoolsDisappearance(tags);
	}
}
