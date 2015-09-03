/*******************************************************************************/
/*!
\file   OnDeathEvent.cs
\author Khan Sweetman
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
 TODO:
  - Fragments
  - Disappears
  - 
  
*/
/*******************************************************************************/  

using UnityEngine;
using UnityEditor;
using System.Collections;

public class OnDeathEffects : MonoBehaviour
{
  [SerializeField] public bool Active = true;
  [SerializeField] public bool Explodes = false;
  [SerializeField] public GameObject Explosion;
  [SerializeField] public bool Destroys = false;

  void OnDeath()
  {
    if (Active == false)
      return;
    Active = false;

    // Explosion?
    if (Explodes)
      Instantiate(Explosion, transform.position, Quaternion.identity);

    // Destroys?
    if (Destroys)
      Destroy(gameObject);
  }
}

[CustomEditor(typeof(OnDeathEffects))]
public class OnDeathEffectsEditor : Editor
{
  OnDeathEffects obj;
  override public void OnInspectorGUI()
  {
    // Acript
    obj = EditorGUILayout.ObjectField("Script", obj, typeof(OnDeathEffects), false) as OnDeathEffects;

    // Grab script
    var script = target as OnDeathEffects;
    
    // Only show other options if Active
    script.Active = GUILayout.Toggle(script.Active, "Active");
    if (script.Active == false) 
      return;
    
    // Explosion option
    script.Explodes = EditorGUILayout.Toggle("Explodes", script.Explodes);

    // Show explosion archetype property
    if(script.Explodes)
      script.Explosion = (GameObject) EditorGUILayout.ObjectField("Explosion", script.Explosion, typeof(GameObject), false);

    // Destroys option
    script.Destroys = EditorGUILayout.Toggle("Destroys", script.Destroys);
  }
}
