using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CGDCreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    //todo rename to general main menu buttons manager
    public InputField CreateRoomInput;
    public InputField JoinRoomInput;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public Slider MusicSlider;
    public CGDMainMenuMusicManager MusicManager;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void BackButton()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void OnMusicSldierChange()
    {
        CGDGameSettings.MusicVolume = MusicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        PlayerPrefs.Save();
        MusicManager.UpdateMusicVolume(MusicSlider.value);
        
    }
}
