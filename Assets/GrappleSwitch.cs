using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleSwitch : MonoBehaviour {

	DoorTrigger doortrigger;
	Grapple grapple;

	// Use this for initialization
	void Start () {
		doortrigger = GetComponentInParent<DoorTrigger>();
		grapple = FindObjectOfType<Grapple>();
	}

	public void UnlockDoor () {
		doortrigger.isSwichActive = true;
		doortrigger.OpenDoor();
	}

	public void LockDoor () {
		doortrigger.isSwichActive = false;
		doortrigger.CloseDoor();
	}
}
