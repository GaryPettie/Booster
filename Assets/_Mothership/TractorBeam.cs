using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour {
	
	[SerializeField] float pullSpeed = 5f;
	[SerializeField] float destroyThreshold = 0.1f;
	[SerializeField] Vector3 offset;
	[SerializeField] GameObject[] lightsPrefabs;
	[SerializeField] ParticleSystem particlesPrefab;


	ParticleSystem particleSystem;
	List<GameObject> lights = new List<GameObject>();
	Grapple grapple;
	Pickup pickup;
	SpringJoint pickupSJ;
	Rigidbody pickupRB;


	void Start () {
		grapple = FindObjectOfType<Grapple>();
		particleSystem = Instantiate(particlesPrefab, transform.position, Quaternion.identity, transform);
		for (int i = 0; i < lightsPrefabs.Length; i++) {
			GameObject light = Instantiate(lightsPrefabs[i], lightsPrefabs[i].transform.position, lightsPrefabs[i].transform.rotation, transform);
			lights.Add(light);
			light.SetActive(false);
		}
	}

	void Update () {
		if ((grapple.hasCargo || pickup != null)) {
			particleSystem.Play();
			foreach (GameObject light in lights) {
				light.SetActive(true);
			}

		}
		else {
			particleSystem.Stop();
			foreach (GameObject light in lights) {
				light.SetActive(false);
			}
		}
	}

	//Captures the pickup
	void OnTriggerEnter (Collider other) {
		pickup = other.GetComponent<Pickup>();
		if (pickup) {
			pickupSJ = pickup.GetComponent<SpringJoint>();
			pickupRB = pickup.GetComponent<Rigidbody>();
			pickupRB.useGravity = false;
			pickupRB.isKinematic = true;
			pickupSJ.breakForce = 0;
			//TODO Add LineRenderer and Particles to show link between grapple and pickup.
		}

	}

	//Moves the pickup a location inside the mothership
	void OnTriggerStay (Collider other) {
		Pickup pickup = other.GetComponent<Pickup>();
		if (pickup) {
			grapple.hasCargo = false;
			Vector3 distance = (transform.position + offset) - pickup.transform.position;
			pickup.transform.position = Vector3.MoveTowards(pickup.transform.position, transform.position + offset, pullSpeed * Time.deltaTime);
			if (distance.magnitude < destroyThreshold) {
				Destroy(pickup.gameObject);
				pickup = null;
			}

		}
		//TODO Destroy pickup GO and Score points / store collected item;
	}
}