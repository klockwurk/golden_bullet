/*******************************************************************************/
/*!
\file   OrientTowardsVelocity.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class OrientTowardsVelocity : MonoBehaviour
{
	void FixedUpdate ()
  {
    Vector3 vel = GetComponent<Rigidbody>().velocity;

    // Guard against 0 length vectors
    if(vel.sqrMagnitude > 0.01)
      transform.forward = vel.normalized;
	}
}
