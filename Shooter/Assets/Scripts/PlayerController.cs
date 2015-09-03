using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
  // Other components
  CharacterController Controller;

  // Game Objects
  GameObject Camera;
  GameObject Gun;

  // Movement
  [SerializeField] public float MoveSpeed = 5.0f;
  [SerializeField] public float SprintSpeed = 8.0f;
  [SerializeField] public KeyCode SprintButton = KeyCode.LeftShift;
  float CurrMoveSpeed;

  // Jumping
  [SerializeField] public float   JumpSpeed  = 20.0f;
  [SerializeField] public KeyCode JumpButton = KeyCode.Space;
  private Vector3 MoveDirection = Vector3.zero;

  // Grounding
  [SerializeField] public float Gravity = 20.0f;
  [SerializeField] public float StickToGroundForce = 5.0f;

  // Actions
  [SerializeField] public KeyCode ActionKey = KeyCode.E;

	// Use this for initialization
	void Start ()
  {
    // Grab components
    Controller = gameObject.GetComponent<CharacterController>();

    // Grab objects
    Camera = transform.FindChild("Shaker").transform.FindChild("Camera").gameObject;
    Gun   = Camera.transform.FindChild("Gun").gameObject;
	}

  /////////////////////////////////////                 /////////////////////////////////////
  /////////////////////////////////// Per Frame Functions ///////////////////////////////////
  /////////////////////////////////////                 /////////////////////////////////////
  void MovementUpdate()
  {
    // Movement speed
    if (Input.GetKey(SprintButton))
      CurrMoveSpeed = SprintSpeed;
    else
      CurrMoveSpeed = MoveSpeed;

    // Create movement vector
    // Create temporary variable, so we don't overwrite previously stored movement
    Vector3 move = Vector3.zero;
    move.x = Input.GetAxisRaw("Horizontal");
    move.z = Input.GetAxisRaw("Vertical");
    move = transform.TransformDirection(move) * CurrMoveSpeed * Time.deltaTime;
    move.y = MoveDirection.y * Time.deltaTime;

    if (Input.GetKey(JumpButton))
      Jump();

    // Apply movement
    Controller.Move(move);

    // Update gravity
    MoveDirection.y -= Time.deltaTime * Gravity;

    // Stick to the ground
    if (Controller.isGrounded)
    {
      MoveDirection.y = -StickToGroundForce;
    }
  }

  void ActionUpdate()
  {
    if (Input.GetKeyDown(ActionKey))
      Camera.GetComponent<PicksUp>().Attach();
  }

  void FixedUpdate()
  {
    // Jumping
    if (Input.GetKeyDown(JumpButton))
      Jump();

    // Shooting
    if (Input.GetMouseButton(0))
      Gun.GetComponent<Gun>().Shoot();
    
  }

	void Update ()
  {
    MovementUpdate();
    ActionUpdate();
	}

  /////////////////////////////////////              /////////////////////////////////////
  /////////////////////////////////// Called Functions ///////////////////////////////////
  /////////////////////////////////////              /////////////////////////////////////
  void Jump()
  {
    if (Controller.isGrounded == false)
      return;

    MoveDirection.y = JumpSpeed;
  }
}
