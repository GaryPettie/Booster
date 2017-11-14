using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour {

	[SerializeField] Vector3 pickupConnectedAnchor;
	float pickupRadius;
	bool hasCargo = false;
	
	Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		pickupRadius = GetComponent<SphereCollider>().radius;	
	}

	void OnTriggerEnter (Collider other) {
		if (!hasCargo) {
			SpringJoint springJoint = other.GetComponent<SpringJoint>();
			if (springJoint) {
				springJoint.autoConfigureConnectedAnchor = false;
				springJoint.connectedAnchor = pickupConnectedAnchor;
				springJoint.connectedBody = rigidbody;
				//TODO Add LineRenderer and Particles to show link between grapple and pickup.
			}
		}
	}
}
