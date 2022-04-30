using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CGDConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Not connected, attempting to connect");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfuly connected to master!");
        SceneManager.LoadScene("LoginScene");
    }
}
