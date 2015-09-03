/*******************************************************************************/
/*!
\file   WanderAI.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
  
*/
/*******************************************************************************/

using UnityEngine;
using System.Collections;

public class WanderAI : AIBase
{
  // Use this for initialization
  void Start()
  {
    // Get other components
    Agent = GetComponent<NavMeshAgent>();

    // Start by walking towards a random point
    FindNextNode();
  }

  //////////////////////////////////////              //////////////////////////////////////
  //////////////////////////////////// Event Functions /////////////////////////////////////
  //////////////////////////////////////              //////////////////////////////////////
  void OnTriggerEnter(Collider col)
  {
    // Check if we're colliding with our target
    if (col.gameObject != Nodes[CurrNodeNumber])
      return;

    // Start heading towards the next target
    FindNextNode();
  }

  ///////////////////////////////////////              //////////////////////////////////////
  //////////////////////////////////// Helper Functions /////////////////////////////////////
  ///////////////////////////////////////              //////////////////////////////////////
  public override void FindNextNode()
  {
    // Get next target
    int nextNumber = -1;
    do
      nextNumber = Random.Range(0, Nodes.Length);
    while (nextNumber == CurrNodeNumber);
    CurrNodeNumber = nextNumber;

    // Start heading towards the next target
    Agent.destination = Nodes[CurrNodeNumber].transform.position;
  }
}
