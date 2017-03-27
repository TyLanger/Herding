using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdMovement : MonoBehaviour {

	HerdManager herdManager;

	public Transform target;
	public float targetWeight;

	public Transform danger;
	public float dangerWeight;

	Rigidbody rBody;
	public Vector3 moveDirection;
	public float moveSpeed = 0.1f;



	[SerializeField]
	Vector3 alignment = Vector3.zero;
	public float alignmentWeight;
	[SerializeField]
	Vector3 cohesion = Vector3.zero;
	public float cohesionWeight;
	[SerializeField]
	Vector3 separation = Vector3.zero;
	public float separationWeight;
	[SerializeField]
	Vector3 threat = Vector3.zero;

	Vector3 lastAlignment;
	Vector3 lastCohesion;
	Vector3 lastSeparation;

	void Start () {
		alignment = Vector3.zero;
		cohesion = Vector3.zero;
		separation = Vector3.zero;

		threat = Vector3.zero;

		herdManager = FindObjectOfType<HerdManager> ();
		herdManager.addToHerd (this);
		moveDirection = Vector3.zero;

		lastAlignment = alignment;
		lastCohesion = cohesion;
		lastSeparation = separation;

		randomStats ();
	}

	void randomStats()
	{
		// numbers are just what looks about right
		alignmentWeight = Random.Range (0.5f, 3f);
		cohesionWeight = Random.Range (0.25f, 2f);
		// if separation weight was lower than 5, they tend to run into one another
		//separationWeight = Random.Range ();
		moveSpeed = Random.Range (0.2f, 0.4f);
		dangerWeight = Random.Range (0.85f, 1.1f);
	}

	void OnDrawGizmosSelected()
	{
		// multiply all by 10 so they are easier to see in game view
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, threat * 10);
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay (transform.position, alignment*10);
		Gizmos.color = Color.green;
		Gizmos.DrawRay (transform.position, cohesion*10);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay (transform.position, separation*10);
	}

	void FixedUpdate () {

		alignment = herdManager.computeAlignment (this) * alignmentWeight;
		cohesion = herdManager.computeCohesion (this) * cohesionWeight;
		separation = herdManager.computeSeparation (this) * separationWeight;


		// hopefully this smooths things out slightly
		// without this, the separate herds pull apart easier
		// seems to be less stuttering around where there's the tug-of-war
		/*
		alignment = ((alignment + lastAlignment) / 2f) ;
		cohesion = ((cohesion + lastCohesion) / 2f) ;
		separation = ((separation + lastSeparation) / 2f) ;
		*/

		lastAlignment = alignment;
		lastSeparation = separation;
		lastCohesion = cohesion;



		moveDirection = alignment  + cohesion  + separation ;
		// move towards a target position
		// the closer you are, the harder the pull in that direction
		// for now, 100 is just an arbitrary number. This should be the max range you can see the target from. Like cohesionDistance, etc. in HerdManager
		// if the magic number is bigger than the actual distance, they may not travel towards the target.
		// max so that the herd doesn't move away from the target when further away than max range
		// 0.5f is so that the herd doesn't get totally lost. They will alway pull a little bit towards the target
		moveDirection += (target.position-transform.position).normalized * targetWeight * Mathf.Max(0.5f, (120f - Vector3.Distance(target.position, transform.position))) / 40f;

		// move away from the danger
		// closer you are, stronger push away is
		// set threat so that you can see it in game as a ray
		threat = (transform.position - danger.position).normalized * dangerWeight * Mathf.Max(0.01f, (10f - Vector3.Distance(transform.position, danger.position)));
		moveDirection += threat;

		// normalized or normalize?
		// normalize sets it to magnitude 1
		// normalized leaves it as is, but uses the mag 1 version
		// Normalize so you don't move really fast sometimes and really slow others

		// gets messed up when the objects collide with one another and change their rotation
		//transform.Translate (moveDirection.normalized * moveSpeed);

		// doesn't get as messsed up by collisions
		transform.position = Vector3.MoveTowards (transform.position, transform.position + moveDirection, moveSpeed);
	}
}
