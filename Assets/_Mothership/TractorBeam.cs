using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour {

	[SerializeField] float pullSpeed = 5f;
	[SerializeField] float destroyThreshold = 0.1f;
	[SerializeField] Vector3 offset;
	
	ParticleSystem particleSystem;
	MeshRenderer meshRenderer;
	Grapple grapple;
	Pickup pickup;
	SpringJoint pickupSJ;
	Rigidbody pickupRB;


	void Start () {
		grapple = FindObjectOfType<Grapple>();
		particleSystem = GetComponent<ParticleSystem>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	void Update () {
		if (grapple.hasCargo || pickup != null) {
			particleSystem.Play();
			meshRenderer.enabled = true;
		}
		else {
			particleSystem.Stop();
			meshRenderer.enabled = false;
		}
	}

	//Captures the pickup
	void OnTriggerEnter (Collider other) {
		pickup = other.GetComponent<Pickup>();
		if (pickup) {
			pickupSJ = pickup.GetComponent<SpringJoint>();
			pickupRB = pickup.GetComponent<Rigidbody>();
			grapple.hasCargo = false;
			pickupRB.useGravity = false;
			pickupRB.isKinematic = true;
			pickupSJ.breakForce = 0;
			//TODO Add LineRenderer and Particles to show link between grapple and pickup.
		}

	}

	//Moves the pickup a location inside the mothership
	void OnTriggerStay (Collider other) {
		Pickup pickup = other.GetComponent<Pickup>();
		if (pickup) {
			Vector3 distance = (transform.position + offset) - pickup.transform.position;
			pickup.transform.position = Vector3.MoveTowards(pickup.transform.position, transform.position + offset, pullSpeed * Time.deltaTime);
			if (distance.magnitude < destroyThreshold) {
				Destroy(pickup.gameObject);
				pickup = null;
			}

		}
		//TODO Destroy pickup GO and Score points / store collected item;
	}
}