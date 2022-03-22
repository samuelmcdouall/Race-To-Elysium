using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGameSettings : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static CGDGameSettings Instance;
    public static int PlayerNum = -1; // have to decrease this by one if someone in the queue moves up
    public static float MouseSensitivity = 10.0f; 
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //_view = GetComponent<PhotonView>();
    }

    //public override void OnLeftRoom()
    //{
    //    print("leaving this room");
    //    ModifiyPlayerNumForAllPlayers(PlayerNum);
    //    base.OnLeftRoom();
    //}

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    print("leaving this room (disconnected from server)");
    //    ModifiyPlayerNumForAllPlayers(PlayerNum);
    //    base.OnDisconnected(cause);
        
    //}

    //[PunRPC]
    //void ModifiyPlayerNumForAllPlayers(int leftPlayerNum) 
    //{
    //    print("Leaving the room, tell the room order to adjust in my absence");
    //    _view.RPC("ModifiyPlayerNum", RpcTarget.All, leftPlayerNum);
    //}

    //void ModifiyPlayerNum(int leftPlayerNum)
    //{
    //    print("Another player left the room, adjusting order");
    //    if (PlayerNum > leftPlayerNum)
    //    {
    //        PlayerNum--;
    //        print("You are now player: " + PlayerNum);
    //    }
    //}


}
