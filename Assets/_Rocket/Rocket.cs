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
	[SerializeField] ParticleSystem exhaustParticlesPrefab;

	#region Private Variables
	bool isTransitioning = false;

	public float currentHealth { get; private set; }
	public float currentFuel { get; private set; }
	Vector3 startLocation;
	ParticleSystem exhaustParticles;


	Rigidbody rigidbody;
	AudioSource audioSource;

	float fuelComsumption;
	float vertical;
	float horizontal;
	bool isThrusting = false;
	bool explosionCalled = false;
	#endregion

	#region Game Loop
	void Start () {
		Setup();
	}

	void FixedUpdate () {
		if (!isTransitioning) {
			RespondToThrust();
			RespondToRotate();
		}
	}
	#endregion	

	#region Setup & Reset
	void Setup () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		ResetRocket();
		exhaustParticles = Instantiate(exhaustParticlesPrefab);
		exhaustParticles.transform.SetParent(this.transform);
	}

	void ResetRocket () {
		isTransitioning = false;
		currentFuel = maxFuel;
		currentHealth = maxHealth;
	}
	#endregion

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
		rigidbody.angularVelocity = Vector3.zero;

		if(currentFuel > 0) {
			if (horizontal < 0f) {
				transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
			}
			if (horizontal > 0f) {
				transform.Rotate(-Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
			}
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
		isTransitioning = true;
		if (lives > 0) {
			ResetRocket();
			//TODO Write code to reset ship at launchpad
		}
		else {
			//TODO Write code to reload the game
			bool explosionCalled = false;
			//BUG Explosion sound can be called multiple times.
			if (!explosionCalled) {
				PlayExplosionAudio();
			}
			Destroy(gameObject);
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
		AudioManager.Play2DClipAtPoint(explosionAudio, transform.position, explosionVolume);
	}
	#endregion
}
