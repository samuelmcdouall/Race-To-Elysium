using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDRotateCamera : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField]
    float RotationSpeed = 10.0f;
    public Transform CameraTargetToRotateAround;
    float _mouseX;
    float _mouseY;

    [Header("Vertical Mouse Limits")]
    [SerializeField]
    float MouseYMinClamp = -35.0f;
    [SerializeField]
    float MouseYMaxClamp = 60.0f;

    //[Header("Menus")]
    //public GameObject pause_menu;
    //public GameObject options_menu;
    //public GameObject controls_menu;
    void Update()
    {
        //if (!pause_menu.activeSelf && !options_menu.activeSelf && !controls_menu.activeSelf)
        //{
            GetMouseInput();
            CameraTargetToRotateAround.rotation = Quaternion.Euler(_mouseY, _mouseX, 0.0f);
        //}
    }

    void GetMouseInput()
    {
        _mouseX += Input.GetAxis("Mouse X") * RotationSpeed;//SettingsManager.look_sensitivity;
        _mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;//SettingsManager.look_sensitivity;
        _mouseY = Mathf.Clamp(_mouseY, MouseYMinClamp, MouseYMaxClamp);
    }
}
