using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CGDPauseManager : MonoBehaviourPunCallbacks
{
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public Slider MouseSensitivitySlider;
    public Slider MusicVolumeSlider;
    public Slider SoundVolumeSlider;
    public CGDMusicManager MusicManager;
    public AudioClip ClickSFX;
    public static bool Paused;
    PhotonView _view;
    GameObject _audioListenerPosition;

    void Start()
    {
        Paused = false;
        _view = GetComponent<PhotonView>();
        MouseSensitivitySlider.value = CGDGameSettings.MouseSensitivity;
    }

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
        // Check needed if character changed as original Main Camera will not exist
        if (!_audioListenerPosition)
        {
            _audioListenerPosition = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    void HideSettingsMenu()
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

    void HidePauseMenu()
    {
        PauseMenu.SetActive(false);
        Paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnClickResumeButon()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        HidePauseMenu();
    }

    public void OnClickSettingsButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        ShowSettingsMenu();
    }

    public void OnClickBackButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        HideSettingsMenu();
    }

    public void OnClickQuitToMainMenuButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
        LeaveRoom();
    }

    public void OnClickQuitToDesktopButton()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
        LeaveRoom();
        Application.Quit();
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

    public void OnChangeSoundVolumeSlider()
    {
        CGDGameSettings.SoundVolume = SoundVolumeSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", SoundVolumeSlider.value);
        PlayerPrefs.Save();
    }

    void ShowSettingsMenu()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        CGDGameOverScreenManager.GameOver = false;
        SceneManager.LoadScene("MainMenuScene");

        base.OnLeftRoom();
    }

    void ModifiyPlayerNumForAllPlayers(int leftPlayerNum)
    {
        Debug.Log("Leaving the room, tell the room order to adjust in my absence");
        _view.RPC("ModifiyPlayerNum", RpcTarget.All, leftPlayerNum);
    }

    [PunRPC]
    public void ModifiyPlayerNum(int leftPlayerNum)
    {
        Debug.Log("Another player left the room, adjusting order");
        if (CGDGameSettings.PlayerNum > leftPlayerNum)
        {
            CGDGameSettings.PlayerNum--;
            print("You are now player: " + CGDGameSettings.PlayerNum);
        }
    }
}
