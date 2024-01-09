using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsScript : MonoBehaviour
{
    public bool IsEmpty = true;

    public string CurrentType; // Tool, Objects

    public GameObject CurrentTool;
    public List<GameObject> CurrentObjects = new List<GameObject>();

    public StateGrabObjects StateMainObject;
    public StateToolObjects StateMainTool;

    private void Update()
    {
        if ( CurrentType != "Tool" && CurrentType != "Objects")
        {
            CurrentType = null;
        }
    }
}
