using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour, IDamageable {

	[Header ("Movement:")]
	[SerializeField] float thrustForce = 1000;
	[SerializeField] float rotationSpeed = 150;
	[SerializeField] float maxFuel = 500;
	[SerializeField] float fuelBurnMultiplier = 1;

	[Header ("Health:")]
	[SerializeField] float maxHealth = 100f;
	[SerializeField] float maxShield = 100f;
	[SerializeField] int lives = 3;

	[Header ("Audio:")]
	[SerializeField] AudioClip engineAudio;
	[SerializeField] float engineVolume;

	[SerializeField] AudioClip explosionAudio;
	[SerializeField] float explosionVolume;

	[Header ("Visual:")]
	[SerializeField] ParticleSystem exhaustParticles;


	enum State { ALIVE, RESETTING, DYING, TRANSCENDING }

	State state;
	public float currentHealth { get; private set; }
	public float currentFuel { get; private set; }
	Vector3 startLocation;


	Rigidbody rigidbody;
	RigidbodyConstraints rbConstraints;
	AudioSource audioSource;

	float fuelComsumption;
	float vertical;
	float horizontal;
	bool isThrusting = false;


	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		rbConstraints = rigidbody.constraints;
		audioSource = GetComponent<AudioSource>();
		ResetRocket();
	}

	void FixedUpdate () {
		if (state == State.ALIVE) {
			//BUG Thrust audio continues after death
			RespondToThrust();
			RespondToRotate();
		}
		
	}

	void ResetRocket () {
		state = State.ALIVE;
		currentFuel = maxFuel;
		currentHealth = maxHealth;
	}

	#region Movement
	void RespondToThrust () {
		vertical = CrossPlatformInputManager.GetAxis("Vertical");
		if (vertical > 0f && currentFuel > 0) {
			ApplyThrust();
			BurnFuel();
			PlayEngineEffects();
		}
		else {
			StopEngineEffects();
		}
	}

	void ApplyThrust () {
		rigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
	}

	void BurnFuel () {
		fuelComsumption = fuelBurnMultiplier * Time.deltaTime;
		currentFuel = Mathf.Clamp(currentFuel - fuelComsumption, 0, maxFuel);
	}

	void RespondToRotate () {
		horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

		if(currentFuel > 0) {
			if (horizontal < 0f) {
				rigidbody.freezeRotation = true;
				transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
			}
			if (horizontal > 0f) {
				rigidbody.freezeRotation = true;
				transform.Rotate(-Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
			}
			rigidbody.freezeRotation = false;
			rigidbody.constraints = rbConstraints;
		}
	}
	#endregion

	#region Damage
	public void TakeDamage (float damage) {		
		//Destroys ship quicker when out of fuel
		if (currentFuel <= 0) {
			damage *= 10;
		}

		//Applies damage to the rocket
		currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
		if (currentHealth <= 0) {
			Die();
		} 
	}
	
	public void Die () {
		if (lives > 0) {
			state = State.RESETTING;
			ResetRocket();
			//TODO Write code to reset ship at launchpad
		}
		else {
			state = State.DYING;
			//TODO Write code to kill the player and reload the game
			PlayExplosionAudio();
		}
	}
	#endregion

	#region Effects
	void PlayEngineEffects () {
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(engineAudio, engineVolume);
		}
		exhaustParticles.Play();
	}

	void StopEngineEffects () {
		audioSource.Stop();
		exhaustParticles.Stop();
	}

	void PlayExplosionAudio () {
		audioSource.Stop();
		audioSource.PlayOneShot(explosionAudio, explosionVolume);
	}
	#endregion
}
