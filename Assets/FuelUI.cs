using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour {

	Image fuelBar;
	Rocket rocket;
	UpgradeSystem upgrades;

	// Use this for initialization
	void Start () {
		fuelBar = GetComponent<Image>();
		rocket = FindObjectOfType<Rocket>();
		upgrades = FindObjectOfType<UpgradeSystem>();
	}

	// Update is called once per frame
	void Update () {
		fuelBar.fillAmount = rocket.GetCurrentFuel() / upgrades.GetMaxFuel();
	}
}
