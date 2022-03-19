using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGameOverScreenManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static GameObject WonScreen;
    public static GameObject LossScreen;
    public static bool GameOver;

    void Start()
    {
        GameOver = false;
        WonScreen = GameObject.FindGameObjectWithTag("WinScreen");
        WonScreen.SetActive(false);
        LossScreen = GameObject.FindGameObjectWithTag("LossScreen");
        LossScreen.SetActive(false);
    }
    public static void DisplayWinScreen()
    {
        print("DISPLAY WIN SCREEN");
        GameOver = true;
        Cursor.visible = true;
        WonScreen.SetActive(true);
    }
    public static void DisplayLossScreen()
    {
        print("DISPLAY LOSS SCREEN");
        GameOver = true;
        Cursor.visible = true;
        LossScreen.SetActive(true);
    }

    public void OnClickMainMenuButton()
    {
        StartCoroutine(LeaveRoom());
    }
    IEnumerator LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        GameOver = false;
        PhotonNetwork.LoadLevel("MainMenuScene");
    }
}
