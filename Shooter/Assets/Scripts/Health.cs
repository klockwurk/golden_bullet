/*******************************************************************************/
/*!
\file   Health.cs
\author Khan Sweetman
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
  [SerializeField] public float MaxHealth = 3.0f;
  float CurrHealth = 0.0f;

  // Flags
  [SerializeField] bool Invulnerable = false;

	// Use this for initialization
	void Start ()
  {
    // Set up starting health
    CurrHealth = MaxHealth;
	}

  /////////////////////////////////////                 /////////////////////////////////////
  /////////////////////////////////// Per Frame Functions ///////////////////////////////////
  /////////////////////////////////////                 /////////////////////////////////////

  // Gets called whenever damage is dealt
  void OnDamage(float damage)
  {
    // Skip if invulerable
    if (Invulnerable)
      return;

    // Subtract damage
    CurrHealth -= damage;

    // If we just died
    if (CurrHealth <= 0.0f)
    {
      gameObject.SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
    }
  }
}
