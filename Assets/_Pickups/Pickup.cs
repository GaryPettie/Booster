using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	[SerializeField] PickupType m_pickupType;
	[SerializeField] float m_value;
	[SerializeField] float mass = 1.5f;

	public PickupType pickupType { get; private set; }
	public float value { get; private set; }

	public enum PickupType { FUEL, MONEY, PART }

	void Start () {
		pickupType = m_pickupType;
		value = m_value;
		GetComponent<Rigidbody>().mass = mass;
	}
}
