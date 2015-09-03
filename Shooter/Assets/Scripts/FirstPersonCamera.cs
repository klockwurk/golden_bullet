/*******************************************************************************/
/*!
\file   FirstPersonCamera.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  Rotates the camera around the x-axis, and the player around the y
 
  Meant to be set up like:
   -Player (attach script here)
    - Shaker (Used for screenshake. Most valuable of VFX.)
      -Camera
  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public enum ControllerTypes
{
  Keyboard,
  Gamepad
}

public class FirstPersonCamera : MonoBehaviour
{
  // Camera
  GameObject mCamera;

  // First person camera!
  [SerializeField] public ControllerTypes ControllerType = ControllerTypes.Keyboard;
  [SerializeField] public float X_Sensitivity = 3.0f;
  [SerializeField] public float Y_Sensitivity = 3.0f;

	// Use this for initialization
	void Start ()
  {
    // Lock mouse cursor
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // Grab objects
    mCamera = transform.FindChild("Shaker").transform.FindChild("Camera").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
  {
    var camTrans = mCamera.GetComponent<Transform>();

    // Rotation around the x and y axes
    float xRotation = 0.0f;
    float yRotation = 0.0f;

    // Keyboard & mouse
    if (ControllerType == ControllerTypes.Keyboard) 
    {
      xRotation = camTrans.localEulerAngles.x + Input.GetAxisRaw("Mouse Y") * X_Sensitivity * -1.0f;
      yRotation = transform.localEulerAngles.y + Input.GetAxisRaw("Mouse X") * Y_Sensitivity;
    }
    // Gamepad
    else 
    {
      xRotation = camTrans.localEulerAngles.x + Input.GetAxisRaw("LookY1") * X_Sensitivity * -1.0f; print("LookY: " + Input.GetAxisRaw("LookY1"));
      yRotation = transform.localEulerAngles.y + Input.GetAxisRaw("LookX1") * Y_Sensitivity; print("LookX: " + Input.GetAxisRaw("LookX1"));
    }

    // Limit looking down
    if (xRotation > 90.0f
     && xRotation < 180.0f)
      xRotation = 90.0f;

    // Limit looking up
    if (xRotation < 270.0f
     && xRotation > 90.0f)
      xRotation = 270.0f;

    // Set new euler angles
    transform.eulerAngles = new Vector3(0.0f, yRotation, 0.0f);
    camTrans.localEulerAngles = new Vector3(xRotation, 0.0f, 0.0f);

    // Unity editor needs some extra stuff to work
#if UNITY_EDITOR
    Cursor.lockState = CursorLockMode.Locked;
#endif
	}
}
