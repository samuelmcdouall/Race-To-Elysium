using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CGDMainMenuManager : MonoBehaviourPunCallbacks
{
    //todo rename to general main menu buttons manager
    public InputField CreateRoomInput;
    public InputField JoinRoomInput;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public Slider MusicVolumeSlider;
    public Slider SoundVolumeSlider;
    public CGDMusicManager MusicManager;
    public AudioClip ClickSFX;
    GameObject _audioListenerPosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _audioListenerPosition = GameObject.FindGameObjectWithTag("MainCamera");
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
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        PhotonNetwork.LoadLevel("PlayerLobbyScene");
    }

    public void QuitButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        Application.Quit();
    }

    public void SettingsButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void BackButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void OnChangeMusicVolumeSlider()
    {
        CGDGameSettings.MusicVolume = MusicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
        PlayerPrefs.Save();
        MusicManager.UpdateMusicVolume(MusicVolumeSlider.value);
    }

    public void OnChangeSoundVolumeSlider()
    {
        CGDGameSettings.SoundVolume = SoundVolumeSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", SoundVolumeSlider.value);
        PlayerPrefs.Save();
    }
}
