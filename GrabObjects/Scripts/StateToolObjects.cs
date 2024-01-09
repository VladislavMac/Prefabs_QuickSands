using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateToolObjects : MonoBehaviour
{
    public Vector3 ToolPosition = new Vector3(0, 0, 0);
    public Vector3 ToolRotation = new Vector3(0, 0, 0);
    public Vector3 ToolScale    = new Vector3(0, 0, 0);

    public bool IsPlayerGrabbing = false;

    public void Grab(GameObject playerHands, Transform parentCurrentObjects, GameObject hitObject)
    {
        hitObject.layer = LayerMask.NameToLayer("Hands");

        if (!hitObject.TryGetComponent(out Rigidbody component))
        {
            hitObject.AddComponent<Rigidbody>();
        }

        hitObject.GetComponent<Rigidbody>().isKinematic = true;
        hitObject.GetComponent<BoxCollider>().enabled = false;

        hitObject.transform.parent = playerHands.transform;
        hitObject.transform.localPosition = ToolPosition;
        hitObject.transform.localEulerAngles = ToolRotation;
    }
    public void Drop(Camera playerCamera, GameObject handCurrentTool, Transform parentCurrentObjects)
    {
        handCurrentTool.layer = LayerMask.NameToLayer("Default");
        handCurrentTool.transform.parent = parentCurrentObjects;

        handCurrentTool.GetComponent<Rigidbody>().isKinematic = false;
        handCurrentTool.GetComponent<BoxCollider>().enabled = true;

        handCurrentTool.GetComponent<StateToolObjects>().IsPlayerGrabbing = false;
        handCurrentTool.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 10);

    }

    public void Use(Camera playerCamera, float interactRange, GameObject uiInteractiveCursor)
    {
        if (TryGetComponent(out UseScript script))
        {
            script.ScriptRun(playerCamera, interactRange, uiInteractiveCursor);
        }
        else
        {
            return;
        }
    }
}
