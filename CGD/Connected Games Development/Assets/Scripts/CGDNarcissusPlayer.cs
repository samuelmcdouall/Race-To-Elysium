using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CGDNarcissusPlayer : CGDPlayer
{

    [Header("Ultimate Attack")]
    public GameObject UltimateCollider;
    public GameObject BlindFX;
    public GameObject MedusaPlayer;
    public GameObject MidasPlayer;
    public GameObject ArachnePlayer;

    void Awake()
    {
        InitialPlayerSetup();
    }

    public override void FixedUpdate()
    {
        if (_view.IsMine)
        {
            base.FixedUpdate();
        }
    }
    public override void Update()
    {
        if (_view.IsMine)
        {
            if (!CGDSpawnGateTimer._gameStarted && SceneManager.GetActiveScene().name == "GameScene")
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    GameObject newPlayer = PhotonNetwork.Instantiate(MedusaPlayer.name, transform.position, transform.rotation);
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseX = MainCamera.GetComponent<CGDRotateCamera>()._mouseX;
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseY = MainCamera.GetComponent<CGDRotateCamera>()._mouseY;
                    PhotonNetwork.Destroy(gameObject);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    GameObject newPlayer = PhotonNetwork.Instantiate(MidasPlayer.name, transform.position, transform.rotation);
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseX = MainCamera.GetComponent<CGDRotateCamera>()._mouseX;
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseY = MainCamera.GetComponent<CGDRotateCamera>()._mouseY;
                    PhotonNetwork.Destroy(gameObject);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    GameObject newPlayer = PhotonNetwork.Instantiate(ArachnePlayer.name, transform.position, transform.rotation);
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseX = MainCamera.GetComponent<CGDRotateCamera>()._mouseX;
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseY = MainCamera.GetComponent<CGDRotateCamera>()._mouseY;
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            if (_enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                HandleJumpMechanics();

                if (Input.GetMouseButtonDown(1))
                {
                    UltimateAttack();
                }
            }
            float scaledColorPosition = transform.position.y / 50.0f;
            if (scaledColorPosition > 1.0f)
            {
                scaledColorPosition = 1.0f;
            }
            else if (scaledColorPosition < 0.0f)
            {
                scaledColorPosition = 0.0f;
            }

            Color tartarusColor = new Color(1.0f, 0.0f, 0.0f);
            Color normalColour = new Color(0.45f, 0.45f, 0.45f);
            Color skyboxColor = Color.Lerp(tartarusColor, normalColour, scaledColorPosition);
            RenderSettings.skybox.SetColor("_Tint", skyboxColor);
        }
    }
    public override void UltimateAttack()
    {
        if (UltimateCharge == 100.0f && _enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            print("Narcissus ultimate attack!");
            UltimateCharge = 0.0f;
            UltimateBar.SetBar(UltimateCharge);
            AudioSource.PlayClipAtPoint(UltSFX, transform.position, CGDGameSettings.SoundVolume);
            PhotonNetwork.Instantiate(BlindFX.name, transform.position, Quaternion.identity);
            UltimateCollider.GetComponent<CGDNarcissusUltimateAttack>().ActivateUltimateCollider();
        }
        else
        {
            print("Not enough charge!");
        }
    }

    public override void InitialPlayerSetup()
    {
        // this is being called twice todo
        PlayerRb = GetComponent<Rigidbody>();
        _view = GetComponent<PhotonView>();
        Cursor.visible = _showCursor;
        _cameraTr = Camera.main.transform;
        _ableToJumpOffGround = true;
        _speedModifier = 1.0f;
        _jumpModifier = 1.0f;
        UltimateCharge = 0.0f;
        Cursor.lockState = CursorLockMode.Locked;
        if (!_view.IsMine)
        {
            print(_view.Owner.NickName + " has joined the game");
            NameText.text = _view.Owner.NickName;
        }
        else
        {
            NameText.text = "";
        }
        if (!_view.IsMine)
        {
            Destroy(MainCamera);
        }
    }
}
