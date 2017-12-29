using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour 
{
  public string RaceSceneName;


  public void LoadScene()
  {
    SceneManager.LoadScene(RaceSceneName);
  }
}
