using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CGDCreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField CreateRoomInput;
    public InputField JoinRoomInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("PlayerLobbyScene");
    }
}
