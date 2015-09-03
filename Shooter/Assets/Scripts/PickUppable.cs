/*******************************************************************************/
/*!
\file   PickUppable.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

// Struct gets sent as part of a message
public struct PickupPackage
{
  public GameObject Body;  // GameObject that contains a RigidBody. Should be parent of PickUppable
  public GameObject Brain; // Owner of this script. 
  public string MessageText;
  public bool AttachesImmediately;
  public Sides WhoCanPickUp;

  // Constructors
  public PickupPackage(GameObject body, GameObject brain, string text, bool attachNow, Sides who)
  {
    Body = body;
    Brain = brain;
    MessageText = text;
    AttachesImmediately = attachNow;
    WhoCanPickUp = who;
  }
}

public class PickUppable : MonoBehaviour
{
  [SerializeField] public GameObject Body;
  [SerializeField] public string MessageText = "Pickup";
  [SerializeField] public bool AttachesImmediately = false;
  [SerializeField] public Sides WhoCanPickUp = Sides.Any;

  // Flags
  public bool PickedUp = false;

  void Start()
  {
    if (Body == null)
      Body = gameObject;
    if (Body.GetComponent<Rigidbody>() == null)
      Debug.LogError("Rigidbody on PickUppable not set", transform);
  }

  // Tell the object that we're a pickup
  void OnTriggerEnter(Collider col)
  {
    var package = new PickupPackage(Body, gameObject, MessageText, AttachesImmediately, WhoCanPickUp);
    col.gameObject.SendMessage("OnPickup", package, SendMessageOptions.DontRequireReceiver);
  }

  // Tell the object that we're no longer available
  void OnTriggerExit(Collider col)
  {
    col.gameObject.SendMessage("OnPickupExit", SendMessageOptions.DontRequireReceiver);
  }

  // Gets called when something tries to pick us up
  void OnPickingUp()
  {
    if (PickedUp)
      return;
    PickedUp = true;
    print("Picked up");
  }

  // Called when dropped
  void OnDropping()
  {
    if (!PickedUp)
      return;
    PickedUp = false;
    print("Dropped");
  }
}
