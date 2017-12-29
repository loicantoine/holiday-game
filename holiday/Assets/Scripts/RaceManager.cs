using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour 
{

  #region Singleton

  public static RaceManager Instance;

  private void Awake()
  {
    if (RaceManager.Instance == null)
    {
      RaceManager.Instance = this;
    }
    else
    {
      DestroyImmediate(this);
    }
  }

  #endregion

  private Dictionary<int, GateBehaviour> m_GateBehaviourDictionary = new Dictionary<int, GateBehaviour>();

  private List<Text> m_TextList;

  private int m_CurrentTargetGate;

  private int m_CurrentLap;

  private double m_InGameClock;

  private double m_CurrentLapStartTime;

  public RaceParticipantBehaviour Player;

  public GameObject WindArrowIndicator;

  public GameObject GateParent;

  public GameObject GatePrefab;

  public Transform FirstGate;

  public Text LapTimePrefab;

  public GameObject LapTimeParent;

  public GameObject EndPanel;

  public Vector3 CurrentGatePosition;

  public RaceParameters RaceParameters;

  public WindManager WindManager;

  public bool IsCourseOngoing { get; private set; }

  private void Start()
  {
    EndPanel.SetActive(false);

    UnityEngine.Random.InitState(RaceParameters.Seed);
    WindManager.Seed = RaceParameters.Seed;

    GateBehaviour previousGate = null;

    for (var i = 0; i < RaceParameters.NumberOfGate; i++)
    {
      var angle = UnityEngine.Random.value * (RaceParameters.MaxAngle * 2) - RaceParameters.MaxAngle;

      var distance = UnityEngine.Random.value * (RaceParameters.MaxDistance - RaceParameters.MinDistance) + RaceParameters.MinDistance;

      CurrentGatePosition += new Vector3(distance * Mathf.Sin(Mathf.Deg2Rad * angle), distance * Mathf.Cos(Mathf.Deg2Rad * angle), 0);

      var nextGate = Instantiate(GatePrefab, GateParent.transform, false);

      nextGate.transform.localPosition = CurrentGatePosition;

      previousGate = nextGate.GetComponent<GateBehaviour>();

      previousGate.GateId = i;

      m_GateBehaviourDictionary.Add(i, previousGate);

      Debug.Log("Gate num" + i + " is placed at " + CurrentGatePosition);
    }

    m_TextList = new List<Text>(RaceParameters.NumberOfLap);

    for (var i = 0; i < RaceParameters.NumberOfLap; i++)
    {
      var go = Instantiate(LapTimePrefab, LapTimeParent.transform);

      var text = go.GetComponent<Text>();

      text.text = "--:--:--";

      m_TextList.Add(text);
    }

    m_CurrentTargetGate = 0;
    Player.SetNextGateTransform(m_GateBehaviourDictionary[0].transform);

    IsCourseOngoing = true;
    ChangeWindCondition();
  }

  private void FixedUpdate()
  {
    if (IsCourseOngoing)
    {
      UpdateClock();
    }
  }

  private void UpdateClock()
  {
    m_InGameClock += Time.fixedDeltaTime;
    m_TextList[m_CurrentLap].text = GetTimeFromDouble(m_InGameClock - m_CurrentLapStartTime);
  }

  public void NotifyGateCrossed(int gateId)
  {
    if (m_CurrentTargetGate == gateId && IsCourseOngoing)
    {
      m_CurrentTargetGate = (m_CurrentTargetGate + 1) % RaceParameters.NumberOfGate;
      Player.SetNextGateTransform(m_GateBehaviourDictionary[m_CurrentTargetGate].transform);
      ChangeWindCondition();

      if (m_CurrentTargetGate == 0)
      {
        m_CurrentLapStartTime = m_InGameClock;
        m_CurrentLap++;
        if (m_CurrentLap >= RaceParameters.NumberOfLap)
        {
          IsCourseOngoing = false;
          EndPanel.SetActive(true);
          Debug.Log("COURSE FINIE !");
        }
      }
    }
  }

  private void ChangeWindCondition()
  {
    var angle = -Vector3.SignedAngle(m_GateBehaviourDictionary[m_CurrentTargetGate].transform.localPosition, Vector3.right, Vector3.forward);

    var minAngle = angle - 30;

    if (minAngle < 0)
    {
      minAngle += 360;
    }

    var maxAngle = angle + 30;

    if (maxAngle > 360)
    {
      maxAngle -= 360;
    }

    WindManager.RandomizeWind(minAngle, maxAngle);

    WindArrowIndicator.transform.localEulerAngles = WindManager.GetArrowRotation(WindArrowIndicator.transform.localEulerAngles);
  }

  private string GetTimeFromDouble(double time)
  {
    var hundredth = (time * 100) % 100;
    var seconds = time % 60;
    var minutes = (time / 60) % 60;

    var hundredthStr = (hundredth < 10 ? "0" : "") + Math.Floor(hundredth);
    var secondsStr = (seconds < 10 ? "0" : "") + Math.Floor(seconds);
    var minutesStr = (minutes < 10 ? "0" : "") + Math.Floor(minutes);

    return minutesStr + ":" + secondsStr + ":" + hundredthStr;
  }

  public void LoadMainMenu()
  {
    SceneManager.LoadScene(0);
  }
}
