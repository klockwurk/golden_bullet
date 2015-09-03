/*******************************************************************************/
/*!
\file   Embeds.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  TODO:
    - Prevent weird skewing

  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class Embeds : MonoBehaviour
{
  void OnCollisionEnter(Collision col)
  {
    GetComponent<Rigidbody>().isKinematic = true;
    transform.SetParent(col.collider.gameObject.transform);
  }
}
