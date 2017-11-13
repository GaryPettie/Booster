using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput : MonoBehaviour {
	
	[SerializeField] float thrustForce;
	[SerializeField] float rotSpeed;

	Rigidbody rigidbody;
	AudioSource audioSource;
	float thrust;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update () {
		//Debug.Log("H: " + CrossPlatformInputManager.GetAxis("Horizontal"));
		//Debug.Log("V: " + CrossPlatformInputManager.GetAxis("Vertical"));
		//Debug.Log("T: " + CrossPlatformInputManager.GetButton("Fire1"));
		//Debug.Log("F: " + CrossPlatformInputManager.GetButton("Fire2"));
		AddThrust();
		AddRotation();
	}

	void AddThrust () {
		float horizontal = CrossPlatformInputManager.GetAxis("Vertical");
		if (horizontal > 0f) {
			rigidbody.AddRelativeForce(transform.up * thrustForce * Time.deltaTime);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
		}
		else {
			audioSource.Stop();
		}
	}

	void AddRotation () {
		float vertical = CrossPlatformInputManager.GetAxis("Horizontal");
		if (vertical < 0f) {
			transform.Rotate(transform.forward * rotSpeed * Time.deltaTime);
		}
		if (vertical > 0f) {
			transform.Rotate(-transform.forward * rotSpeed * Time.deltaTime);
		}
	}
}
