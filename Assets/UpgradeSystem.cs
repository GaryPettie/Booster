using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour {

	[Header("Engine:")]
	[SerializeField] float[] engineThrustLevels = { 2000, 2500, 3000, 3500 };
	[SerializeField] float[] fuelBurnMultiplier = { 1f, 1.25f, 1.5f, 1.75f };

	[SerializeField] float[] tankSizeLevels = { 50, 100, 150, 200 };

	[Header("Shield:")]
	[SerializeField] float[] shieldLevels = { 1f, 1.5f, 2f, 2.5f };

	[Header("Light:")]
	[SerializeField] float[] lightRangeLevels = { 10, 15, 20, 30 };
	[SerializeField] float[] lightIntensityLevels = { 1, 2, 3, 4 };
	[SerializeField] float[] lightAngleLevels = { 15, 30, 45, 60 };

	[Header("Grapple:")]
	[SerializeField] float[] grappleLevels = { 2.5f, 5f, 10f, 15f };

	[Header("Weapons:")]
	//[SerializeField] float[] WeaponLevels = { 1f, 1.5f, 2f, 2.5f };

	bool[] hasThrust = { true, false, false, false };
	bool[] hasFuelTank = { true, false, false, false };
	bool[] hasShield = { true, false, false, false };
	bool[] hasLightStrength = { true, false, false, false };
	bool[] hasLightAngle = { true, false, false, false };
	bool[] hasGrapple = { true, false, false, false };
	bool[] hasWeapon = { true, false, false, false };

	Rocket rocket;
	Light spotlight;
	Grapple grapple;

	void Start () {
		rocket = GetComponent<Rocket>();
		spotlight = GameObject.Find("RocketSpotlight").GetComponent<Light>();
		grapple = GetComponentInChildren<Grapple>();
	}


	public float GetMaxFuel () {
		return tankSizeLevels[tankSizeLevels.Length - 1];
	}

	public void ThrustForce (int index) {
		hasThrust[index] = !hasThrust[index];
		if (hasThrust[index]) {
			rocket.SetThrustForce(engineThrustLevels[index]);
			rocket.SetFuelBurnMultiplier(fuelBurnMultiplier[index]);
		}
	}

	public void TankSize (int index) {
		hasFuelTank[index] = !hasFuelTank[index];
		if (hasFuelTank[index]) {
			rocket.SetMaxFuel(tankSizeLevels[index]);
		}
	}

	public bool[] GetShieldUnlocks () {
		return hasShield;
	}

	public void ShieldMultiplier (int index) {
		hasShield[index] = !hasShield[index];
		if (hasShield[index]) {
			rocket.SetShieldMultiplier(shieldLevels[index]);
		}
	}

	public void LightStrength (int index) {
		hasLightStrength[index] = !hasLightStrength[index];
		if (hasLightStrength[index]) {
			spotlight.range = lightRangeLevels[index];
			spotlight.intensity = lightIntensityLevels[index];
		}
	}

	public void LightAngle (int index) {
		hasLightAngle[index] = !hasLightAngle[index];
		for (int i = 0; i < hasLightAngle.Length; i++) {
			if (hasLightAngle[i]) {
				spotlight.spotAngle = lightAngleLevels[i];
			}
		}
	}

	public void GrappleRadius (int index) {
		hasGrapple[index] = !hasGrapple[index];
		for (int i = 0; i < hasGrapple.Length; i++) {
			if (hasGrapple[i]) {
				grapple.SetGrappleRadius(grappleLevels[index]);
			}
		}
	}
}