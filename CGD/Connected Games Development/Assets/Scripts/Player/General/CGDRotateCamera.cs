using UnityEngine;
using Photon.Pun;

public class CGDRotateCamera : MonoBehaviour
{
    [Header("Rotation")]
    public Transform CameraTargetToRotateAround;
    [System.NonSerialized]
    public float _mouseX;
    [System.NonSerialized]
    public float _mouseY;

    [Header("Vertical Mouse Limits")]
    float MouseYMinClamp = -35.0f;
    float MouseYMaxClamp = 60.0f;

    public GameObject OwnPlayer;
    int _cameraLayerIgnore = 7;

    void Update()
    {
        if (!CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            GetMouseInput();
            CameraTargetToRotateAround.rotation = Quaternion.Euler(_mouseY, _mouseX, 0.0f);
        }
        DetermineIfCameraGoesThroughObstacle();
    }

    void GetMouseInput()
    {
        _mouseX += Input.GetAxis("Mouse X") * CGDGameSettings.MouseSensitivity;
        _mouseY -= Input.GetAxis("Mouse Y") * CGDGameSettings.MouseSensitivity;
        _mouseY = Mathf.Clamp(_mouseY, MouseYMinClamp, MouseYMaxClamp);
    }

    void DetermineIfCameraGoesThroughObstacle()
    {
        Vector3 cameraToPlayerDirection = (OwnPlayer.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, cameraToPlayerDirection, out hit, 10.0f, _cameraLayerIgnore))
        {
            if (hit.transform.gameObject != OwnPlayer)
            {
                OwnPlayer.GetComponent<CGDPlayer>().PlayerOutline.enabled = true;
            }
            else
            {
                OwnPlayer.GetComponent<CGDPlayer>().PlayerOutline.enabled = false;
            }
        }
    }
}
