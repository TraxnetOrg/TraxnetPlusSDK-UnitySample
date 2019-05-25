using System.Collections;
using System.Collections.Generic;
using TraxnetPlusSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScene : MonoBehaviour {

  private readonly string TRAXNET_PLUS_KEY = "nnaidshanpgmfsktgrcgogisspprlftaooihpqfjtqeomsmocsejlomgimijeplitjrfhs";

  void Start () {
    TraxnetPlus.initialize (TRAXNET_PLUS_KEY);
  }

  public void changeScenes (string name) {
    SceneManager.LoadScene (name);
  }
}