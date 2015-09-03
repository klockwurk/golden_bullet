/*******************************************************************************/
/*!
\file   Screenshake.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour
{
  [SerializeField] public float ShakeTime = 0.5f;
  [SerializeField] public float Frequency = 12.0f;
  [SerializeField] public float Displacement = 0.5f;

  // Timers
  float Timer = 0.0f;
  float FreqTime = 0.0f;
  float FreqTimer = 0.0f;

  // Direction
  Vector3 DefaultPos = new Vector3();
  Vector3 NextPos = new Vector3();
  Vector3 PrevPos = new Vector3();

	// Use this for initialization
	void Start ()
  {
    // Grab variables
    DefaultPos = transform.localPosition;

    // TODO:
    // - Add an if check to see if we should programatically add the shaker
    // - else (assume we shake the object directly) { ... }
	}
	
  // Wrapper around core of this file. Call whenever you want to shake the camera.
  public void Shake()
  {
    // Update variables before beginning
    FreqTime = 1.0f / Frequency;
    FreqTimer = FreqTime;
    Timer = ShakeTime;
    PrevPos = NextPos = DefaultPos;

    StartCoroutine(ShakeCoroutine());
  }

  IEnumerator ShakeCoroutine()
  {
    // Shake while there is still time left to shake
    while (Timer > 0.0f)
    {
      // Calculate NewPos every time through the loop
      PrevPos = NextPos;
      NextPos = DefaultPos + Random.insideUnitSphere * Displacement;

      // Lerping towards this time interval's destination
      while (FreqTimer > 0.0f)
      {
        FreqTimer -= Time.deltaTime;
        float proportionalTime = (FreqTime - FreqTimer) / FreqTime;
        transform.localPosition = Vector3.Lerp(PrevPos, NextPos, proportionalTime);
        yield return null;
      }

      // Update Timer using the time passed in FreqTimer's loop
      Timer -= FreqTime;
      FreqTimer = FreqTime;
    }

    // Return to center, aka, the original offset from parent position
    // Returns in a timespan of FreqTime
    // Only do this if not being shaken
    FreqTimer = FreqTime;
    PrevPos = NextPos;
    while (transform.localPosition != DefaultPos && Timer < 0.0f)
    {
      FreqTimer -= Time.deltaTime;
      float proportionalTime = 1.0f / ((FreqTime - FreqTimer) / FreqTime);
      transform.localPosition = Vector3.Lerp(PrevPos, DefaultPos, proportionalTime);
      yield return null;
    }
  }
}
