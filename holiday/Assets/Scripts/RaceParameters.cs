using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "RaceParameters", order = 1)]
public class RaceParameters : ScriptableObject
{
  public int Seed;

  public float MinDistance;

  public float MaxDistance;

  public float MaxAngle;

  public int NumberOfLap;
}
