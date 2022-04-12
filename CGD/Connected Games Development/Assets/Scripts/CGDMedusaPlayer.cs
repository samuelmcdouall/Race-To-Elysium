using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CGDMedusaPlayer : CGDPlayer
{
    [Header("Ultimate Attack")]
    [SerializeField]
    float _freezeDuration;
    [SerializeField]
    float _freezeRange;
    public GameObject FreezeFX;
    public AudioClip HitFreezeSFX;
    public GameObject MidasPlayer;
    public GameObject NarcissusPlayer;
    public GameObject ArachnePlayer;
    public GameObject Crosshair;

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
            if (SceneManager.GetActiveScene().name == "GameScene" && !CGDSpawnGateTimer._gameStarted && Input.GetKeyDown(KeyCode.E))
            {
                if (NewPlayer == PlayerToChangeTo.Midas) //todo this will be done via interacting with portraits, just do as number keys for proof of concept
                {
                    GameObject newPlayer = PhotonNetwork.Instantiate(MidasPlayer.name, transform.position, transform.rotation);
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseX = MainCamera.GetComponent<CGDRotateCamera>()._mouseX;
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseY = MainCamera.GetComponent<CGDRotateCamera>()._mouseY;
                    AudioSource.PlayClipAtPoint(NewPlayerSFX, MainCamera.transform.position, CGDGameSettings.SoundVolume);
                    Instantiate(NewPlayerFX, transform.position, Quaternion.identity);
                    PhotonNetwork.Destroy(gameObject);
                }
                if (NewPlayer == PlayerToChangeTo.Narcissus)
                {
                    GameObject newPlayer = PhotonNetwork.Instantiate(NarcissusPlayer.name, transform.position, transform.rotation);
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseX = MainCamera.GetComponent<CGDRotateCamera>()._mouseX;
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseY = MainCamera.GetComponent<CGDRotateCamera>()._mouseY;
                    AudioSource.PlayClipAtPoint(NewPlayerSFX, MainCamera.transform.position, CGDGameSettings.SoundVolume);
                    Instantiate(NewPlayerFX, transform.position, Quaternion.identity);
                    PhotonNetwork.Destroy(gameObject);
                }
                if (NewPlayer == PlayerToChangeTo.Arachne)
                {
                    GameObject newPlayer = PhotonNetwork.Instantiate(ArachnePlayer.name, transform.position, transform.rotation);
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseX = MainCamera.GetComponent<CGDRotateCamera>()._mouseX;
                    newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseY = MainCamera.GetComponent<CGDRotateCamera>()._mouseY;
                    AudioSource.PlayClipAtPoint(NewPlayerSFX, MainCamera.transform.position, CGDGameSettings.SoundVolume);
                    Instantiate(NewPlayerFX, transform.position, Quaternion.identity);
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
            print("Medusa ultimate attack!");
            UltimateCharge = 0.0f;
            UltimateBar.SetBar(UltimateCharge);
            AudioSource.PlayClipAtPoint(UltSFX, transform.position, CGDGameSettings.SoundVolume);
            RaycastHit hit;
            Vector3 forwardDirection = new Vector3(_cameraTr.forward.x, _cameraTr.forward.y, _cameraTr.forward.z);
            forwardDirection = forwardDirection.normalized;
            Debug.DrawRay(_cameraTr.position, forwardDirection * _freezeRange, Color.red, 10.0f); //todo remove this eventually
            if (Physics.Raycast(_cameraTr.position, forwardDirection, out hit, _freezeRange))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    print("I just hit a player, freeze them!");
                    AudioSource.PlayClipAtPoint(HitFreezeSFX, transform.position, CGDGameSettings.SoundVolume);
                    int photonViewID = hit.transform.gameObject.GetComponent<PhotonView>().ViewID;
                    PhotonNetwork.Instantiate(FreezeFX.name, hit.transform.position, Quaternion.identity);
                    hit.transform.gameObject.GetComponent<CGDPlayer>().DisableControlsForSecondsToGivenPlayer(_freezeDuration, photonViewID, true);
                    // todo may want to do other things such as block camera rotation or abilites too
                }
            }
            else
            {
                print("Didn't hit anything!");
            }
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
        NewPlayer = PlayerToChangeTo.None;
        if (!_view.IsMine)
        {
            Crosshair.SetActive(false);
        }
        //if (SetupCameraPosition != Vector3.zero)
        //{
        //    print("changing to a new character");
        //    Camera.main.transform.position = SetupCameraPosition;
        //}
        //else
        //{
        //    print("this is the first character I'm playing");
        //}
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
