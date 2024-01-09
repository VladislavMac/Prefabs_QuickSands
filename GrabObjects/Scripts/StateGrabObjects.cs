using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateGrabObjects : MonoBehaviour
{
    public string TypeGrabObject;
    public string LayerGrabObject;

    public int    HandsMaxAmoutObject;

    public bool   IsOnlyGrabObject;
    public bool   IsPlayerGrabbing = false;

    public void Grab(GameObject hitObject, GameObject playerHands, List<GameObject> handCurrentObjects)
    {
        hitObject.layer = LayerMask.NameToLayer("Hands");

        if (!hitObject.TryGetComponent(out Rigidbody component))
        {
            hitObject.AddComponent<Rigidbody>();
        }

        hitObject.GetComponent<Rigidbody>().isKinematic = true;
        hitObject.GetComponent<MeshCollider>().enabled = false;

        hitObject.transform.parent = playerHands.transform;
        hitObject.transform.localPosition = Vector3.zero;

        IsPlayerGrabbing = true;
    }

    public void Drop(Camera playerCamera, List<GameObject> handCurrentObjects, Transform parentCurrentObjects)
    {
        for (int i = 0; i < handCurrentObjects.Count; i++)
        {
            handCurrentObjects[i].layer = LayerMask.NameToLayer("Default");
            handCurrentObjects[i].transform.parent = parentCurrentObjects;

            handCurrentObjects[i].GetComponent<Rigidbody>().isKinematic = false;
            handCurrentObjects[i].GetComponent<MeshCollider>().enabled = true;

            handCurrentObjects[i].GetComponent<StateGrabObjects>().IsPlayerGrabbing = false;
            handCurrentObjects[i].GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 20f);
        }
    }
}
