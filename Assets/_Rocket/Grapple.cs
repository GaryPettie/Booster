﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Grapple : MonoBehaviour {

	[SerializeField] Vector3 pickupConnectedAnchor;
	
	float pickupRadius;
	public bool hasCargo = false;
	
	Rigidbody rigidbody;
	LineRenderer lineRenderer;
	Rigidbody dropPoint;
	Pickup pickup;
	SpringJoint pickupSJ;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		pickupRadius = GetComponent<SphereCollider>().radius;	
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update () {
		if (CrossPlatformInputManager.GetButtonUp("Fire1")) {
			hasCargo = false;
			Debug.Log("Here");
			if (hasCargo) {
				//dropPoint.transform.position = transform.position;
				pickupSJ.connectedBody = dropPoint;
				pickupSJ.autoConfigureConnectedAnchor = true;
				pickupSJ.connectedAnchor = Vector3.zero;
				dropPoint = null;
			}
		}
		DrawBeam();
	}

	void OnTriggerStay (Collider other) {
		if (!hasCargo && CrossPlatformInputManager.GetButton("Fire1")) {
			pickup = other.GetComponent<Pickup>();
			if (pickup) {
				dropPoint = pickup.transform.parent.GetComponent<Rigidbody>();
				pickupSJ = pickup.GetComponent<SpringJoint>();
				pickupSJ.autoConfigureConnectedAnchor = false;
				pickupSJ.connectedAnchor = pickupConnectedAnchor;
				pickupSJ.connectedBody = rigidbody;
				hasCargo = true;
				//TODO Add LineRenderer and Particles to show link between grapple and pickup.
				
				//TODO Add particle effect to mothership to show tractor field is active. 
			}
		}
	}

	void DrawBeam () {
		if (hasCargo) {
			lineRenderer.enabled = true;
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, pickup.transform.position);
		}
		else {
			lineRenderer.enabled = false;
		}
	}
}