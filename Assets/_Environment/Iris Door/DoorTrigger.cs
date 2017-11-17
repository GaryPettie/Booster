using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	Animator anim;
	
	void Start () {
		anim = GetComponentInParent<Animator>();
	}

	void OnTriggerEnter (Collider other) {
		if (!other.GetComponent<Grapple>() && other.GetComponentInParent<Rocket>()) {
			anim.SetBool("isOpen", true);
		}
	}
	void OnTriggerExit (Collider other) {
		if (!other.GetComponent<Grapple>() && other.GetComponentInParent<Rocket>()) {
			anim.SetBool("isOpen", false);
		}
	}
}
