using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCamera : MonoBehaviour
{
    public float RotationX = 0;

    [SerializeField] private float _lookSpeed = 2.0f;
    [SerializeField] private GameObject _player;

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
        RotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
        RotationX = Mathf.Clamp(RotationX, -_lookXLimit, _lookXLimit);

        transform.localRotation = Quaternion.Euler(RotationX, 0, 0);
        _player.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
    }
}
