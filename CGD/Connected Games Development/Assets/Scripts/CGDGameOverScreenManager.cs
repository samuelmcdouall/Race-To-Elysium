using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGameOverScreenManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static GameObject WinScreen;
    public static GameObject LossScreen;
    public static GameObject PauseMenu;
    public static GameObject SettingsMenu;
    public static bool GameOver;
    public CGDPauseManager PauseManager;

    void Start()
    {
        GameOver = false;
        WinScreen = GameObject.FindGameObjectWithTag("WinScreen");
        WinScreen.SetActive(false);
        LossScreen = GameObject.FindGameObjectWithTag("LossScreen");
        LossScreen.SetActive(false);
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        PauseMenu.SetActive(false);
        SettingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
        SettingsMenu.SetActive(false);
    }
    public static void DisplayWinScreen()
    {
        print("DISPLAY WIN SCREEN");
        TriggerGameOverState();
        WinScreen.SetActive(true);
    }
    public static void DisplayLossScreen()
    {
        print("DISPLAY LOSS SCREEN");
        TriggerGameOverState();
        LossScreen.SetActive(true);
    }

    private static void TriggerGameOverState()
    {
        GameOver = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(false);
        }
        if (SettingsMenu.activeSelf)
        {
            SettingsMenu.SetActive(false);
        }
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
