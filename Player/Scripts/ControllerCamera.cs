using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCamera : MonoBehaviour
{
    [SerializeField] private float _lookSpeed = 2.0f;
    [SerializeField] private Camera _playerCamera;

    private float _rotationX = 0;
    private float _lookXLimit = 90.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerCamera();
    }

    private void PlayerCamera()
    {
        _rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);

        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
    }
}
