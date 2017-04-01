using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour {

	HerdMovement[] herd;
	int maxHerdSize = 20;
	[SerializeField]
	int currentHerdSize = 0;

	public float alignmentDistance = 100f;
	public float cohesionDistance = 300f;
	public float minCohesionDistance = 50f;
	public float separationDistance = 50f;

	void Awake() {
		herd = new HerdMovement[maxHerdSize];
	}

	public void addToHerd(HerdMovement other)
	{
		herd [currentHerdSize] = other;
		currentHerdSize++;
	}

	public void threatenHerd(Vector3 threatPosition, float threatStrength, float threatDuration, float threatMaxRange)
	{
		for (int i = 0; i < currentHerdSize; i++) {
			herd [i].threaten (threatPosition, threatStrength, threatDuration, threatMaxRange);
		}
	}

	public void movingThreat(Transform threatTrans, float threatStrength, float threatDuration, float threatMaxRange)
	{
		for (int i = 0; i < currentHerdSize; i++) {
			herd [i].moveThreaten (threatTrans, threatStrength, threatDuration, threatMaxRange);
		}
	}

	public Vector3 computeAlignment(HerdMovement agent)
	{
		int neighbourCount = 0;
		Vector3 alignmentVector = Vector3.zero;

		for (int i = 0; i < currentHerdSize; i++) {
			HerdMovement current = herd[i];
			if (current == agent) {
				// skip yourself
				continue;
			}
			if (Vector3.Distance(agent.transform.position, current.transform.position) < alignmentDistance) {
				// both agents are close enough to be considered neighbours
				alignmentVector += current.moveDirection;
				neighbourCount++;
			}
		}


		// if no neighbours, just return 0 vector
		if (neighbourCount == 0) {
			return alignmentVector;

		}

		// divide the alignment by the number of neighbours
		alignmentVector /= neighbourCount;

		// return the normalized result
		return alignmentVector.normalized;
	}

	public Vector3 computeSeparation(HerdMovement agent)
	{
		int neighbourCount = 0;
		Vector3 separationVector = Vector3.zero;

		for (int i = 0; i < currentHerdSize; i++) {
			HerdMovement current = herd[i];
			if (current == agent) {
				// skip yourself
				continue;
			}
			if (Vector3.Distance(agent.transform.position, current.transform.position) < separationDistance) {
				// both agents are close enough to be considered neighbours

				// add the difference in positions together
				// multiply by the max distance minus the actual distance
				// gives greater weight when very close together
				separationVector += (separationDistance-Vector3.Distance(agent.transform.position, current.transform.position)) * (current.transform.position - agent.transform.position).normalized;
				neighbourCount++;
			}
		}


		// if no neighbours, just return 0 vector
		if (neighbourCount == 0) {
			return separationVector;

		}

		// divide the separation by the number of neighbours
		// this gives us the center of masss
		separationVector /= neighbourCount;

		// negate the separation so they move away from one another
		separationVector *= -1;

		// return the normalized result
		return separationVector;//.normalized;
	}

	public Vector3 computeCohesion(HerdMovement agent)
	{
		int neighbourCount = 0;
		Vector3 cohesionVector = Vector3.zero;

		for (int i = 0; i < currentHerdSize; i++) {
			HerdMovement current = herd[i];
			if (current == agent) {
				// skip yourself
				continue;
			}
			float dist = Vector3.Distance (agent.transform.position, current.transform.position);
			if (dist < cohesionDistance && dist > minCohesionDistance) {
				// both agents are close enough to be considered neighbours

				// add the positions together
				// multiply by the distance. This gives positions that are closer less weight. Should let the herd space out when they are all close together, but attract when far away.
				//TODO Make it threshold based. Members that are too close don't impact at all. Could be better
				cohesionVector += current.transform.position; // * Vector3.Distance(agent.transform.position, current.transform.position);
				neighbourCount++;
			}
		}


		// if no neighbours, just return 0 vector
		if (neighbourCount == 0) {
			return cohesionVector;

		}

		// divide the cohesion by the number of neighbours
		// this gives us the center of masss
		cohesionVector /= neighbourCount;

		// subtracting this agent's position gives the direction to the center of mass
		cohesionVector = cohesionVector - agent.transform.position;

		// return the normalized result
		return cohesionVector.normalized;
	}
}
