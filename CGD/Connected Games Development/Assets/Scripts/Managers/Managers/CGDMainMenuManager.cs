using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CGDMainMenuManager : MonoBehaviourPunCallbacks
{
    public InputField CreateRoomInput;
    public InputField JoinRoomInput;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public Slider MusicVolumeSlider;
    public Slider SoundVolumeSlider;
    public CGDMusicManager MusicManager;
    public AudioClip ClickSFX;
    GameObject _audioListenerPosition;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _audioListenerPosition = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void OnClickJoinRandomRoomButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void OnClickCreateRoomButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        PhotonNetwork.CreateRoom(CreateRoomInput.text);
    }

    public void OnClickJoinRoomButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        PhotonNetwork.JoinRoom(JoinRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("PlayerLobbyScene");
    }

    public void OnClickQuitButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        Application.Quit();
    }

    public void OnClickSettingsButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void OnClickBackButton()
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
