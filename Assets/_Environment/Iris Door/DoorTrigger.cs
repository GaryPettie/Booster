using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
	
	public bool isSwichActive = true;
	Animator anim;
	
	void Start () {
		anim = GetComponentInParent<Animator>();
	}

	public void OpenDoor () {
		if (isSwichActive) {
			anim.SetBool("isOpen", true);
		}
	}

	public void CloseDoor () {
		anim.SetBool("isOpen", false);
	}

	void OnTriggerEnter (Collider other) {
		if (!other.GetComponent<Grapple>() && other.GetComponentInParent<Rocket>()) {
			OpenDoor();
		}
	}


	void OnTriggerExit (Collider other) {
		if (!other.GetComponent<Grapple>() && other.GetComponentInParent<Rocket>()) {
			CloseDoor();
		}
	}
}
