using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "WindManager", order = 1)]
public class WindManager : ScriptableObject 
{
  private Vector3 m_Vector3BackField;
  private float m_WindDirection;
  private float m_WindForce;
  public int m_Seed;
  public bool m_IsRandomShouldBeInit = true;

  public float MinWindForce;
  public float MaxWindForce;

  public float WindDirection { get { return m_WindDirection; } }
  public float WindForce { get { return m_WindForce; } }

  public int Seed { get { return m_Seed; } set {
      if (m_Seed != value)
      {
        m_Seed = value;
        m_IsRandomShouldBeInit = true;
      }
    }}
 
  public void RandomizeWind(float minWindAngle = 0, float maxWindAngle = 360)
  {
    if (m_IsRandomShouldBeInit)
    {
      Random.InitState(Seed);
      m_IsRandomShouldBeInit = false;
    }

    m_WindDirection = Random.Range(minWindAngle, maxWindAngle);
    Debug.Log("Requesting Wind with minWindAngle = " + minWindAngle + " and maxWindAngle = " + maxWindAngle + " returning with wind direction = " + m_WindDirection);
    m_WindForce = Random.Range(MinWindForce, MaxWindForce);
  }

  public Vector3 GetArrowRotation(Vector3 localEulerAngles)
  {
    m_Vector3BackField.Set(localEulerAngles.x, localEulerAngles.y, WindDirection);
    return m_Vector3BackField;
  }
}
