using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SlotsScript : MonoBehaviour
{
    [SerializeField] private KeyCode _keySlot1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode _keySlot2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode _keySlot3 = KeyCode.Alpha3;

    [SerializeField] private GameObject _playerMainCamera;
    [SerializeField] private GameObject _playerHands;

    [SerializeField] private List<GameObject> _slots = new List<GameObject>();

    private HandsScript _hands;

    private void Update()
    {
        if (_playerMainCamera.GetComponent<ControllerCamera>().RotationX > 75f)
        {
            foreach (GameObject slot in _slots)
            {
                slot.GetComponent<SlotState>().Icon.SetActive(true);
            }

            _hands = _playerHands.GetComponent<HandsScript>();

            if (!_hands.IsEmpty && _hands.CurrentType == "Tool")
            {
                if (Input.GetKeyDown(_keySlot1)) SetSlot(0);
                if (Input.GetKeyDown(_keySlot2)) SetSlot(1);
                if (Input.GetKeyDown(_keySlot3)) SetSlot(2);
            }else if (_hands.IsEmpty)
            {
                if (Input.GetKeyDown(_keySlot1)) GrabSlot(0);
                if (Input.GetKeyDown(_keySlot2)) GrabSlot(1);
                if (Input.GetKeyDown(_keySlot3)) GrabSlot(2);
            }
        }
        else {
            foreach (GameObject slot in _slots)
            {
                slot.GetComponent<SlotState>().Icon.SetActive(false);
            }
        }
    }
    private void SetSlot(int slotNumber)
    {
        GameObject currentSlot = _slots[slotNumber];

        SlotState currentSlotState = currentSlot.GetComponent<SlotState>();

        if (currentSlotState.Tool.transform.childCount == 0)
        {
            _hands.CurrentTool.transform.parent = currentSlotState.Tool.transform;
            _hands.CurrentTool.layer = LayerMask.NameToLayer("Hands");

            _hands.CurrentTool.GetComponent<Rigidbody>().isKinematic = true;
            _hands.CurrentTool.GetComponent<BoxCollider>().enabled = false;

            _hands.CurrentTool.transform.localPosition = Vector3.zero;
            _hands.CurrentTool.transform.localEulerAngles = Vector3.zero;
            _hands.CurrentTool.transform.localScale = new Vector3(_hands.CurrentTool.transform.localScale.x / 2, _hands.CurrentTool.transform.localScale.y / 2, _hands.CurrentTool.transform.localScale.z / 2);

            _hands.CurrentTool = null;
            _hands.CurrentType = null;

            _hands.StateMainTool.IsPlayerGrabbing = false;
            _hands.StateMainTool = null;
            _hands.IsEmpty = true;
        }
        else if (currentSlotState.Tool.transform.childCount > 0)
        {
            _hands.CurrentTool.transform.parent = currentSlotState.Tool.transform;
            _hands.CurrentTool.layer = LayerMask.NameToLayer("Hands");

            _hands.CurrentTool.GetComponent<Rigidbody>().isKinematic = true;
            _hands.CurrentTool.GetComponent<BoxCollider>().enabled = false;

            _hands.CurrentTool.transform.localPosition = Vector3.zero;
            _hands.CurrentTool.transform.localEulerAngles = Vector3.zero;
            _hands.CurrentTool.transform.localScale = new Vector3(_hands.CurrentTool.transform.localScale.x / 2, _hands.CurrentTool.transform.localScale.y / 2, _hands.CurrentTool.transform.localScale.z / 2);

            _hands.CurrentTool = null;
            _hands.CurrentType = null;

            _hands.StateMainTool.IsPlayerGrabbing = false;
            _hands.StateMainTool = null;
            _hands.IsEmpty = true;

            this.GrabSlot(slotNumber);
        }
    }

    private void GrabSlot(int slotNumber)
    {
        GameObject currentSlot = _slots[slotNumber];

        SlotState currentSlotState = currentSlot.GetComponent<SlotState>();

        if (currentSlotState.Tool.transform.childCount != 0)
        {
            GameObject currentTool = currentSlotState.Tool.transform.GetChild(0).gameObject;

            currentTool.transform.parent = _playerHands.transform;
            currentTool.layer = LayerMask.NameToLayer("Hands");

            currentTool.GetComponent<Rigidbody>().isKinematic = true;
            currentTool.GetComponent<BoxCollider>().enabled = false;

            currentTool.transform.localPosition    = currentTool.GetComponent<StateToolObjects>().ToolPosition;
            currentTool.transform.localEulerAngles = currentTool.GetComponent<StateToolObjects>().ToolRotation;
            currentTool.transform.localScale       = currentTool.GetComponent<StateToolObjects>().ToolScale;

            _hands.CurrentTool = currentTool;
            _hands.CurrentType = "Tool";

            _hands.StateMainTool = currentTool.GetComponent<StateToolObjects>();
            _hands.StateMainTool.IsPlayerGrabbing = true;
            _hands.IsEmpty = false;
        }
    }
}   
