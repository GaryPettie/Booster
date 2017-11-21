using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Vectrosity;

public class Grapple : MonoBehaviour {

	[SerializeField] Vector3 pickupConnectedAnchor;
	[SerializeField] Color grappleColor;
	[SerializeField] Material grappleMaterial;

	SphereCollider grappleCollider;
	public bool hasCargo = false;
	
	Rigidbody rigidbody;
	VectorLine grappleLine;
	VectorLine grappleArea;
	Rigidbody dropPoint;
	Pickup pickup;
	SpringJoint pickupSJ;
	GrappleSwitch grappleSwitch;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		grappleCollider = GetComponent<SphereCollider>();
		SetupGrappleLine();
		//DrawGrappleRadius();
	}

	void Update () {
		DrawBeam();
		if (CrossPlatformInputManager.GetButtonUp("Fire1")) {
			if (hasCargo) {
				grappleLine.active = false;
				if (pickup) {
					ReleasePickup();
				}
			}
			hasCargo = false;
		}
	}

	void OnTriggerStay (Collider other) {
		if (!hasCargo && CrossPlatformInputManager.GetButton("Fire1")) {
			grappleSwitch = other.GetComponent<GrappleSwitch>();
			pickup = other.GetComponent<Pickup>();

			if (grappleSwitch) {
				GrabSwitch();
			}

			if (pickup) {
				pickupSJ = pickup.GetComponent<SpringJoint>();
				if (pickupSJ) {
					GrabPickup();
				}
			}
		}
	}

	void GrabSwitch () {
		hasCargo = true;
		grappleSwitch.UnlockDoor();
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

	void SetupGrappleLine () {
		List<Vector3> linePoints = new List<Vector3>();
		linePoints.Add(transform.position);
		linePoints.Add(transform.position);
		grappleLine = new VectorLine("GrappleLine", linePoints, 12f);
		grappleLine.color = grappleColor;
		grappleLine.material = grappleMaterial;
	}

	void DrawGrappleLine (Vector3 endpoint) {
		grappleLine.active = true;
		grappleLine.points3[0] = transform.position;
		grappleLine.points3[1] = endpoint;
		grappleLine.Draw3D();
	}

	void DrawBeam () {
		if (hasCargo) {
			if (pickup) {
				DrawGrappleLine(pickup.transform.position);
			}
			else if (grappleSwitch) {
				DrawGrappleLine(grappleSwitch.transform.position);
			}
		}
		else {
			grappleLine.active = false;
		}
	}


	//TODO Add code to fade out line over time.
	//TODO Add code to make the drawn circle follow the player
	public void DrawGrappleRadius() {
		if (grappleArea != null) {
			VectorLine.Destroy(ref grappleArea);
		}
		List<Vector3> linepoints = new List<Vector3>();
		for (int i = 0; i < 50; i++) {
			linepoints.Add(Vector3.zero);
		}
		grappleArea = new VectorLine("GrappleRadius", linepoints, 5f, LineType.Continuous);
		grappleArea.color = grappleColor;
		grappleArea.material = grappleMaterial;
		grappleArea.MakeCircle(transform.position, grappleCollider.radius);
		grappleArea.Draw3D();
	}

	public void RemoveDrawnGrappleRadius () {
		VectorLine.Destroy(ref grappleArea);
	}

	public void SetGrappleRadius (float anAmount) {
		grappleCollider.radius = anAmount;
	}
}
