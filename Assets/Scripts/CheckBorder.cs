using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBorder : MonoBehaviour
{
	public Transform positionPoint;
	
	void OnTriggerStay(Collider other)
	{
		if (positionPoint.transform.position.x == 0)
		{
			other.transform.position = new Vector3(other.transform.position.x,
				other.transform.position.y, positionPoint.transform.position.z);
		}
		else
		{
			other.transform.position = new Vector3(positionPoint.transform.position.x,
				other.transform.position.y, other.transform.position.z);
		}
	}
}
