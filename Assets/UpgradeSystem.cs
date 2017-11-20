using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour {

	[Header("Engine:")]
	[SerializeField] bool[] hasThrust = { true, false, false, false };
	[SerializeField] float[] EngineThrustLevels = { 2000, 2500, 3000, 3500 };

	[SerializeField] bool[] hasFuelTank = { true, false, false, false };
	[SerializeField] float[] TankSizeLevels = { 50, 100, 150, 200 };

	[Header("Shield:")]
	[SerializeField] bool[] hasShield = { true, false, false, false };
	[SerializeField] float[] ShieldLevels = { 1f, 1.5f, 2f, 2.5f };
	
	[Header("Light:")]
	[SerializeField] bool[] hasLightStrength = { true, false, false, false};
	[SerializeField] bool[] hasLightAngle = { true, false, false, false };

	[SerializeField] float[] LightRangLevels = { 10, 15, 20, 30 };
	[SerializeField] float[] LightIntensityLevels = { 1, 2, 3, 4 };
	[SerializeField] float[] LightAngleLevels = { 15, 30, 45, 60 };

	[Header("Grapple:")]
	[SerializeField] bool[] hasGrapple = { true, false, false, false };
	//float[] grappleLevels = { 1f, 1.5f, 2f, 2.5f };

	[Header("Weapons:")]
	[SerializeField] bool[] hasWeapon = { true, false, false, false };
	//float[] WeaponLevels = { 1f, 1.5f, 2f, 2.5f };

	Rocket rocket;
	Light spotlight;
	
	// Use this for initialization
	void Start () {
		rocket = GetComponent<Rocket>();
		spotlight = GameObject.Find("RocketSpotlight").GetComponent<Light>();
		ThrustForce(0);
		TankSize(0);
		ShieldMultiplier(0);
		LightStrength(0);
		LightAngle(0);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			//Engine
			if (hasThrust[1]) {
				ThrustForce(1);
			}
			if (hasThrust[2]) {
				ThrustForce(2);
			}
			if (hasThrust[3]) {
				ThrustForce(3);
			}

			if (hasFuelTank[1]) {
				TankSize(1);
			}
			if (hasFuelTank[2]) {
				TankSize(2);
			}
			if (hasFuelTank[3]) {
				TankSize(3);
			}

			//Shield
			if (hasShield[1]) {
				ShieldMultiplier(1);
			}
			if (hasShield[2]) {
				ShieldMultiplier(2);
			}
			if (hasShield[3]) {
				ShieldMultiplier(3);
			}

			//Lights
			if (hasLightStrength[1]) {
				LightStrength(1);
			}
			if (hasLightStrength[2]) {
				LightStrength(2);
			}
			if (hasLightStrength[3]) {
				LightStrength(3);
			}

			if (hasLightAngle[1]) {
				LightAngle(1);
			}
			if (hasLightAngle[2]) {
				LightAngle(2);
			}
			if (hasLightAngle[3]) {
				LightAngle(3);
			}
		}

	}

	void ThrustForce (int index) {
		rocket.SetThrustForce(EngineThrustLevels[index]);
	}

	void TankSize (int index) {
		rocket.SetMaxFuel(TankSizeLevels[index]);
	}

	void ShieldMultiplier (int index) {
		rocket.SetShieldMultiplier(ShieldLevels[index]);
	}

	void LightStrength (int index) {
		spotlight.range = LightRangLevels[index];
		spotlight.intensity = LightIntensityLevels[index];
	}

	void LightAngle (int index) {
		spotlight.spotAngle = LightAngleLevels[index];
	}

}