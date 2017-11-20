using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rocket))]
public class InputControls : MonoBehaviour {

	Rocket rocket;
	float vertical;
	float horizontal;

	void Start () {
		rocket = GetComponent<Rocket>();
	}
	
	void FixedUpdate () {
		if (!rocket.isTransitioning) {
			RespondToThrust();
			RespondToRotate();
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
}
