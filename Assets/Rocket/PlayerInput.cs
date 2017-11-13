using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput : MonoBehaviour {
	
	[SerializeField] float thrustForce;
	[SerializeField] float rotSpeed;

	Rigidbody rigidbody;
	RigidbodyConstraints rbConstraints;
	AudioSource audioSource;
	float thrust;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		rbConstraints = rigidbody.constraints;
		audioSource = GetComponent<AudioSource>();
	}

	void FixedUpdate () {
		Thrust();
		Rotate();
	}

	void Thrust () {
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		if (vertical > 0f) {
			rigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
		}
		else {
			audioSource.Stop();
		}
	}

	void Rotate () {
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

		if (horizontal < 0f) {
			rigidbody.freezeRotation = true;
			transform.Rotate(Vector3.forward * rotSpeed * Time.fixedDeltaTime);
		}
		if (horizontal > 0f) {
			rigidbody.freezeRotation = true;
			transform.Rotate(-Vector3.forward * rotSpeed * Time.fixedDeltaTime);
		}
		rigidbody.freezeRotation = false;
		rigidbody.constraints = rbConstraints;
	}
}
