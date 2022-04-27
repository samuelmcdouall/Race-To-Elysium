using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CGDGameOverScreenManager : MonoBehaviourPunCallbacks
{
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

    public static void TriggerGameOverState()
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
        LeaveRoom();
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        GameOver = false;
        SceneManager.LoadScene("MainMenuScene");

        base.OnLeftRoom();
    }
}
