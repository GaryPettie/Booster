using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

	Image healthbar;
	Rocket rocket;

	// Use this for initialization
	void Start () {
		healthbar = GetComponent<Image>();
		rocket = FindObjectOfType<Rocket>();
	}
	
	// Update is called once per frame
	void Update () {
		healthbar.fillAmount = rocket.GetCurrentHealth() / rocket.GetMaxHealth();
	}
}
