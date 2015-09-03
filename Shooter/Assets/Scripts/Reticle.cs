/*******************************************************************************/
/*!
\file   Reticle.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  Creates an aiming reticle at the center of the viewport. Changes color
  based on what you're looking at.
  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour
{
  [SerializeField] public Texture2D ReticleTexture;
  [SerializeField] public Sides OwnerFaction;
  [SerializeField] Vector2 Size = new Vector2(50.0f,50.0f);
  [SerializeField] Camera OwnerCamera;
  Rect ReticleRect;

  // Color tracking
  Vector4 currColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
  [SerializeField] public Vector4 neutralColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);
  [SerializeField] public Vector4 allyColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
  [SerializeField] public Vector4 enemyColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);

	// Use this for initialization
	void Start ()
  {
    ReticleRect = new Rect((OwnerCamera.pixelWidth - Size.x) * 0.5f + OwnerCamera.pixelRect.x, (OwnerCamera.pixelHeight - Size.y) * 0.5f + OwnerCamera.pixelRect.y, Size.x, Size.y);
	}

  void Update()
  {
    Ray ray = new Ray(transform.position + transform.forward, transform.forward);
    RaycastHit info;

    // Check to see what we're looking at
    if (Physics.Raycast(ray, out info, 300.0f))
    {
      if (info.collider.gameObject.GetComponent<Faction>() == null)
        currColor = neutralColor;
      else
      {
        Sides hitSide = info.collider.gameObject.GetComponent<Faction>().Side;
        if (hitSide == OwnerFaction)
          currColor = allyColor;
        else if (hitSide != Sides.Any)
          currColor = enemyColor;
      }
    }
    else // Didn't hit anything, set to neutral
      currColor = neutralColor;

    //print(currColor);
  }

  // Called for rendering/handling GUI events
  // Potentially called multiple times per frame
  void OnGUI()
  {
    // Set color -> draw -> reset color
    Vector4 color = GUI.color;
    GUI.color = currColor;
    GUI.DrawTexture(ReticleRect, ReticleTexture);
    GUI.color = color;
  }
}
