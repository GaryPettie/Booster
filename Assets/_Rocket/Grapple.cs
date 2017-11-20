using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Grapple : MonoBehaviour {

	[SerializeField] Vector3 pickupConnectedAnchor;
	
	SphereCollider grappleCollider;
	public bool hasCargo = false;
	
	Rigidbody rigidbody;
	LineRenderer lineRenderer;
	Rigidbody dropPoint;
	Pickup pickup;
	SpringJoint pickupSJ;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		grappleCollider = GetComponent<SphereCollider>();	
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update () {
		DrawBeam();
		if (CrossPlatformInputManager.GetButtonUp("Fire1")) {
			if (hasCargo) {
				ReleasePickup();
			}
			hasCargo = false;
		}
	}

	void OnTriggerStay (Collider other) {
		if (!hasCargo && CrossPlatformInputManager.GetButton("Fire1")) {
			pickup = other.GetComponent<Pickup>();
			if (pickup) {
				pickupSJ = pickup.GetComponent<SpringJoint>();
				if (pickupSJ) {
					GrabPickup();
				}				
			}
		}
	}

	void GrabPickup () {
		dropPoint = pickup.transform.parent.GetComponent<Rigidbody>();
		pickupSJ.autoConfigureConnectedAnchor = false;
		pickupSJ.connectedAnchor = pickupConnectedAnchor;
		pickupSJ.connectedBody = rigidbody;
		hasCargo = true;
	}

	void ReleasePickup () {
		pickupSJ.connectedBody = dropPoint;
		pickupSJ.autoConfigureConnectedAnchor = true;
		pickupSJ.connectedAnchor = Vector3.zero;
		dropPoint = null;
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

	public void SetGrappleRadius (float anAmount) {
		grappleCollider.radius = anAmount;
	}
}
