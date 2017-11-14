using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour, IDamageable {

	[Header ("Movement:")]
	[SerializeField] float thrustForce = 1000;
	[SerializeField] float rotSpeed = 150;
	[SerializeField] float maxFuel = 500;
	[SerializeField] float fuelConsumptionMultiplier = 1;

	[Header ("Health:")]
	[SerializeField] float maxHealth = 100f;
	[SerializeField] float maxShield = 100f;
	[SerializeField] int lives = 3;

	enum State { ALIVE, RESETTING, DYING, TRANSCENDING }

	State state	= State.ALIVE;
	public float currentHealth { get; private set; }
	public float currentFuel { get; private set; }

	Rigidbody rigidbody;
	RigidbodyConstraints rbConstraints;
	AudioSource audioSource;

	float fuelComsumption;
	float vertical;
	float horizontal;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		rbConstraints = rigidbody.constraints;
		audioSource = GetComponent<AudioSource>();
		currentFuel = maxFuel;
		currentHealth = maxHealth;
	}

	void FixedUpdate () {
		if (state == State.ALIVE) {
			//BUG Thrust audio continues after death
			Thrust();
			Rotate();
		}
		
	}

	void Thrust () {
		vertical = CrossPlatformInputManager.GetAxis("Vertical");
		if (vertical > 0f && currentFuel > 0) {
			fuelComsumption = fuelConsumptionMultiplier * Time.deltaTime;
			currentFuel = Mathf.Clamp(currentFuel - fuelComsumption, 0, maxFuel);
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
		horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

		if(currentFuel > 0) {
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

	public void TakeDamage (float damage) {		
		//Destroys ship quicker when out of fuel
		if (currentFuel <= 0) {
			damage *= 10;
		}

		currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
		if (currentHealth <= 0) {
			Die();
		} 
	}
	
	public void Die () {
		if (lives > 0) {
			state = State.RESETTING;
			//TODO Write code to reset ship at launchpad
		}
		else {
			state = State.DYING;
			//TODO Write code to kill the player and reload the game
		}
	}
}
