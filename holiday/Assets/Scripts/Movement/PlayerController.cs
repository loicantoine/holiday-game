using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private bool m_IsBoostActive;

  private float m_BoostForce;

  private float m_BoostOrientation;

  private Vector2 m_CurrentForwardDirection;

  private float m_CurrentOrientation;
 
  private Vector2 m_Vector2BackField;

  private Vector3 m_Vector3BackField;

  private bool m_IsRotationBoostActive;

  private bool m_IsBraking;

  private float m_PreBrakeForce;

  private bool m_IsBoostReady;

  private bool m_IsBoostInCooldown;

  //public MeshRenderer SeaMaterial;

  public VehicleParameters VehicleParameters;

  public BoostParameters BoostParameters;

  public Transform BoostBarTransform;

  public float CurrentSpeed;

  [Range(0, 1)]
  public float Drag = 1;

  // Update is called once per frame
  void FixedUpdate()
  {
    InputManagement();

    PhysicsManagement();
  }

  private void PhysicsManagement()
  {
    if (m_IsBraking)
    {
      {
        m_CurrentForwardDirection *= VehicleParameters.BrakingFactor;
      }
    }

    /*if (m_IsBoostActive)
    {
      SeaMaterial.material.mainTextureOffset = SeaMaterial.material.mainTextureOffset + m_CurrentForwardDirection;

      WorldSpaceManager.Instance.NotifyPlayerMovement(m_CurrentForwardDirection);

      CurrentSpeed = m_CurrentForwardDirection.magnitude;
    }
    else */
    if (m_CurrentForwardDirection.magnitude > 0.1)
    {
      //SeaMaterial.material.mainTextureOffset = Vector2.Lerp(SeaMaterial.material.mainTextureOffset, SeaMaterial.material.mainTextureOffset + m_CurrentForwardDirection, Time.fixedDeltaTime);

      WorldSpaceManager.Instance.NotifyPlayerMovement(m_CurrentForwardDirection * Time.fixedDeltaTime);

      //var newAngle = Vector2.SignedAngle(Vector2.up, m_CurrentForwardDirection);

      //var currentAngle = transform.localEulerAngles.z;

      //Debug.Log("formule = currentangle = " + currentAngle + "  new angle : " + newAngle);

      //transform.Rotate(0, 0, (currentAngle - newAngle) / 100);

      CurrentSpeed = m_CurrentForwardDirection.magnitude;

      m_CurrentForwardDirection *= Drag;
    }
  }

  private void InputManagement()
  {
    //Debug.Log("Accelerate = " + Input.GetAxis("Accelerate"));
    if (System.Math.Abs(Input.GetAxis("Accelerate")) > 0.1f && !m_IsBoostActive && !m_IsBraking)
    {
      OnLinearMovementPressed((int)Mathf.Sign(Input.GetAxis("Accelerate")));
    }
    if (System.Math.Abs(Input.GetAxis("Rotate")) > 0.1f)
    {
      OnRotationPressed((int)Mathf.Sign(Input.GetAxis("Rotate")));
    }
    /*if (Input.GetButton("Boost") && !m_IsBoostActive)
    {
      StartBoost();
    }*/
    if (Input.GetButtonDown("AngularBoost"))
    {
      m_IsRotationBoostActive = true;
    }
    if (Input.GetButtonUp("AngularBoost"))
    {
      m_IsRotationBoostActive = false;
    }
    if (Input.GetButtonDown("Boost") && !m_IsBoostInCooldown)
    {
      //m_PreBrakeForce = m_CurrentForwardDirection.magnitude;
      m_CurrentForwardDirection.Set(m_CurrentForwardDirection.x * 0.2f, m_CurrentForwardDirection.y * 0.2f);
      //m_Vector3BackField.Set(0, 1, 1);
      //BoostBarTransform.localScale = m_Vector3BackField;
      m_IsBoostActive = true;
      m_IsBoostReady = true;
      m_IsBoostInCooldown = true;
      StartCoroutine(StartingBoostCoroutine());
    }
    if (Input.GetButtonUp("Boost") && m_IsBoostReady)
    {
      //m_IsBoostReady = false;
      //m_CurrentOrientation = transform.localEulerAngles.z;
      //m_CurrentForwardDirection.Set(-m_PreBrakeForce * Mathf.Sin(Mathf.Deg2Rad * m_CurrentOrientation), VehicleParameters.LinearPower * Mathf.Cos(Mathf.Deg2Rad * m_CurrentOrientation));
      StartBoost();
    }
    if (Input.GetButtonDown("Brake"))
    {
      //m_PreBrakeForce = m_CurrentForwardDirection.magnitude;
      m_IsBraking = true;
    }
    if (Input.GetButtonUp("Brake"))
    {
      //m_CurrentOrientation = transform.localEulerAngles.z;
      //m_CurrentForwardDirection.Set(-m_PreBrakeForce * Mathf.Sin(Mathf.Deg2Rad * m_CurrentOrientation), VehicleParameters.LinearPower * Mathf.Cos(Mathf.Deg2Rad * m_CurrentOrientation));
      m_IsBraking = false;
    }
  }

  private IEnumerator StartingBoostCoroutine()
  {
    var timer = 0f;
    while(timer < BoostParameters.SlowdownMaxDuration && m_IsBoostReady)
    {
      m_Vector3BackField.Set((BoostParameters.SlowdownMaxDuration - timer) / BoostParameters.SlowdownMaxDuration, 1, 1);
      BoostBarTransform.localScale = m_Vector3BackField;
      timer += Time.deltaTime;
      yield return null;
    }

    if (m_IsBoostReady)
    {
      StartBoost();
    }
  }

  private void StartBoost()
  {
    m_IsBoostReady = false;
    StartCoroutine(BoostCoroutine());
  }

  private IEnumerator BoostCoroutine()
  {
    m_BoostOrientation = transform.localEulerAngles.z;
    var totalBoostDuration = BoostParameters.BoostDuration + BoostParameters.BoostCoolDown;
    var timer = 0f;
    while (timer < BoostParameters.BoostDuration)
    {
      m_BoostForce = BoostParameters.BoostCurve.Evaluate(timer) * VehicleParameters.MaxSpeed;

      //Debug.Log("m_BoostForce = " + m_BoostForce);
      m_Vector2BackField.Set(-m_BoostForce * Mathf.Sin(Mathf.Deg2Rad * m_BoostOrientation), m_BoostForce * Mathf.Cos(Mathf.Deg2Rad * m_BoostOrientation));

      m_CurrentForwardDirection = m_Vector2BackField;
      timer += Time.deltaTime;
      m_Vector3BackField.Set((timer / totalBoostDuration), 1, 1);
      BoostBarTransform.localScale = m_Vector3BackField;
      yield return null;
    }
    m_CurrentForwardDirection = m_CurrentForwardDirection.normalized * VehicleParameters.MaxSpeed;

    m_IsBoostActive = false;

    while (timer < totalBoostDuration)
    {
      timer += Time.deltaTime;
      m_Vector3BackField.Set((timer / totalBoostDuration), 1, 1);
      BoostBarTransform.localScale = m_Vector3BackField;
      yield return null;
    }

    m_Vector3BackField.Set(1, 1, 1);
    BoostBarTransform.localScale = m_Vector3BackField;
    m_IsBoostInCooldown = false;
  }

  private void OnLinearMovementPressed(int direction)
  {
    m_Vector2BackField.Set(-VehicleParameters.LinearPower * Mathf.Sin(Mathf.Deg2Rad * m_CurrentOrientation), VehicleParameters.LinearPower * Mathf.Cos(Mathf.Deg2Rad * m_CurrentOrientation));

    m_CurrentForwardDirection = m_CurrentForwardDirection + m_Vector2BackField * direction * Time.fixedDeltaTime;

    if (m_CurrentForwardDirection.magnitude > VehicleParameters.MaxSpeed)
    {
      m_CurrentForwardDirection.Normalize();
      m_CurrentForwardDirection *= VehicleParameters.MaxSpeed;
    }
  }

  private void OnRotationPressed(int direction)
  {
    var currentRotation = transform.localEulerAngles;
    var currentAngularSpeed = (direction * VehicleParameters.AngularSpeed) * (m_IsRotationBoostActive ? VehicleParameters.BoostAngularSpeedFactor : 1);
    m_CurrentOrientation = Mathf.Lerp(currentRotation.z, currentRotation.z + currentAngularSpeed, Time.fixedDeltaTime);
    var newRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, m_CurrentOrientation);
    transform.localRotation = newRotation;
  }
}
