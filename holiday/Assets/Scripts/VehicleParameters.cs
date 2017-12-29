using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "VehicleParameters", order = 1)]
public class VehicleParameters : ScriptableObject
{
  public float AngularSpeed;

  public float LinearPower;

  public float MaxSpeed;
 
  [Range(0,1)]
  public float BoostAngularSpeedFactor;

  [Range(0,1)]
  public float BrakingFactor;
}
