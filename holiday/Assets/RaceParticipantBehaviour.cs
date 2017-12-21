using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class RaceParticipantBehaviour : MonoBehaviour 
{
  private Transform m_NextGate;

  private Image m_ArrowImage;

  public GameObject Arrow;

  public float ArrowRadius;

  public float MaxDistance;

  private void Start()
  {
    m_NextGate = RaceManager.Instance.FirstGate;
    m_ArrowImage = Arrow.GetComponent<Image>();
  }

  private void OnTriggerEnter(Collider collision)
  {
    var gate = collision.GetComponent<GateBehaviour>();

    if (gate != null && gate.NextGate != null)
    {
      m_NextGate = gate.NextGate.transform;
    }
  }

  private void FixedUpdate()
  {
    if (m_NextGate != null)
    {
      var angle = -Vector3.SignedAngle(m_NextGate.localPosition, Vector3.right, Vector3.forward);

      Arrow.transform.localEulerAngles = new Vector3(0, 0, angle);

      Arrow.transform.localPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * ArrowRadius, Mathf.Sin(Mathf.Deg2Rad * angle) * ArrowRadius, 0);

      var dist = m_NextGate.position.magnitude;

      m_ArrowImage.color = Color.Lerp(Color.red, Color.white, Mathf.Min(dist / MaxDistance, 1));
    }
  }
}
