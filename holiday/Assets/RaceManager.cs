using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  public GameObject GateParent;

  public GameObject GatePrefab;

  public Transform FirstGate;

  public int NumberOfGate;

  public Vector3 CurrentGatePosition;

  public int Seed;

  public float MinDistance;

  public float MaxDistance;

  public float MaxAngle;

  private void Start()
  {
    Random.InitState(Seed);

    var firstGatePosition = new Vector3();

    GateBehaviour previousGate = null;

    for (int i = 0; i < NumberOfGate; i++)
    {
      var angle = Random.value * (MaxAngle * 2) - MaxAngle;

      var distance = Random.value * (MaxDistance - MinDistance) + MinDistance;

      CurrentGatePosition += new Vector3(distance * Mathf.Sin(Mathf.Deg2Rad * angle), distance * Mathf.Cos(Mathf.Deg2Rad * angle), 0);

      var nextGate = Instantiate(GatePrefab, GateParent.transform, false);

      nextGate.transform.localPosition = CurrentGatePosition;

      if (previousGate != null)
      {
        previousGate.NextGate = nextGate;
      }

      previousGate = nextGate.GetComponent<GateBehaviour>();

      if (i == 0)
      {
        firstGatePosition = CurrentGatePosition;
        FirstGate = nextGate.transform;
      }

      Debug.Log("Gate num" + i + " is placed at " + CurrentGatePosition);
    }

    CurrentGatePosition = firstGatePosition;
  }
}
