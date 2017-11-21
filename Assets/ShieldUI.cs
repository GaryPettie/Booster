using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour {

	[SerializeField] Image shield1;
	[SerializeField] Image shield2;
	[SerializeField] Image shield3;
	UpgradeSystem upgrades;
	bool[] shields;

	// Use this for initialization
	void Start () {
		upgrades = FindObjectOfType<UpgradeSystem>();
		shields = upgrades.GetShieldUnlocks();
	}

	// Update is called once per frame
	void Update () {
		if (shields[1]) {
			shield1.fillAmount = 1;
		}
		if (shields[2]) {
			shield2.fillAmount = 2;
		}
		if (shields[3]) {
			shield3.fillAmount = 3;
		}
	}
}
