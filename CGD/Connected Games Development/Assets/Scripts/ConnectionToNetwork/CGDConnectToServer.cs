using Photon.Pun;
using UnityEngine.SceneManagement;

public class CGDConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            print("Not connected, attempting to connect");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        print("Successfuly connected to master!");
        SceneManager.LoadScene("LoginScene");
    }
}
