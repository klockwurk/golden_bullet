/*******************************************************************************/
/*!
\file   RunnerAI.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class RunnerAI : AIBase
{
	// Use this for initialization
	void Start ()
  {
    // Grab other components
    Agent = GetComponent<NavMeshAgent>();

    // Start running towards first destination
    Agent.destination = Nodes[0].transform.position;
	}

  //////////////////////////////////////              //////////////////////////////////////
  //////////////////////////////////// Event Functions /////////////////////////////////////
  //////////////////////////////////////              //////////////////////////////////////
  void OnTriggerEnter(Collider col)
  {
    // Check if we're colliding with our target
    if (col.gameObject != Nodes[CurrNodeNumber])
      return;
    
    // Go to next target
    FindNextNode();
  }

  ///////////////////////////////////////              //////////////////////////////////////
  //////////////////////////////////// Helper Functions /////////////////////////////////////
  ///////////////////////////////////////              //////////////////////////////////////

  // Redirects the AI towards next target. Eventually runs in a circle
  override public void FindNextNode()
  {
    // Start heading towards the next target
    ++CurrNodeNumber;
    if (CurrNodeNumber == Nodes.Length)
      CurrNodeNumber = 0;
    Agent.destination = Nodes[CurrNodeNumber].transform.position;
  }
}
