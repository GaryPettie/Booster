using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

	[SerializeField] float impactThreshold;
	[SerializeField] float damageMultiplier;


	Collider col;

	void Start () {
		col = GetComponentInChildren<Collider>();
	}

	void OnCollisionEnter (Collision other) {
		Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
		if (damageableComponent) {
			float otherSpeed = other.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
			if (otherSpeed > impactThreshold) {
				float damage = otherSpeed * damageMultiplier;
				(damageableComponent as IDamageable).TakeDamage(damage);
				Debug.Log(other.gameObject.GetComponent<Player>().currentHealth);
			}
		}
	}
}
