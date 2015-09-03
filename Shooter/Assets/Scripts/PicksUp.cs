/*******************************************************************************/
/*!
\file   PicksUp.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.
  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

public class PicksUp : MonoBehaviour
{
  [SerializeField] public Vector3 Offset = new Vector3(0,0,0);
  [SerializeField] public GameObject MessageObject;

  PickupPackage CurrentPickup;

  void OnPickup(PickupPackage pickup)
  {
    // Check if we can pick it up
    if(pickup.WhoCanPickUp != Sides.Any
    && pickup.WhoCanPickUp != GetComponent<Faction>().Side)
      return;

    // Passed tests, log it as current pickup
    CurrentPickup = pickup;

    // React to parameters of the pickup
    if (pickup.MessageText != "")
      Message(pickup.MessageText);
    if (pickup.AttachesImmediately)
      Attach();
  }

  void OnPickupExit()
  {
    CurrentPickup = new PickupPackage();
  }

  void Message(string msgText)
  {
    //print(msgText);
    // Make text appear in HUD
    // ...
  }

  public void Attach()
  {
    if (CurrentPickup.Body == null)
      return;
    if (CurrentPickup.Brain.GetComponent<PickUppable>().PickedUp == true)
    {
      Detach();
      return;
    }

    CurrentPickup.Body.transform.parent = gameObject.transform;
    CurrentPickup.Body.GetComponent<Rigidbody>().isKinematic = true;
    CurrentPickup.Body.transform.localPosition = Offset;
    CurrentPickup.Body.transform.forward = transform.forward;

    // Tell the pickup that it's ours now
    CurrentPickup.Brain.SendMessage("OnPickingUp", SendMessageOptions.DontRequireReceiver);
  }

  public void Detach()
  {
    CurrentPickup.Body.transform.parent = null;
    CurrentPickup.Body.GetComponent<Rigidbody>().isKinematic = false;

    // Launch pickup
    CurrentPickup.Body.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 10.0f;

    // Alignment
    CurrentPickup.Body.transform.forward = new Vector3(0, 0, 1);

    // Tell the pickup that it's free now
    CurrentPickup.Brain.SendMessage("OnDropping", SendMessageOptions.DontRequireReceiver);

    // Clear reference
    CurrentPickup = new PickupPackage();
  }
}
