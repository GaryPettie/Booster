using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketMonitor : MonoBehaviour {

	[SerializeField] Text healthText;
	[SerializeField] Text fuelText;

	Rocket player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Rocket>();
	}
	
	// Update is called once per frame
	void Update () {
		healthText.text = "Health: " + Mathf.Round(player.currentHealth * 100) / 100;
		fuelText.text = "Fuel: " + Mathf.Round(player.currentFuel * 100) / 100;
	}
}
