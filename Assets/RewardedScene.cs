using System.Collections;
using System.Collections.Generic;
using TraxnetPlusSDK;
using UnityEngine;

public class RewardedScene : MonoBehaviour {

	private readonly string ZONE_ID = "5cd7e1f1c94ec10001086e5f";

	public void Request () {
		TraxnetPlus.requestRewardedVideo (ZONE_ID,

			(string zoneId) => {
				Debug.Log ("on response " + zoneId);
			},
			(TraxnetError error) => {
				Debug.Log ("Error " + error.message);
			}
		);
	}

	public void Show () {
		TraxnetPlus.showAd (ZONE_ID,

			(string zoneId) => {
				Debug.Log ("onOpenAd " + zoneId);
			},
			(string zoneId) => {
				Debug.Log ("onCloseAd " + zoneId);
			},
			(string zoneId) => {
				Debug.Log ("onReward " + zoneId);
			},
			(TraxnetError error) => {
				Debug.Log ("onError " + error.message);
			}
		);
	}
}