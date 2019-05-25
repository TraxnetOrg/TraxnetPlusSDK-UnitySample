using System.Collections;
using System.Collections.Generic;
using TraxnetPlusSDK;
using UnityEngine;

public class StandardBannerScene : MonoBehaviour {

  private readonly string ZONE_ID = "5cd7e2cac94ec10001086e61";

  public void Show () {
    TraxnetPlus.showBannerAd (ZONE_ID, BannerType.BANNER_320x50, Gravity.BOTTOM, Gravity.CENTER,

			(string zoneId) => {
				Debug.Log ("on response " + zoneId);
			},
			(TraxnetError error) => {
				Debug.Log ("Error " + error.message);
			});
  }

  public void hide () {
    TraxnetPlus.hideBanner ();
  }
}