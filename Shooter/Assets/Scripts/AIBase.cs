/*******************************************************************************/
/*!
\file   AIBase.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

abstract public class AIBase : MonoBehaviour
{
  // Other components
  protected NavMeshAgent Agent;

  // Navigation
  [SerializeField] public GameObject[] Nodes;
  protected int CurrNodeNumber = 0;

  ///////////////////////////////////////              //////////////////////////////////////
  //////////////////////////////////// Helper Functions /////////////////////////////////////
  ///////////////////////////////////////              //////////////////////////////////////

  // Redirects the AI.
  abstract public void FindNextNode();
}
