using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour {

	[Header("Engine:")]
	[SerializeField] float[] EngineThrustLevels = { 2000, 2500, 3000, 3500 };

	[SerializeField] float[] TankSizeLevels = { 50, 100, 150, 200 };

	[Header("Shield:")]
	[SerializeField] float[] ShieldLevels = { 1f, 1.5f, 2f, 2.5f };

	[Header("Light:")]
	[SerializeField] float[] LightRangeLevels = { 10, 15, 20, 30 };
	[SerializeField] float[] LightIntensityLevels = { 1, 2, 3, 4 };
	[SerializeField] float[] LightAngleLevels = { 15, 30, 45, 60 };

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

	// Use this for initialization
	void Start () {
		rocket = GetComponent<Rocket>();
		spotlight = GameObject.Find("RocketSpotlight").GetComponent<Light>();
		grapple = GetComponentInChildren<Grapple>();
	}


	public void ThrustForce (int index) {
		hasThrust[index] = !hasThrust[index];
		if (hasThrust[index]) {
			rocket.SetThrustForce(EngineThrustLevels[index]);
		}
	}

	public void TankSize (int index) {
		hasFuelTank[index] = !hasFuelTank[index];
		if (hasFuelTank[index]) {
			rocket.SetMaxFuel(TankSizeLevels[index]);
		}
	}

	public void ShieldMultiplier (int index) {
		hasShield[index] = !hasShield[index];
		if (hasShield[index]) {
			rocket.SetShieldMultiplier(ShieldLevels[index]);
		}
	}

	public void LightStrength (int index) {
		hasLightStrength[index] = !hasLightStrength[index];
		if (hasLightStrength[index]) {
			spotlight.range = LightRangeLevels[index];
			spotlight.intensity = LightIntensityLevels[index];
		}
	}

	public void LightAngle (int index) {
		hasLightAngle[index] = !hasLightAngle[index];
		for (int i = 0; i < hasLightAngle.Length; i++) {
			if (hasLightAngle[i]) {
				spotlight.spotAngle = LightAngleLevels[i];
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