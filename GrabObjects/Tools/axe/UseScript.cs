using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseScript : MonoBehaviour
{
    public string NameScript = "None";

    private StatePalmObject _stateOfPalmObject;
    public void ScriptRun(Camera playerCamera, float interactRange, GameObject uiInteractiveCursor)
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
