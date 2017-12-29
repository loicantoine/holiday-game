using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "BoostParameters", order = 1)]
public class BoostParameters : ScriptableObject
{
  public AnimationCurve BoostCurve;

  public float SlowdownMaxDuration;

  public float BoostDuration;

  public float BoostCoolDown;
}
