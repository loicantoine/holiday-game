using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour 
{
  private Vector3 InitialTransformPosition;

	void Start () 
  {
    InitialTransformPosition = transform.localPosition;
	}
	
	void FixedUpdate () 
  {
    transform.localPosition = new Vector3(InitialTransformPosition.x + WorldSpaceManager.Instance.TotalDisplacement.x * WorldSpaceManager.Instance.ScalingFactor,
                                          InitialTransformPosition.y + WorldSpaceManager.Instance.TotalDisplacement.y * WorldSpaceManager.Instance.ScalingFactor,
                                          InitialTransformPosition.z);
	}

  public void Move(Vector3 movement)
  {
    InitialTransformPosition += movement;
  }
}
