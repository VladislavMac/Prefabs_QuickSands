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
        _hands.GrabTool(hit, stateOfTool);
    }

    private void PlayerHandsDropTool()
    {
        _hands.DropTool(_playerCamera);
    }
    private void PlayerHandsUseTool()
    {
        _hands.StateMainTool.Use(_playerCamera, _interactRange, _uiInteractiveCursor);
    }

    private void PlayerHandsGrabObject(RaycastHit hit, StateGrabObjects stateOfObject)
    {
        _hands.GrabObject(hit, stateOfObject);
    }

    private void PlayerHandsDropObjects()
    {
        _hands.DropObjects(_playerCamera);
    }
}
