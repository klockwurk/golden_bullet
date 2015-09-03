using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
public class PlayerController2 : MonoBehaviour
{
  // Other components
  CharacterController Controller;
  [SerializeField] public int ControllerNumber;

  // Game Objects
  GameObject Camera;
  GameObject Gun;

  // First person camera!
  [SerializeField] public ControllerTypes ControllerType = ControllerTypes.Keyboard;
  [SerializeField] public float X_Sensitivity = 3.0f;
  [SerializeField] public float Y_Sensitivity = 3.0f;

  // Movement
  [SerializeField] public float MoveSpeed = 5.0f;
  [SerializeField] public float SprintSpeed = 8.0f;
  float CurrMoveSpeed;

  // Jumping
  [SerializeField] public float   JumpSpeed  = 20.0f;
  private Vector3 MoveDir = Vector3.zero;

  // Grounding
  [SerializeField] public float Gravity = 20.0f;
  [SerializeField] public float StickToGroundForce = 5.0f;

  // Actions
  //[SerializeField] public string ActionKey = "E";

  // 
  private Vector2 InputVec; //
  private CollisionFlags ColFlags;
  private bool PreviouslyGrounded;
  private bool JumpPressed;
  private bool m_Jumping;

	// Use this for initialization
	void Start ()
  {
    // Grab components
    Controller = gameObject.GetComponent<CharacterController>();

    // Grab objects
    Camera = transform.FindChild("Shaker").transform.FindChild("Camera").gameObject;
    Gun    = Camera.transform.FindChild("GunShaker").FindChild("Gun").gameObject;

    // Lock mouse cursor
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
	}

  /////////////////////////////////////                 /////////////////////////////////////
  /////////////////////////////////// Per Frame Functions ///////////////////////////////////
  /////////////////////////////////////                 /////////////////////////////////////
  
  // Copy-paste from FirstPersonCamera.cs
  // Used to look around
  void CameraUpdate() 
  {
    var camTrans = Camera.GetComponent<Transform>();

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
      xRotation = camTrans.localEulerAngles.x + Input.GetAxisRaw("LookY" + ControllerNumber) * X_Sensitivity * -1.0f;
      yRotation = transform.localEulerAngles.y + Input.GetAxisRaw("LookX" + ControllerNumber) * Y_Sensitivity;
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

  void FixedUpdate()
  {
    // Check input
    float speed;
    CheckInput(out speed);

    // Temporary movement vector
    Vector3 move = transform.TransformDirection(InputVec.x, 0.0f, InputVec.y);
    
    // Sweep the player
    RaycastHit hitInfo;
    Physics.SphereCast(transform.position, Controller.radius, Vector3.down, out hitInfo, Controller.height * 0.5f);
    move = Vector3.ProjectOnPlane(move, hitInfo.normal).normalized * speed;

    // Feed temporary movement vector into member vector
    MoveDir.x = move.x;
    MoveDir.z = move.z;

    // Handle y direction
    if (Controller.isGrounded)
    {
      // Slide along the ground
      MoveDir.y = -StickToGroundForce;

      // Jumping
      if (JumpPressed)
      {
        MoveDir.y = JumpSpeed;
        JumpPressed = false;
      }
    }
    else
    {
      // Apply gravity when the player is in the air
      MoveDir.y += Physics.gravity.y * Time.fixedDeltaTime;
    }

    // Apply movement
    Controller.Move(MoveDir * Time.fixedDeltaTime);
  }

	void Update ()
  {
    // Look around
    CameraUpdate();

    // Check jump input. Needs to be done in Update()
    JumpPressed = Input.GetButton("Jump");

    // Shoot
    // Keyboard & mouse
    if (ControllerType == ControllerTypes.Keyboard)
    {
      if (Input.GetButton("Fire1"))
        Gun.GetComponent<Gun>().Shoot();
    }
    // Gamepad
    else
    {
      if (Input.GetAxis("Trigger" + ControllerNumber) > 0.3)
        Gun.GetComponent<Gun>().Shoot();
    }
	}

  /////////////////////////////////////              /////////////////////////////////////
  /////////////////////////////////// Called Functions ///////////////////////////////////
  /////////////////////////////////////              /////////////////////////////////////
  private void CheckInput(out float speed)
  {
    // Sideways/forward movement this frame
    float horizontal = 0.0f;
    float vertical = 0.0f;

    // Read input
    if (ControllerType == ControllerTypes.Keyboard)
    {
      horizontal = Input.GetAxisRaw("Horizontal");
      vertical = Input.GetAxisRaw("Vertical");
    }
    // Gamepad
    else
    {
      horizontal = Input.GetAxisRaw("Horizontal" + ControllerNumber);
      vertical = Input.GetAxisRaw("Vertical" + ControllerNumber);
    }

    // set the desired speed to be walking or running
    speed = Input.GetButton("Fire2") ? SprintSpeed : MoveSpeed;
    InputVec = new Vector2(horizontal, vertical);

    // Normalize InputVec if it's too big.
    // Allow it to be smaller for walking and stuff via joystick
    if (InputVec.sqrMagnitude > 1.0f)
      InputVec.Normalize();
  }
}
