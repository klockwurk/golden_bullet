/*******************************************************************************/
/*!
\file   DropoffPoint.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public enum Sides
{
  Red,
  Black,
  Any
}

public class DropoffPoint : MonoBehaviour
{
  [SerializeField] public Sides Side;

  void OnTriggerEnter(Collider col)
  {
    // Check if it is the player
    if (col.gameObject.GetComponent<PlayerController>() != null)
    {
      // Check if the player is holding the flag
      if (col.transform.FindChild("Camera/Flag") == null)
        return;
    }
    // Check if it's the flag itself
    else if (col.gameObject.GetComponent<Flag>() == null)
      return;

    print("Flag captured");
  }
}
