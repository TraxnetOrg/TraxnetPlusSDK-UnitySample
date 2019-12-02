
Adding TraxnetPlus to your Unity Project
----

#### Integrating TraxnetPlus plugin, you must use Gradle build system for Unity.

### Import TraxnetPlus SDK

Version 1.0.0.0

First of all, download [TraxnetPlus Unity Package](https://github.com/TraxnetOrg/TraxnetPlusSDK-UnitySample/releases/download/v1.0.0.0/traxnetplus-v1.0.0.0.unitypackage) and add this to your project.

In `Player Settings` of your project, select `Custom Gradle Template` from `Publishing Settings`.
Add the following lines to dependencies of `mainTemplate.gradle` file in `Assets/Plugins/Android` directory of your project.

```gradle
dependencies {
  implementation fileTree(dir: 'libs', include: ['*.jar'])

  implementation fileTree(dir: 'libs', include: ['*.aar'])

  implementation 'com.google.code.gson:gson:2.8.5'
  implementation 'com.squareup.retrofit2:retrofit:2.5.0'
  implementation 'com.squareup.retrofit2:converter-gson:2.5.0'
  implementation 'com.squareup.okhttp3:logging-interceptor:3.12.1'
  implementation 'ee.traxnet.sdk:traxnet-sdk-android:1.2.1'

  implementation 'com.unity3d.ads:unity-ads:3.0.0'
  implementation 'com.google.android.gms:play-services-ads:17.2.1'
  implementation 'com.google.android.gms:play-services-basement:16.2.0'
  implementation 'com.google.android.gms:play-services-ads-identifier:16.0.0'
  implementation 'com.google.android.gms:play-services-location:16.0.0'

  implementation 'com.facebook.android:audience-network-sdk:5.3.0'
}
```

Add the following lines to `allprojects` section of the `mainTemplate.gradle` file.


```
allprojects {
    repositories {
        google()
        jcenter()
        flatDir {
            dirs 'libs'
        }

        maven {
            url 'https://dl.bintray.com/traxnet/maven'
        }
    }
}
```

Add the following lines to `android` section of the `mainTemplate.gradle` file.


```gradle
android {
  compileOptions {
    sourceCompatibility JavaVersion.VERSION_1_8
    targetCompatibility JavaVersion.VERSION_1_8
  }
}
```

### Proguard Configuration

Get `proguard.properties` file from [this link](https://github.com/TraxnetOrg/TraxnetPlusSDK-AndroidSample/blob/master/app/proguard-rules.pro) and add it to proguard properties of your app.

### Initialize TraxnetPlus SDK

Get your app-key from [Traxnet Dashboard](https://dashboard.tracxnet.com/) and Initialize the SDK in a script when app starts.

```cs
void Start () {
  TraxnetPlus.initialize (TRAXNET_KEY);
}
```

Where `TRAXNET_KEY` is the app-key copied from your traxnet dashboard.

### Implementing Rewarded Video Ads

Feest of all, you must create a new rewarded video ad-zone in your application dashboard and use the generated `zoneId` to show rewarded video ads.

Use the following code to request a new rewarde video ad using the TraxnetPlus SDK:

```cs
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
```

When `response` action is called, the ad is ready to be shown. You can start showing the video using the `showAd` method and the `zoneId` from you dashboard:

```cs
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
```

### Implementing Interstitial Ads

To implement interstitial ads in your application, follow the procedure describe in implementing rewarded ads but use `TraxnetPlus.requestInterstitial` method instead of `requestRewardedVideo` method.
The `zoneId` used in this method must have interstitial type in your dashboard.


### Implementing Standard Banner Ads

To show a standard banner, use the `showBannerAd` method from `TraxnetPlus` class as shown in the following code block.

```cs
TraxnetPlus.showBannerAd (ZONE_ID, BANNER_TYPE, VERTICAL_GRAVITY, HORIZONTAL_GRAVITY,
  (string zoneId) => {
    Debug.Log ("on response " + zoneId);
  },
  (TraxnetError error) => {
    Debug.Log ("Error " + error.message);
  });
```

`BANNER_TYPE` parameter indicates the size of banner and can have any of the values given in the following table.


|     keyword    |   size  |
|:--------------:|:-------:|
|  BANNER_320x50 |  320x50 |
| BANNER_320x100 | 320x100 |
| BANNER_250x250 | 250x250 |
| BANNER_300x250 | 300x250 |
|  BANNER_468x60 |  468x60 |
|  BANNER_728x90 |  728x90 |


`VERTICAL_GrAVITY` and `HORIZONTAL_GRAVITY` indicate the vertical and horizontal position of banner on the screen. You can use `Gravity.TOP`, `Gravity.BOTTOM` or `Gravity.CENTER` for vertical gravity and `Gravity.LEFT`, `Gravity.RIGHT` or `Gravity.CENTER` for horizontal gravity.

For example, if you want to show a banner at the bottom of the screen with 320x50 size, you can use the following code:

```cs
TraxnetPlus.showBannerAd (ZONE_ID, BannerType.BANNER_320x50, Gravity.BOTTOM, Gravity.CENTER,
  (string zoneId) => {
    Debug.Log ("on response " + zoneId);
  },
  (TraxnetError error) => {
    Debug.Log ("Error " + error.message);
  });
```

To hide the banner, use `hideBanner` function:

```cs
TraxnetPlus.hideBanner ();
```

### Implementing Native Banner Ads

You need to create a native banner ad-zone in your dashboard to use the generated `zoneId` for showing native banner ads.

To request a native banner ad, use the following code sample:

```cs
public void Request () {
  TraxnetPlus.requestNativeBanner (this, ZONE_ID,
    (TraxnetNativeBannerAd result) => {
      Debug.Log ("on response");
      //show ad
    },
    (TraxnetError error) => {
      Debug.Log ("Error " + error.message);
    }
  );
}
```

The result delivered to `onResponse` action includes the contents and creatives of the advertisement which can be used to show the ad to the user.
`TraxnetNativeBannerAd` class has a few functions to get this content. These functions are introduced in the following table.

|           function          |      usage      |
|:---------------------------:|:---------------:|
|         getTitle  ()        |     title       |
|      getDescription  ()     |   description   |
|         getIcon  ()         |      icon       |
| getLandscapeBannerImage  () | landscape image |
|  getPortraitBannerImage  () |  portrait image |
|     getCallToAction  (),    | call to action  |

To open the advertisement when user clicks on the call to action button, use `clicked` function from `TraxnetNativeBannerAd` instance.

```cs
nativeAd.clicked ();
```

A sample project is available on this github repository.
