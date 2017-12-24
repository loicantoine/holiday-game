﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
  public Vector2 m_CurrentForwardDirection;

  public float m_CurrentOrientation;

  public Vector2 m_VectorBackField;
 
  public MeshRenderer SeaMaterial;

  public float AngularSpeed;

  public float LinearPower;

  public float MaxSpeed;

  [Range(0,1)]
  public float Drag = 1;

  // Update is called once per frame
  void FixedUpdate ()
  {
    InputManagement();

    PhysicsManagement();
	}

  private void PhysicsManagement()
  {

    if (m_CurrentForwardDirection.magnitude > 0.1)
    {
      SeaMaterial.material.mainTextureOffset = Vector2.Lerp(SeaMaterial.material.mainTextureOffset, SeaMaterial.material.mainTextureOffset + m_CurrentForwardDirection, Time.fixedDeltaTime);

      WorldSpaceManager.Instance.NotifyPlayerMovement(m_CurrentForwardDirection * Time.fixedDeltaTime);

      //var newAngle = Vector2.SignedAngle(Vector2.up, m_CurrentForwardDirection);

      //var currentAngle = transform.localEulerAngles.z;

      //Debug.Log("formule = currentangle = " + currentAngle + "  new angle : " + newAngle);

      //transform.Rotate(0, 0, (currentAngle - newAngle) / 100);

	    m_CurrentForwardDirection *= Drag;
    }
  }

  private void InputManagement()
  {
    if (Input.GetKey(KeyCode.UpArrow))
    {
      OnLinearMovementPressed(1);
    }
    if (Input.GetKey(KeyCode.DownArrow))
    {
      OnLinearMovementPressed(-1);
    }
    if (Input.GetKey(KeyCode.LeftArrow))
    {
      OnRotationPressed(1);
    }
    if (Input.GetKey(KeyCode.RightArrow))
    {
      OnRotationPressed(-1);
    }
  }

  private void OnLinearMovementPressed(int direction)
  {
    m_VectorBackField.Set(-LinearPower * Mathf.Sin(Mathf.Deg2Rad * m_CurrentOrientation), LinearPower * Mathf.Cos(Mathf.Deg2Rad * m_CurrentOrientation));

    m_CurrentForwardDirection = m_CurrentForwardDirection + m_VectorBackField * direction * Time.fixedDeltaTime;

    if (m_CurrentForwardDirection.magnitude > MaxSpeed)
    {
      m_CurrentForwardDirection.Normalize();
      m_CurrentForwardDirection *= MaxSpeed;
    }
  }

  private void OnRotationPressed(int direction)
  {
    var currentRotation = transform.localEulerAngles;
    m_CurrentOrientation = Mathf.Lerp(currentRotation.z, currentRotation.z + (direction * AngularSpeed), Time.fixedDeltaTime);
    var newRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, m_CurrentOrientation);
    transform.localRotation = newRotation;
  }
}
