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
    private GameObject _handCurrentTool;

    private Transform _parentCurrentObjects;

    private StateGrabObjects _stateOfMainObject;
    private StateToolObjects _stateOfMainTool;

    private void Update()
    {
        Interactive();

        if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            if (Input.GetKeyDown(_keyHandsDrop) && _handCurrentObjects.Count != 0) { PlayerHandsDropObjects(); };
            if (Input.GetKeyDown(_keyHandsDrop) && _handCurrentObjects.Count == 0) { PlayerHandsDropTool(); };
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
        else if(!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        hitObject.layer = LayerMask.NameToLayer("Hands");

        if (!hitObject.TryGetComponent(out Rigidbody component))
        {
            hitObject.AddComponent<Rigidbody>();
        }

        hitObject.GetComponent<Rigidbody>().isKinematic = true;
        hitObject.GetComponent<MeshCollider>().enabled = false;

        hitObject.transform.parent = _playerHands.transform;
        hitObject.transform.localPosition = _stateOfMainTool.ToolPosition;
        hitObject.transform.localEulerAngles = _stateOfMainTool.ToolRotation;

        _handCurrentTool = hitObject;

        _stateOfMainTool.IsPlayerGrabbing = true;
    }
    private void PlayerHandsDropTool()
    {
        if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            _handCurrentTool.layer = LayerMask.NameToLayer("Default");
            _handCurrentTool.transform.parent = _parentCurrentObjects;

            _handCurrentTool.GetComponent<Rigidbody>().isKinematic = false;
            _handCurrentTool.GetComponent<MeshCollider>().enabled = true;

            _handCurrentTool.GetComponent<StateToolObjects>().IsPlayerGrabbing = false;
            _handCurrentTool.GetComponent<Rigidbody>().AddForce(_playerCamera.transform.forward * 10);

            _handCurrentTool = null;
            _stateOfMainTool = null;

            _playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty = true;
        }
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

        hitObject.layer = LayerMask.NameToLayer("Hands");

        if (!hitObject.TryGetComponent(out Rigidbody component))
        {
            hitObject.AddComponent<Rigidbody>();
        }

        hitObject.GetComponent<Rigidbody>().isKinematic = true;
        hitObject.GetComponent<MeshCollider>().enabled = false;

        hitObject.transform.parent = _playerHands.transform;
        hitObject.transform.localPosition = Vector3.zero;

        _handCurrentObjects.Add(hitObject);

        stateOfObject.IsPlayerGrabbing = true;
    }
    private void PlayerHandsDropObjects()
    {
        if (!_playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty)
        {
            for (int i = 0; i < _handCurrentObjects.Count; i++)
            {
                _handCurrentObjects[i].layer = LayerMask.NameToLayer("Default");
                _handCurrentObjects[i].transform.parent = _parentCurrentObjects;

                _handCurrentObjects[i].GetComponent<Rigidbody>().isKinematic = false;
                _handCurrentObjects[i].GetComponent<MeshCollider>().enabled = true;

                _handCurrentObjects[i].GetComponent<StateGrabObjects>().IsPlayerGrabbing = false;
                _handCurrentObjects[i].GetComponent<Rigidbody>().AddForce(_playerCamera.transform.forward * 10);
            }

            _handCurrentObjects.Clear();
            _stateOfMainObject = null;
            _playerHands.GetComponent<IsHandsEmpty>().isHandsEmpty = true;
        }
    }
}
