using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RaceParticipantBehaviour : MonoBehaviour 
{
  private Transform NextGate;

  public GameObject Arrow;

  private void Start()
  {
    NextGate = RaceManager.Instance.FirstGate;
  }

  private void OnTriggerEnter(Collider collision)
  {
    var gate = collision.GetComponent<GateBehaviour>();

    if (gate != null && gate.NextGate != null)
    {
      NextGate = gate.NextGate.transform;
    }
  }

  private void FixedUpdate()
  {
    if (NextGate != null)
    {
      var angle = Vector3.SignedAngle(NextGate.localPosition, Vector3.right, Vector3.forward);

      Arrow.transform.localEulerAngles = new Vector3(0, 0, -angle);
    }
  }
}
