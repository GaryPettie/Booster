using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TractorBeam : MonoBehaviour {
	
	[SerializeField] float pullSpeed = 5f;
	[SerializeField] float destroyThreshold = 0.1f;
	[SerializeField] Vector3 offset;
	[SerializeField] GameObject[] lightsPrefabs;
	[SerializeField] ParticleSystem particlesPrefab;

	//TODO This stuff needs to move with the Storage code:
	#region
	[SerializeField] Text partsText;
	[SerializeField] Text cashText;
	Rocket rocket;
	#endregion

	ParticleSystem particleSystem;
	List<GameObject> lights = new List<GameObject>();
	Grapple grapple;
	Pickup pickup;

	


	void Start () {
		Setup();

		//TODO refactor this method out of the class
		ClearUI();
	}

	void Update () {
		if ((grapple.hasCargo || pickup != null)) {
			BeamOn(true);
		}
		else {
			BeamOn(false);
		}
	}

	//Captures the pickup
	void OnTriggerEnter (Collider other) {
		pickup = other.GetComponent<Pickup>();
		if (pickup) {
			GrabPickup();
		}

	}

	//Moves the pickup a location inside the mothership
	void OnTriggerStay (Collider other) {
		pickup = other.GetComponent<Pickup>();
		if (pickup) {
			grapple.hasCargo = false;
			PullPickup();
		}
		//TODO Score points / store collected item;
	}

	void Setup () {
		grapple = FindObjectOfType<Grapple>();
		particleSystem = Instantiate(particlesPrefab, transform.position, Quaternion.identity, transform);
		for (int i = 0; i < lightsPrefabs.Length; i++) {
			GameObject light = Instantiate(lightsPrefabs[i], lightsPrefabs[i].transform.position, lightsPrefabs[i].transform.rotation, transform);
			lights.Add(light);
			light.SetActive(false);
		}
	}

	//BUG Effects sometimes switch off whilst pulling in the pickup.  
	void BeamOn (bool isOn) {
		if (isOn) {
			particleSystem.Play();
		}
		else {
			particleSystem.Stop();
		}
		foreach (GameObject light in lights) {
			light.SetActive(isOn);
		}
	}

	void GrabPickup () {
		SpringJoint pickupSJ = pickup.GetComponent<SpringJoint>();
		Rigidbody pickupRB = pickup.GetComponent<Rigidbody>();
		Collider pickupCol = pickup.GetComponent<Collider>();
		pickupCol.isTrigger = true;
		pickupRB.useGravity = false;
		pickupRB.isKinematic = true;
		pickupSJ.breakForce = 0;
	}

	void PullPickup () {
		Vector3 distance = (transform.position + offset) - pickup.transform.position;
		pickup.transform.position = Vector3.MoveTowards(pickup.transform.position, transform.position + offset, pullSpeed * Time.deltaTime);
		if (distance.magnitude <= destroyThreshold) {
			StorePickup();
			DestroyPickup();
		}
	}

	//TODO This probably shouldn't sit in this class...
	void ClearUI () {
		cashText.text = "Cash: \u0243 0";
		partsText.text = "Parts Collected: 0";
	}

	void StorePickup () {
		rocket = FindObjectOfType<Rocket>().GetComponent<Rocket>();
		switch (pickup.pickupType) {
			case Pickup.PickupType.FUEL:
				rocket.AddFuel(pickup.value);
				break;
			case Pickup.PickupType.MONEY:
				rocket.AddCash(pickup.value);
				cashText.text = "Cash: \u0243 " + rocket.currentCash;
				break;
			case Pickup.PickupType.PART:
				rocket.AddPart();
				partsText.text = "Parts Collected: " + rocket.currentParts;
				break;
			default:
				Debug.LogError("Unrecognised pickup type");
				break;
		}
	}

	void DestroyPickup () {
		Destroy(pickup.gameObject);
		pickup = null;
	}
}