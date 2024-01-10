using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandsScript : MonoBehaviour
{
    [SerializeField] private GameObject _hands;

    public bool IsEmpty = true;

    public string CurrentType; // Tool, Objects

    public GameObject CurrentTool;
    public List<GameObject> CurrentObjects = new List<GameObject>();

    public StateGrabObjects StateMainObject;
    public StateToolObjects StateMainTool;

    private Transform _parentCurrentObjects;

    private void Update()
    {
        if ( CurrentType != "Tool" && CurrentType != "Objects")
        {
            CurrentType = null;
        }
    }

    public void GrabTool(RaycastHit hit, StateToolObjects stateOfTool)
    {
        if (IsEmpty)
        {
            StateMainTool = stateOfTool;
            _parentCurrentObjects = hit.transform.parent;

            IsEmpty = false;
        }
        else if (!IsEmpty)
        {
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        StateMainTool.Grab(_hands, _parentCurrentObjects, hitObject);

        CurrentTool = hitObject;
        CurrentType = "Tool";

        StateMainTool.IsPlayerGrabbing = true;
    }

    public void DropTool(Camera playerCamera)
    {
        if (!IsEmpty)
        {
            StateMainTool.Drop(playerCamera, CurrentTool, _parentCurrentObjects);

            CurrentTool = null;
            CurrentType = null;

            StateMainTool = null;

            IsEmpty = true;
        }
    }

    public void GrabObject(RaycastHit hit, StateGrabObjects stateOfObject)
    {
        if (IsEmpty)
        {
            StateMainObject = stateOfObject;
            _parentCurrentObjects = hit.transform.parent;

            IsEmpty = false;
        }
        else if (!IsEmpty)
        {
            if (StateMainObject.TypeGrabObject != stateOfObject.TypeGrabObject) return;
            if (StateMainObject.HandsMaxAmoutObject <= CurrentObjects.Count) return;
        }

        GameObject hitObject = hit.collider.gameObject;

        stateOfObject.Grab(hitObject, _hands, CurrentObjects);

        CurrentObjects.Add(hitObject);

        CurrentType = "Objects";
    }
    
    public void DropObjects(Camera playerCamera)
    {
        if (!IsEmpty)
        {
            StateMainObject.Drop(playerCamera, CurrentObjects, _parentCurrentObjects);

            CurrentObjects.Clear();
            StateMainObject = null;
            CurrentType = null;

            IsEmpty = true;
        }
    }
}
