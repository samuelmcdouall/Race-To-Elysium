using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGameSettings : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static CGDGameSettings Instance;
    public static int CharacterNum = 1;
    public static int PlayerNum = -1; // have to decrease this by one if someone in the queue moves up
    public static float MouseSensitivity = -1.0f;
    public static float MusicVolume = -1.0f;
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
        MouseSensitivity = PlayerPrefs.GetFloat("Sensitivity", 10.5f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        //_view = GetComponent<PhotonView>();
    }

    //todo debug only
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            CharacterNum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CharacterNum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CharacterNum = 3;
        }
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
