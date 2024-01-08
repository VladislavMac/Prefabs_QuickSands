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

    private List<GameObject> _handCurrentObjects = new List<GameObject>();
    [SerializeField] private GameObject _handCurrentTool;

    private Transform _parentCurrentObjects;

    private StateGrabObjects _stateOfMainObject;
    [SerializeField] private StateToolObjects _stateOfMainTool;

    private void Update()
    {
        Interactive();

        if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            if (Input.GetKeyDown(_keyHandsDrop) && _handCurrentObjects.Count != 0) { PlayerHandsDropObjects(); };
            if (Input.GetKeyDown(_keyHandsDrop) && _handCurrentTool != null) { PlayerHandsDropTool(); };
        }
        if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty && _handCurrentObjects.Count == 0)
        {
            if (Input.GetMouseButtonDown(0) && _handCurrentTool != null) { PlayerHandsUseTool(); };
        }
    }

    private void Interactive()
    {
        Ray ray = _playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange))
        {
            if (hit.collider.TryGetComponent(out StateGrabObjects stateOfObject) )
            {
                if (Input.GetKeyDown(_keyInteract) && _handCurrentTool == null) { PlayerHandsGrabObject(hit, stateOfObject); };

                _uiInteractiveCursor.SetActive(true);
            }

            if (hit.collider.TryGetComponent(out StateToolObjects stateOfTool)) 
            {
                if (Input.GetKeyDown(_keyInteract) && _handCurrentObjects.Count == 0) { PlayerHandsGrabTool(hit, stateOfTool); };

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
        if (_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            _stateOfMainTool = stateOfTool;
            _parentCurrentObjects = hit.transform.parent;

            _playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty = false;
        }
        else if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        _stateOfMainTool.Grab(_playerHands, _parentCurrentObjects, hitObject);

        _handCurrentTool = hitObject;
        _stateOfMainTool.IsPlayerGrabbing = true;
    }

    private void PlayerHandsDropTool()
    {
        if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            _stateOfMainTool.Drop(_playerCamera, _handCurrentTool, _parentCurrentObjects);

            _stateOfMainTool = null;

            _playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty = true;
        }
    }
    private void PlayerHandsUseTool()
    {
        _stateOfMainTool.Use(_playerCamera, _interactRange, _uiInteractiveCursor);
    }




    private void PlayerHandsGrabObject(RaycastHit hit, StateGrabObjects stateOfObject)
    {
        if (_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            _stateOfMainObject = stateOfObject;
            _parentCurrentObjects = hit.transform.parent;

            _playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty = false;
        }
        else if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            if (_stateOfMainObject.TypeGrabObject != stateOfObject.TypeGrabObject) return;
            if (_stateOfMainObject.HandsMaxAmoutObject <= _handCurrentObjects.Count) return;
        }

        GameObject hitObject = hit.collider.gameObject;

        stateOfObject.Grab(hitObject, _playerHands, _handCurrentObjects);
    }
    private void PlayerHandsDropObjects()
    {
        if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            _stateOfMainObject.Drop(_playerCamera, _handCurrentObjects, _parentCurrentObjects);

            _stateOfMainObject = null;

            _playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty = true;
        }
    }
}
