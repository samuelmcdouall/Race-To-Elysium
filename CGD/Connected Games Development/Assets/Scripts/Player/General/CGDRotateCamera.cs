using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CGDRotateCamera : MonoBehaviour
{
    [Header("Rotation")]
    //[SerializeField]
    //float RotationSpeed = 10.0f;
    public Transform CameraTargetToRotateAround;
    [System.NonSerialized]
    public float _mouseX;
    [System.NonSerialized]
    public float _mouseY;

    [Header("Vertical Mouse Limits")]
    [SerializeField]
    float MouseYMinClamp = -35.0f;
    [SerializeField]
    float MouseYMaxClamp = 60.0f;

    public PhotonView View;
    public GameObject OwnPlayer;

    float _maxPossibleCameraDistance;

    int _hazardLayerIgnore = 7;

    //[Header("Menus")]
    //public GameObject pause_menu;
    //public GameObject options_menu;
    //public GameObject controls_menu;
    void Start()
    {
        _maxPossibleCameraDistance = Vector3.Distance(transform.position, CameraTargetToRotateAround.position); 
    }
    void Update()
    {
        //if (!pause_menu.activeSelf && !options_menu.activeSelf && !controls_menu.activeSelf)
        //{
        if (View.IsMine && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            GetMouseInput();
            CameraTargetToRotateAround.rotation = Quaternion.Euler(_mouseY, _mouseX, 0.0f);
        }
        RaycastHit hit;
        Vector3 cameraToPlayerDirection = OwnPlayer.transform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        cameraToPlayerDirection = cameraToPlayerDirection.normalized;
        Vector3 maxPossibleCameraPosition = OwnPlayer.transform.position + -cameraToPlayerDirection * _maxPossibleCameraDistance;
        Debug.DrawRay(maxPossibleCameraPosition, cameraToPlayerDirection * 10.0f, Color.cyan, 1.0f);
        
        if (Physics.Raycast(maxPossibleCameraPosition, cameraToPlayerDirection, out hit, 10.0f, _hazardLayerIgnore))
        {
            if (hit.transform.gameObject != OwnPlayer)
            {
                //print("can't see player, move camera closer");
                float playerToObstacleDistance = Vector3.Distance(hit.point, OwnPlayer.transform.position);
                transform.position = OwnPlayer.transform.position + -cameraToPlayerDirection * (playerToObstacleDistance);
            }
            else
            {
                //print("can see player, move camera to normal position");
                transform.position = maxPossibleCameraPosition;
            }
        }
        //}
    }

    void GetMouseInput()
    {
        _mouseX += Input.GetAxis("Mouse X") * CGDGameSettings.MouseSensitivity;//SettingsManager.look_sensitivity;
        _mouseY -= Input.GetAxis("Mouse Y") * CGDGameSettings.MouseSensitivity;//SettingsManager.look_sensitivity;
        _mouseY = Mathf.Clamp(_mouseY, MouseYMinClamp, MouseYMaxClamp);
    }
}
