using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private KeyCode _keyInteract = KeyCode.E;
    [SerializeField] private KeyCode _keyHandsDrop = KeyCode.G;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _playerHands;
    [SerializeField] private GameObject _uiInteractiveCursor;

    [SerializeField] private float _interactRange = 2;

    private HandsScript _hands;
    private Transform _parentCurrentObjects;

    private void Update()
    {
        _hands = _playerHands.GetComponent<HandsScript>();

        Interactive();

        if (!_hands.IsEmpty)
        {
            if (Input.GetKeyDown(_keyHandsDrop) && _hands.CurrentObjects.Count != 0) { PlayerHandsDropObjects(); };
            if (Input.GetKeyDown(_keyHandsDrop) && _hands.CurrentTool != null) { PlayerHandsDropTool(); };
        }
        if (!_hands.IsEmpty && _hands.CurrentObjects.Count == 0)
        {
            if (Input.GetMouseButtonDown(0) && _hands.CurrentTool != null) { PlayerHandsUseTool(); };
        }
    }

    private void Interactive()
    {
        Ray ray = _playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange))
        {
            if (hit.collider.TryGetComponent(out StateGrabObjects stateOfObject) )
            {
                if (Input.GetKeyDown(_keyInteract) && _hands.CurrentTool == null) { PlayerHandsGrabObject(hit, stateOfObject); };

                _uiInteractiveCursor.SetActive(true);
            }

            if (hit.collider.TryGetComponent(out StateToolObjects stateOfTool)) 
            {
                if (Input.GetKeyDown(_keyInteract) && _hands.CurrentObjects.Count == 0) { PlayerHandsGrabTool(hit, stateOfTool); };

                _uiInteractiveCursor.SetActive(true);
            }

            if (hit.collider.TryGetComponent(out StatePalmObject stateOfPalm))
            {
                _uiInteractiveCursor.SetActive(true);
            }
        }
        else
        {
            _uiInteractiveCursor.SetActive(false);
        }
    }

    private void PlayerHandsGrabTool(RaycastHit hit, StateToolObjects stateOfTool)
    {
        if (_hands.IsEmpty)
        {
            _hands.StateMainTool = stateOfTool;
            _parentCurrentObjects = hit.transform.parent;

            _hands.IsEmpty = false;
        }
        else if (!_hands.IsEmpty)
        {
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        _hands.StateMainTool.Grab(_playerHands, _parentCurrentObjects, hitObject);

        _hands.CurrentTool = hitObject;
        _hands.CurrentType = "Tool";

        _hands.StateMainTool.IsPlayerGrabbing = true;
    }

    private void PlayerHandsDropTool()
    {
        if (!_hands.IsEmpty)
        {
            _hands.StateMainTool.Drop(_playerCamera, _hands.CurrentTool, _parentCurrentObjects);

            _hands.CurrentTool = null;
            _hands.StateMainTool = null;

            _hands.CurrentType = null;

            _hands.IsEmpty = true;
        }
    }
    private void PlayerHandsUseTool()
    {
        _hands.StateMainTool.Use(_playerCamera, _interactRange, _uiInteractiveCursor);
    }

    private void PlayerHandsGrabObject(RaycastHit hit, StateGrabObjects stateOfObject)
    {
        if (_hands.IsEmpty)
        {
            _hands.StateMainObject = stateOfObject;
            _parentCurrentObjects = hit.transform.parent;

            _hands.IsEmpty = false;
        }
        else if (!_hands.IsEmpty)
        {
            if (_hands.StateMainObject.TypeGrabObject != stateOfObject.TypeGrabObject) return;
            if (_hands.StateMainObject.HandsMaxAmoutObject <= _hands.CurrentObjects.Count) return;
        }

        GameObject hitObject = hit.collider.gameObject;

        stateOfObject.Grab(hitObject, _playerHands, _hands.CurrentObjects);

        _hands.CurrentObjects.Add(hitObject);

        _hands.CurrentType = "Objects";
    }

    private void PlayerHandsDropObjects()
    {
        if (!_hands.IsEmpty)
        {
            _hands.StateMainObject.Drop(_playerCamera, _hands.CurrentObjects, _parentCurrentObjects);

            _hands.CurrentObjects.Clear();
            _hands.StateMainObject = null;
            _hands.CurrentType = null;

            _hands.IsEmpty = true;
        }
    }
}
