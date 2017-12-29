using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GateBehaviour : MonoBehaviour 
{
  public int GateId;

 private void OnTriggerEnter(Collider collision)
  {
    RaceManager.Instance.NotifyGateCrossed(GateId);
  }
}
