using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {
	
	[SerializeField] float maxHealth = 100f;
	[SerializeField] float maxShield = 100f;
	public float currentHealth { get; private set; }

	void Start () {
		currentHealth = maxHealth;
	}

	public void TakeDamage (float damage) {
		currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
		if (currentHealth <= 0) {
			Die();
		} 
	}

	public void Die () {
		Destroy(this.gameObject);
	}
}
