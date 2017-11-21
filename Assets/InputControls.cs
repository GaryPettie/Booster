using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rocket))]
public class InputControls : MonoBehaviour {

	Rocket rocket;
	Grapple grapple;
	float vertical;
	float horizontal;

	void Start () {
		rocket = GetComponent<Rocket>();
		grapple = GetComponentInChildren<Grapple>();
	}
	
	void FixedUpdate () {
		if (!rocket.isTransitioning) {
			RespondToThrust();
			RespondToRotate();
			ShowGrappleRadius();
		}
	}

	void RespondToThrust () {
		vertical = CrossPlatformInputManager.GetAxis("Vertical");
		if (vertical > 0f && rocket.GetCurrentFuel() > 0) {
			rocket.ApplyThrust();
			rocket.BurnFuel();
			rocket.PlayEngineEffects();
		}
		else {
			rocket.StopEngineEffects();
		}
	}

	void RespondToRotate () {
		horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

		if (rocket.GetCurrentFuel() > 0) {
			if (horizontal < 0f) {
				rocket.ApplyRotation(Vector3.forward);
			}
			if (horizontal > 0f) {
				rocket.ApplyRotation(-Vector3.forward);
			}
		}
	}

	//TODO Move grapple input here.
	void ShowGrappleRadius () {
		if (CrossPlatformInputManager.GetButtonDown("Fire3")) {
			grapple.DrawGrappleRadius();
		}
		if (CrossPlatformInputManager.GetButtonUp("Fire3")) {
			grapple.RemoveDrawnGrappleRadius();
		}
	}
}
