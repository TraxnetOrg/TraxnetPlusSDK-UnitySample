using System.Collections;
using System.Collections.Generic;
using TraxnetPlusSDK;
using UnityEngine;

public class InterstititalScene : MonoBehaviour {

	private readonly string ZONE_ID = "5ce8dd128f0dc300014a9039";

	public void Request () {
		TraxnetPlus.requestInterstitial (ZONE_ID,

			(string zoneId) => {
				Debug.Log ("on response" + zoneId);
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
			});
	}
}