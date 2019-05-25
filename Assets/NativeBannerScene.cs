using System;
using System.Collections;
using System.Linq;
using ArabicSupport;
using TraxnetPlusSDK;
using UnityEngine;
using UnityEngine.UI;

public class NativeBannerScene : MonoBehaviour {

	private readonly string ZONE_ID = "5cd7e2e9c94ec10001086e62";
	public static TraxnetNativeBannerAd nativeAd = null;

	public void Request () {
		TraxnetPlus.requestNativeBanner (this, ZONE_ID,

			(TraxnetNativeBannerAd result) => {
				Debug.Log ("on response");
				NativeBannerScene.nativeAd = result;
			},
			(TraxnetError error) => {
				Debug.Log ("Error " + error.message);
			}
		);
	}

	void OnGUI () {
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (NativeBannerScene.nativeAd != null) {
			GUIStyle titleStyle = new GUIStyle ();
			titleStyle.alignment = TextAnchor.UpperRight;
			titleStyle.fontSize = 32;
			titleStyle.normal.textColor = Color.white;
			GUI.Label (new Rect (50, 500, 600, 50), ArabicFixer.Fix (NativeBannerScene.nativeAd.getTitle (), true), titleStyle);

			GUIStyle descriptionStyle = new GUIStyle ();
			descriptionStyle.richText = true;
			descriptionStyle.alignment = TextAnchor.MiddleRight;
			descriptionStyle.fontSize = 32;
			descriptionStyle.normal.textColor = Color.white;
			GUI.Label (new Rect (50, 550, 600, 50), ArabicFixer.Fix (NativeBannerScene.nativeAd.getDescription (), true), descriptionStyle);

			GUI.DrawTexture (new Rect (660, 500, 100, 100), NativeBannerScene.nativeAd.getIcon ());

			Rect callToActionRect;
			if (NativeBannerScene.nativeAd.getLandscapeBannerImage () != null) {
				GUI.DrawTexture (new Rect (50, 610, 710, 400), NativeBannerScene.nativeAd.getLandscapeBannerImage ());
				callToActionRect = new Rect (50, 1020, 710, 100);
			} else if (NativeBannerScene.nativeAd.getPortraitBannerImage () != null) {
				GUI.DrawTexture (new Rect (50, 300, 500, 280), NativeBannerScene.nativeAd.getPortraitBannerImage ());
				callToActionRect = new Rect (50, 580, 500, 50);
			} else {
				callToActionRect = new Rect (50, 300, 500, 50);
			}

			GUIStyle buttonStyle = new GUIStyle ("button");
			buttonStyle.fontSize = 32;
			if (GUI.Button (callToActionRect, ArabicFixer.Fix (NativeBannerScene.nativeAd.getCallToAction (), true), buttonStyle)) {
				NativeBannerScene.nativeAd.clicked ();
			}
		}
		#endif

	}
}