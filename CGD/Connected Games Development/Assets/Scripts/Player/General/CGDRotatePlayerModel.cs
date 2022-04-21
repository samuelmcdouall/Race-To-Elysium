using UnityEngine;
using Photon.Pun;

public class CGDRotatePlayerModel : MonoBehaviour
{
    PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (_view.IsMine && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            transform.Rotate(0.0f, Input.GetAxis("Mouse X") * CGDGameSettings.MouseSensitivity, 0.0f);
        }
    }
}
