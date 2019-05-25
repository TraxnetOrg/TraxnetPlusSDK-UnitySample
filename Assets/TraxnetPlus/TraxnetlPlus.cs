using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace TraxnetPlusSDK {
	
	public class Gravity {
		public static int TOP = 1;
		public static int BOTTOM = 2;
		public static int LEFT = 3;
		public static int RIGHT = 4;
		public static int CENTER = 5;
	}

	public class BannerType {
		public static int BANNER_320x50 = 1;
		public static int BANNER_320x100 = 2;
		public static int BANNER_250x250 = 3;
		public static int BANNER_300x250 = 4;
		public static int BANNER_468x60 = 5;
		public static int BANNER_728x90 = 6;
	}
	
	[Serializable]
	public class TraxnetError {
		public string message;
		public string zoneId;
	}

	[Serializable]
	public class TraxnetNativeBannerAd {
		public string zoneId;
		public string adId;
		public string title;
		public string description;
		public string iconUrl;
		public string callToActionText;
		public string portraitStaticImageUrl;
		public string landscapeStaticImageUrl;

		public Texture2D portraitBannerImage;
		public Texture2D landscapeBannerImage;
		public Texture2D iconImage;

		public string getTitle () {
			return title;
		}

		public string getDescription () {
			return description;
		}

		public string getCallToAction () {
			return callToActionText;
		}

		public Texture2D getPortraitBannerImage () {
			return portraitBannerImage;
		}

		public Texture2D getLandscapeBannerImage () {
			return landscapeBannerImage;
		}

		public Texture2D getIcon () {
			return iconImage;
		}

		public void clicked () {
			TraxnetPlus.nativeBannerAdClicked (this.zoneId, this.adId);
		}
	}

	public class TraxnetPlus {
		#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaClass traxnetPlus;
		#endif

		private static Dictionary<string, Action<string>> requestResponsePool = new Dictionary<string, Action<string>> ();
		private static Dictionary<string, Action<TraxnetNativeBannerAd>> nativeBannerResponsePool = new Dictionary<string, Action<TraxnetNativeBannerAd>> ();
		private static Dictionary<string, Action<TraxnetError>> requestErrorPool = new Dictionary<string, Action<TraxnetError>> ();

		private static Dictionary<string, Action<string>> openAdPool = new Dictionary<string, Action<string>> ();
		private static Dictionary<string, Action<string>> closeAdPool = new Dictionary<string, Action<string>> ();
		private static Dictionary<string, Action<string>> rewardPool = new Dictionary<string, Action<string>> ();
		private static Dictionary<string, Action<TraxnetError>> errorPool = new Dictionary<string, Action<TraxnetError>> ();
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		private static MonoBehaviour mMonoBehaviour;
		#endif

		private static GameObject traxnetPlusManager = null;

		public static void initialize (string key) {
			if (traxnetPlusManager == null) {
				traxnetPlusManager = new GameObject ("TraxnetPlusManager");
				UnityEngine.Object.DontDestroyOnLoad (traxnetPlusManager);
				traxnetPlusManager.AddComponent<TraxnetPlusMessageHandler> ();
			}
			
			#if UNITY_ANDROID && !UNITY_EDITOR
			setJavaObject ();
			traxnetPlus.CallStatic ("initialize", key);
			#endif
		}

		private static void setJavaObject () {
			#if UNITY_ANDROID && !UNITY_EDITOR
			traxnetPlus = new AndroidJavaClass ("ee.traxnet.plus.TraxnetPlusUnity");
			#endif
		}

		public static void setDebugMode (int logLevel) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			traxnetPlus.CallStatic ("setDebugMode", logLevel);
			#endif
		}

		public static void requestRewardedVideo (
			string zoneId, Action<string> onRequestResponse, Action<TraxnetError> onRequestError) {

			#if UNITY_ANDROID && !UNITY_EDITOR
			if (requestResponsePool.ContainsKey (zoneId)) {
				requestResponsePool.Remove (zoneId);
			}

			if (requestErrorPool.ContainsKey (zoneId)) {
				requestErrorPool.Remove (zoneId);
			}

			requestResponsePool.Add (zoneId, onRequestResponse);
			requestErrorPool.Add (zoneId, onRequestError);

			traxnetPlus.CallStatic ("requestRewardedVideo", zoneId);
			#endif
		}

		public static void requestInterstitial (
			string zoneId, Action<string> onRequestResponse, Action<TraxnetError> onRequestError) {
				
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (requestResponsePool.ContainsKey (zoneId)) {
				requestResponsePool.Remove (zoneId);
			}

			if (requestErrorPool.ContainsKey (zoneId)) {
				requestErrorPool.Remove (zoneId);
			}

			requestResponsePool.Add (zoneId, onRequestResponse);
			requestErrorPool.Add (zoneId, onRequestError);

			traxnetPlus.CallStatic ("requestInterstitial", zoneId);
			#endif
		}

		public static void requestNativeBanner (
			MonoBehaviour monoBehaviour, string zoneId, Action<TraxnetNativeBannerAd> onRequestResponse, Action<TraxnetError> onRequestError) {
				
			#if UNITY_ANDROID && !UNITY_EDITOR
			mMonoBehaviour = monoBehaviour;

			if (nativeBannerResponsePool.ContainsKey (zoneId)) {
				nativeBannerResponsePool.Remove (zoneId);
			}

			if (requestErrorPool.ContainsKey (zoneId)) {
				requestErrorPool.Remove (zoneId);
			}

			nativeBannerResponsePool.Add (zoneId, onRequestResponse);
			requestErrorPool.Add (zoneId, onRequestError);

			traxnetPlus.CallStatic ("requestNativeBanner", zoneId);
			#endif
		}

		public static void onRequestResponse (String zoneId) {
			if (requestResponsePool.ContainsKey (zoneId)) {
				requestResponsePool[zoneId] (zoneId);
			}
		}

		public static void onNativeRequestResponse (TraxnetNativeBannerAd result) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			string zoneId = result.zoneId;
			if (result != null) {
				if (mMonoBehaviour != null && mMonoBehaviour.isActiveAndEnabled) {
					mMonoBehaviour.StartCoroutine (loadNativeBannerAdImages (result));
				} else {
					if (errorPool.ContainsKey (zoneId)) {
						TraxnetError error = new TraxnetError();
						error.zoneId = zoneId;
						error.message = "Invalid MonoBehaviour Object";
						errorPool[zoneId] (error);
					}
				}
			} else {
				if (requestErrorPool.ContainsKey (zoneId)) {
					TraxnetError error = new TraxnetError();
					error.zoneId = zoneId;
					error.message = "Invalid Result";
					errorPool[zoneId] (error);
				}
			}
			#endif
		}

		public static void onRequestError (TraxnetError error) {
			if (requestErrorPool.ContainsKey (error.zoneId)) {
				requestErrorPool[error.zoneId] (error);
			}
		}

		public static void showAd (
			string zoneId,
			Action<string> onShowAd,
			Action<string> onCloseAd,
			Action<string> onReward,
			Action<TraxnetError> onError) {
				
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (openAdPool.ContainsKey (zoneId)) {
				openAdPool.Remove (zoneId);
			}

			if (closeAdPool.ContainsKey (zoneId)) {
				closeAdPool.Remove (zoneId);
			}

			if (rewardPool.ContainsKey (zoneId)) {
				rewardPool.Remove (zoneId);
			}

			if (errorPool.ContainsKey (zoneId)) {
				errorPool.Remove (zoneId);
			}

			openAdPool.Add (zoneId, onShowAd);
			closeAdPool.Add (zoneId, onCloseAd);
			rewardPool.Add (zoneId, onReward);
			errorPool.Add (zoneId, onError);

			traxnetPlus.CallStatic ("showAd", zoneId);
			#endif
		}

		public static void onOpenAd (String zoneId) {
			if (openAdPool.ContainsKey (zoneId)) {
				openAdPool[zoneId] (zoneId);
			}
		}

		public static void onCloseAd (String zoneId) {
			if (closeAdPool.ContainsKey (zoneId)) {
				closeAdPool[zoneId] (zoneId);
			}
		}

		public static void onReward (String zoneId) {
			if (rewardPool.ContainsKey (zoneId)) {
				rewardPool[zoneId] (zoneId);
			}
		}

		public static void onError (TraxnetError error) {
			if (errorPool.ContainsKey (error.zoneId)) {
				errorPool[error.zoneId] (error);
			}
		}

		public static void showBannerAd (
			string zoneId, int bannerType, int horizontalGravity, int verticalGravity, 
				Action<string> onRequestResponse, Action<TraxnetError> onRequestError) {

			#if UNITY_ANDROID && !UNITY_EDITOR
			if (requestResponsePool.ContainsKey (zoneId)) {
				requestResponsePool.Remove (zoneId);
			}

			if (requestErrorPool.ContainsKey (zoneId)) {
				requestErrorPool.Remove (zoneId);
			}

			requestResponsePool.Add (zoneId, onRequestResponse);
			requestErrorPool.Add (zoneId, onRequestError);

			traxnetPlus.CallStatic ("showBannerAd", zoneId, bannerType, horizontalGravity, verticalGravity);
			#endif
		}

		public static void hideBanner () {
			#if UNITY_ANDROID && !UNITY_EDITOR
			traxnetPlus.CallStatic ("hideBanner");
			#endif
		}

		public static void nativeBannerAdClicked (string zoneId, string adId) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			traxnetPlus.CallStatic ("nativeBannerAdClicked", zoneId, adId);
			#endif
		}

		static IEnumerator loadNativeBannerAdImages (TraxnetNativeBannerAd result) {
			if (result.iconUrl != null && !result.iconUrl.Equals ("")) {
				UnityWebRequest wwwIcon = UnityWebRequestTexture.GetTexture (result.iconUrl);
				yield return wwwIcon.SendWebRequest ();
				if (wwwIcon.isNetworkError || wwwIcon.isHttpError) {
					Debug.Log (wwwIcon.error);
				} else {
					result.iconImage = ((DownloadHandlerTexture) wwwIcon.downloadHandler).texture;
				}
			}

			if (result.portraitStaticImageUrl != null && !result.portraitStaticImageUrl.Equals ("")) {
				UnityWebRequest wwwPortrait = UnityWebRequestTexture.GetTexture (result.portraitStaticImageUrl);
				yield return wwwPortrait.SendWebRequest ();
				if (wwwPortrait.isNetworkError || wwwPortrait.isHttpError) {
					Debug.Log (wwwPortrait.error);
				} else {
					result.portraitBannerImage = ((DownloadHandlerTexture) wwwPortrait.downloadHandler).texture;
				}
			}

			if (result.landscapeStaticImageUrl != null && !result.landscapeStaticImageUrl.Equals ("")) {
				UnityWebRequest wwwLandscape = UnityWebRequestTexture.GetTexture (result.landscapeStaticImageUrl);
				yield return wwwLandscape.SendWebRequest ();
				if (wwwLandscape.isNetworkError || wwwLandscape.isHttpError) {
					Debug.Log (wwwLandscape.error);
				} else {
					result.landscapeBannerImage = ((DownloadHandlerTexture) wwwLandscape.downloadHandler).texture;
				}
			}

			if (nativeBannerResponsePool.ContainsKey (result.zoneId)) {
				nativeBannerResponsePool[result.zoneId] (result);
			}
		}
	}
}