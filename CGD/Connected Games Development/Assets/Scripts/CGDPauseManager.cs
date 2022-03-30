using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CGDPauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public Slider MouseSensitivitySlider;
    public Slider MusicVolumeSlider;
    public CGDMusicManager MusicManager;
    public static bool Paused;
    PhotonView _view;
    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        _view = GetComponent<PhotonView>();
        MouseSensitivitySlider.value = CGDGameSettings.MouseSensitivity;
        //MusicVolumeSlider.value = CGDGameSettings.MusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CGDGameOverScreenManager.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SettingsMenu.activeSelf)
                {
                    HideSettingsMenu();
                }
                else if (PauseMenu.activeSelf)
                {
                    HidePauseMenu();
                }
                else
                {
                    ShowPauseMenu();
                }
            }
        }
    }

    private void HideSettingsMenu()
    {
        SettingsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    void ShowPauseMenu()
    {
        PauseMenu.SetActive(true);
        Paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HidePauseMenu()
    {
        PauseMenu.SetActive(false);
        Paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnClickResumeButon()
    {
        HidePauseMenu();
    }

    public void OnClickSettingsButton()
    {
        ShowSettingsMenu();
    }

    public void OnClickBackButton()
    {
        HideSettingsMenu();
    }

    private void ShowSettingsMenu()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void OnClickQuitToMainMenuButton()
    {
        print("leaving this room");
        ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
        StartCoroutine(LeaveRoom());
    }
    public void OnClickQuitToDesktopButton()
    {
        print("leaving this room and quitting to desktop");
        ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
        StartCoroutine(LeaveRoomAndQuitApplication());
    }

    public void OnChangeMouseSensivitySlider()
    {
        CGDGameSettings.MouseSensitivity = MouseSensitivitySlider.value;
        PlayerPrefs.SetFloat("Sensitivity", MouseSensitivitySlider.value);
        PlayerPrefs.Save();
    }
    public void OnChangeMusicVolumeSlider()
    {
        CGDGameSettings.MusicVolume = MusicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
        PlayerPrefs.Save();
        MusicManager.UpdateMusicVolume(MusicVolumeSlider.value);
    }
    IEnumerator LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        CGDGameOverScreenManager.GameOver = false;
        PhotonNetwork.LoadLevel("MainMenuScene");
    }
    IEnumerator LeaveRoomAndQuitApplication()
    {
        PhotonNetwork.LeaveRoom(true);
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        Application.Quit();
        //CGDGameOverScreenManager.GameOver = false;
        //PhotonNetwork.LoadLevel("MainMenuScene");
    }


    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    print("leaving this room (disconnected from server)");
    //    ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
    //    base.OnDisconnected(cause);
    //}
    //void OnApplicationQuit()
    //{
    //    print("leaving this room (disconnected from server)");
    //    ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
    //}

    void ModifiyPlayerNumForAllPlayers(int leftPlayerNum)
    {
        print("Leaving the room, tell the room order to adjust in my absence");
        _view.RPC("ModifiyPlayerNum", RpcTarget.All, leftPlayerNum);
    }

    [PunRPC]
    void ModifiyPlayerNum(int leftPlayerNum)
    {
        print("Another player left the room, adjusting order");
        if (CGDGameSettings.PlayerNum > leftPlayerNum)
        {
            CGDGameSettings.PlayerNum--;
            print("You are now player: " + CGDGameSettings.PlayerNum);
        }
    }
}
