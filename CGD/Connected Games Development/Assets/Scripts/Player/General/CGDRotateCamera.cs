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

    [Header("Own Player")]
    public PhotonView View;
    public GameObject OwnPlayer;

    float _maxPossibleCameraDistance; // todo remove if not needed
    int _cameraLayerIgnore = 7;

    GameObject _tmpObject; // todo remove if not needed

    void Start()
    {
        _maxPossibleCameraDistance = Vector3.Distance(transform.position, CameraTargetToRotateAround.position);
        print("Max possible camera distance" + _maxPossibleCameraDistance);
    }
    void Update()
    {
        if (View.IsMine && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused) //todo since the camera is destroyed on other players may not need the photon view check
        {
            GetMouseInput();
            CameraTargetToRotateAround.rotation = Quaternion.Euler(_mouseY, _mouseX, 0.0f);
        }
        //DetermineIfCameraShouldMoveCloser();
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
        Debug.DrawRay(transform.position, cameraToPlayerDirection * 10.0f, Color.cyan, 0.1f); //todo can remove at end
        RaycastHit hit;

        if (Physics.Raycast(transform.position, cameraToPlayerDirection, out hit, 10.0f, _cameraLayerIgnore))
        {
            if (hit.transform.gameObject != OwnPlayer)
            {
                OwnPlayer.GetComponent<CGDPlayer>().PlayerOutline.enabled = true;
                //if (_tmpObject && _tmpObject.GetComponent<Renderer>() && _tmpObject.GetComponent<Renderer>().material.color.a == 0.5f) //todo get rid of after confirmed going with outline version
                //{
                //    Color _tmpColor = _tmpObject.GetComponent<Renderer>().material.color;
                //    _tmpObject.GetComponent<Renderer>().material.color = new Color(_tmpColor.r, _tmpColor.g, _tmpColor.b, 1.0f);
                //}
                //_tmpObject = hit.transform.gameObject;
                //if (_tmpObject && _tmpObject.GetComponent<Renderer>() && _tmpObject.GetComponent<Renderer>().material.color.a == 1.0f)
                //{
                //    Color _tmpColor = _tmpObject.GetComponent<Renderer>().material.color;
                //    _tmpObject.GetComponent<Renderer>().material.color = new Color(_tmpColor.r, _tmpColor.g, _tmpColor.b, 0.5f);
                //}
            }
            else
            {
                OwnPlayer.GetComponent<CGDPlayer>().PlayerOutline.enabled = false;
                //if (_tmpObject && _tmpObject.GetComponent<Renderer>() && _tmpObject.GetComponent<Renderer>().material.color.a == 0.5f)
                //{
                //    Color _tmpColor = _tmpObject.GetComponent<Renderer>().material.color;
                //    _tmpObject.GetComponent<Renderer>().material.color = new Color(_tmpColor.r, _tmpColor.g, _tmpColor.b, 1.0f);
                //}
            }
        }
    }

    void DetermineIfCameraShouldMoveCloser()
    {
        Vector3 cameraToPlayerDirection;
        Vector3 maxPossibleCameraPosition;
        DetermineMaxPossibleCameraPosition(out cameraToPlayerDirection, out maxPossibleCameraPosition);
        RaycastHit hit;

        if (Physics.Raycast(maxPossibleCameraPosition, cameraToPlayerDirection, out hit, 10.0f, _cameraLayerIgnore))
        {
            if (hit.transform.gameObject != OwnPlayer)
            {
                float playerToObstacleDistance = Vector3.Distance(hit.point, OwnPlayer.transform.position);
                print("reduced distance at: " + playerToObstacleDistance);
                transform.position = OwnPlayer.transform.position + -cameraToPlayerDirection * playerToObstacleDistance;
            }
            else
            {
                transform.position = maxPossibleCameraPosition;
            }
        }
    }

    void DetermineMaxPossibleCameraPosition(out Vector3 cameraToPlayerDirection, out Vector3 maxPossibleCameraPosition)
    {
        cameraToPlayerDirection = (OwnPlayer.transform.position - transform.position).normalized;
        maxPossibleCameraPosition = OwnPlayer.transform.position + -cameraToPlayerDirection * _maxPossibleCameraDistance;
        Debug.DrawRay(maxPossibleCameraPosition, cameraToPlayerDirection * 10.0f, Color.cyan, 0.1f); //todo can remove at end
    }
}
