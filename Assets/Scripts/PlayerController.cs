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
			herdManager.threatenHerd (transform.position, 5f, 2f, 20f);
			//threaten
		}
	}
}
