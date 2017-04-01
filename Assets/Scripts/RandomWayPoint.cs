using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWayPoint : MonoBehaviour {

	// this is mostly just for testing to see if the herding works

	void OnTriggerEnter(Collider col)
	{
		transform.position = new Vector3 (Random.Range (-70f, 70f), 0, Random.Range (-70f, 70f));
	}
}
