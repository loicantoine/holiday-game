using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class RaceParticipantBehaviour : MonoBehaviour 
{
  private Transform m_NextGate;

  private Image m_ArrowImage;

  private Vector3 m_VectorBuffer1;

  public GameObject Arrow;

  public float ArrowRadius;

  public float MaxDistance;

  private void Start()
  {
    m_ArrowImage = Arrow.GetComponent<Image>();
  }

  public void SetNextGateTransform(Transform nextGateTransform)
  {
    m_NextGate = nextGateTransform;
  }

  private void FixedUpdate()
  {
    if (m_NextGate != null)
    {
      var angle = -Vector3.SignedAngle(m_NextGate.localPosition, Vector3.right, Vector3.forward);
      m_VectorBuffer1.Set(0, 0, angle);
      Arrow.transform.localEulerAngles = m_VectorBuffer1;

      var dist = m_NextGate.localPosition.magnitude;
      
      m_VectorBuffer1.Set(Mathf.Cos(Mathf.Deg2Rad * angle) * ArrowRadius, Mathf.Sin(Mathf.Deg2Rad * angle) * ArrowRadius, 0);

      Arrow.transform.localPosition = m_VectorBuffer1;

      m_ArrowImage.color = Color.Lerp(Color.red, Color.white, Mathf.Min(dist / MaxDistance, 1));
    }
  }
}
