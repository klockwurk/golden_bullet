/*******************************************************************************/
/*!
\file   PressEscape.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class PressEscape : MonoBehaviour
{
	// Use this for initialization
	void Start ()
  {
    
	}
	
	// Update is called once per frame
	void Update ()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit(); // Play mode

#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
	}
}
