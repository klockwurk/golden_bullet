/*******************************************************************************/
/*!
\file   MoveTo.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  
*/
/*******************************************************************************/

using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour
{
  public Transform Goal;

  void Start()
  {
    NavMeshAgent agent = GetComponent<NavMeshAgent>();
    agent.destination = Goal.position;
  }
}