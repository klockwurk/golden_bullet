/*******************************************************************************/
/*!
\file   EditorScript.cs
\author Khan Sweetman
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
*/
/*******************************************************************************/  

using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorScript
{
  // Press Shift + p to run the game
  [MenuItem("File/Run Game #p")]
  private static void RunGame()
  {
    EditorApplication.SaveCurrentSceneIfUserWantsTo();
    EditorApplication.isPlaying = true;
  }
}
