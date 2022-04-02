using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CGDNarcissusPlayer : CGDPlayer
{
    [Header("Camera")]
    public GameObject MainCamera;
    [Header("Ultimate Attack")]
    public GameObject UltimateCollider;
    public GameObject BlindFX;

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
        UltimateCharge = 0.0f;
        _powerUpHeld = PowerUpHeld.None;
        Cursor.lockState = CursorLockMode.Locked;
        if (!_view.IsMine)
        {
            Destroy(MainCamera);
        }
    }
}
