using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour, IDamageable {

	[Header ("Movement:")]
	[SerializeField] float thrustForce = 1000;
	[SerializeField] float rotationSpeed = 150;
	[SerializeField] float maxFuel = 500;
	[SerializeField] float fuelBurnMultiplier = 1f;

	[Header ("Health:")]
	[SerializeField] float maxHealth = 100f;
	[SerializeField] float shieldMultiplier = 1f;
	[SerializeField] int lives = 3;

	[Header ("Audio:")]
	[SerializeField] AudioClip engineAudio;
	[SerializeField] float engineVolume;

	[SerializeField] AudioClip explosionAudio;
	[SerializeField] float explosionVolume;

	[Header ("Visual:")]
	[SerializeField] ParticleSystem exhaustParticlesPrefab;
	[SerializeField] Vector3 exhaustOffset;

	#region Private Variables
	public bool isTransitioning = false;

	public float currentHealth { get; private set; }
	public float currentFuel { get; private set; }
	public float currentParts { get; private set; }
	public float currentCash { get; private set; }
	Vector3 startLocation;
	ParticleSystem exhaustParticles;


	Rigidbody rigidbody;
	AudioSource audioSource;

	float fuelComsumption;
	float vertical;
	float horizontal;
	#endregion

	#region Game Loop
	void Start () {
		Setup();
	}
	#endregion	

	#region Setup & Reset
	void Setup () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		startLocation = transform.position;
		ResetRocket();
		exhaustParticles = Instantiate(exhaustParticlesPrefab, transform.position + exhaustOffset, exhaustParticlesPrefab.transform.rotation, transform);
	}

	void ResetRocket () {
		isTransitioning = false;
		currentFuel = maxFuel;
		currentHealth = maxHealth;
		transform.position = startLocation;
		transform.rotation = Quaternion.identity;
		rigidbody.velocity = Vector3.zero;

	}
	#endregion

	#region Engine Management (Movement)
	public float GetCurrentFuel () {
		return currentFuel;
	}

	public float GetMaxFuel () {
		return maxFuel;
	}

	public void SetMaxFuel (float anAmount) {
		maxFuel = anAmount;
	}

	public float GetThrustForce () {
		return thrustForce;
	}

	public void SetThrustForce (float anAmount) {
		thrustForce = anAmount;
	}
	
	public void AddFuel (float anAmount) {
		currentFuel = Mathf.Clamp(currentFuel + anAmount, 0, maxFuel);
	}

	public void BurnFuel () {
		fuelComsumption = fuelBurnMultiplier * Time.deltaTime;
		currentFuel = Mathf.Clamp(currentFuel - fuelComsumption, 0, maxFuel);
	}

	public void ApplyThrust () {
		rigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
	}

	public void ApplyRotation (Vector3 direction) {
		rigidbody.angularVelocity = Vector3.zero;
		transform.Rotate(direction * rotationSpeed * Time.fixedDeltaTime);
	}
	#endregion


	#region Damage
	public void SetShieldMultiplier (float anAmount) {
		shieldMultiplier = anAmount;
	}

	public void TakeDamage (float damage) {		
		//Destroys ship quicker when out of fuel
		if (currentFuel <= 0) {
			damage = Mathf.Infinity;
		}

		//Applies damage to the rocket
		currentHealth = Mathf.Clamp(currentHealth - (damage / shieldMultiplier), 0, maxHealth);
		if (currentHealth <= 0) {
			Die();
		} 
	}
	
	public void Die () {
		isTransitioning = true;

		bool explosionCalled = false;
		//BUG Explosion sound can be called multiple times - seems to be calsued by the old movement bug.  Not sure if this is still here.  
		if (!explosionCalled) {
			PlayExplosionAudio();
		}

		if (lives > 0) {
			ResetRocket();
			lives--;
		}
		else {
			Destroy(gameObject);
		}
	}
	#endregion

	#region Effects
	public void PlayEngineEffects () {
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(engineAudio, engineVolume);
		}
		exhaustParticles.Play();
	}

	public void StopEngineEffects () {
		audioSource.Stop();
		exhaustParticles.Stop();
	}

	void PlayExplosionAudio () {
		AudioManager.Play2DClipAtPoint(explosionAudio, transform.position, explosionVolume);
	}
	#endregion

	#region Stuff to refactor
	public void AddPart () {
		currentParts++;
	}

	public void AddCash (float anAmount) {
		currentCash += anAmount;
	}
	#endregion
}
