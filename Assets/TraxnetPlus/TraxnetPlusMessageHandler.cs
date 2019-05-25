using System;
using TraxnetPlusSDK;
using UnityEngine;

public class TraxnetPlusMessageHandler : MonoBehaviour {

	public void notifyRequestResponse (String zoneId) {
		Debug.Log ("notifyRequestResponse:" + zoneId);
		TraxnetPlus.onRequestResponse (zoneId);
	}

	public void notifyNativeRequestResponse (String body) {
		TraxnetNativeBannerAd result = new TraxnetNativeBannerAd ();
		result = JsonUtility.FromJson<TraxnetNativeBannerAd> (body);
		Debug.Log ("notifyNativeRequestResponse:" + result.zoneId);
		TraxnetPlus.onNativeRequestResponse (result);
	}

	public void notifyRequestError (String body) {
		TraxnetError error = new TraxnetError ();
		error = JsonUtility.FromJson<TraxnetError> (body);
		Debug.Log ("notifyRequestError:" + error.zoneId);
		TraxnetPlus.onRequestError (error);
	}

	public void notifyAdOpened (String zoneId) {
		Debug.Log ("notifyAdOpened:" + zoneId);
		TraxnetPlus.onOpenAd (zoneId);
	}

	public void notifyAdClosed (String zoneId) {
		Debug.Log ("notifyAdClosed:" + zoneId);
		TraxnetPlus.onCloseAd (zoneId);
	}

	public void notifyReward (String zoneId) {
		Debug.Log ("notifyReward:" + zoneId);
		TraxnetPlus.onReward (zoneId);
	}

	public void notifyError (String body) {
		TraxnetError error = new TraxnetError ();
		error = JsonUtility.FromJson<TraxnetError> (body);
		Debug.Log ("notifyError:" + error.zoneId);
		TraxnetPlus.onError (error);
	}
}