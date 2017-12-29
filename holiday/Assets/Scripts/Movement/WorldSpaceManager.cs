using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceManager : MonoBehaviour
{
  #region Singleton

  public static WorldSpaceManager Instance;

  private void Awake()
  {
    if (WorldSpaceManager.Instance == null)
    {
      WorldSpaceManager.Instance = this;
    }
    else
    {
      DestroyImmediate(this);
    }
  }

  #endregion

  public float ScalingFactor;

  public MeshRenderer SeaMaterial;

  public Vector2 TotalDisplacement { get; private set; }

  public void NotifyPlayerMovement(Vector2 movement)
  {
    TotalDisplacement -= movement;
  }

  private void FixedUpdate()
  {
    SeaMaterial.material.mainTextureOffset = TotalDisplacement * -1;
  }
}
