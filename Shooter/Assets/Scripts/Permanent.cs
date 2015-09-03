/*******************************************************************************/
/*!
\file   Permanent.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class Permanent : MonoBehaviour
{
	// Use this for initialization
	void Awake ()
  {
    DontDestroyOnLoad(transform.gameObject);
	}
}
