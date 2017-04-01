using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	Rigidbody rbody;

	public HerdManager herdManager;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {



		rbody.transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal")*moveSpeed*Time.deltaTime, 0, Input.GetAxisRaw("Vertical")*moveSpeed*Time.deltaTime));

		if(Input.GetButtonDown("Fire1"))
		{
			// left click
			// swing staff to get herd to move away from you
			herdManager.threatenHerd (transform.position, 5f, 2f, 20f);
		}
		if (Input.GetButtonDown ("Fire2")) {
			// right click
			// this might be used to control the sheepdog later
			// right now, whistle to get herd to follow you
			// pass in the transform to get the threat to follow you
			herdManager.movingThreat(transform, -4f, 4f, 35f);
		}
	}
}
