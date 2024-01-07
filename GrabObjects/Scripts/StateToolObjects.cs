using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateToolObjects : MonoBehaviour
{
    public bool IsPlayerGrabbing = false;
    public readonly Vector3 ToolPosition = new Vector3(0.032f, 0.136f, -0.182f);
    public readonly Vector3 ToolRotation = new Vector3(172.014f, 0.9530029f, -38.83801f);

    private StatePalmObject _stateOfPalmObject;

    public void Use(Camera playerCamera, float interactRange, GameObject uiInteractiveCursor )
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out StatePalmObject stateOfObject))
            {
                if (stateOfObject.AmountAxeHit == 0)
                {
                    hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    stateOfObject.IsPalmFell = true;
                }
                
                if (stateOfObject.AmountAxeHit < -stateOfObject.AmountAxeHit)
                {
                    Vector3 PalmPosition = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z);

                    for (int i = 0; i < stateOfObject.AmountWoodLog; i++)
                    {
                        PalmPosition += new Vector3(0, 1.7f, 0);

                        GameObject newWoodObject = Instantiate(stateOfObject.WoodLog);
                        newWoodObject.transform.parent = stateOfObject.ParentWoodLog;
                        newWoodObject.transform.position = PalmPosition;
                        newWoodObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                        newWoodObject.GetComponent<Rigidbody>().isKinematic = false;
                    }

                    Destroy(hit.collider.gameObject);
                }
                
                stateOfObject.AmountAxeHit--;
            }
        }
    }
}
