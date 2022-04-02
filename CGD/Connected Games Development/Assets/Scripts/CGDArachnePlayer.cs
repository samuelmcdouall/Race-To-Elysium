using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CGDArachnePlayer : CGDPlayer
{
    [Header("Camera")]
    public GameObject MainCamera;
    [Header("Ultimate Attack")]
    public GameObject ArachneProjectile;
    public Transform ProjectileSpawnPoint;
    [SerializeField]
    float _projectileSpeed;

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
            print("Arachne ultimate attack!");
            UltimateCharge = 0.0f;
            UltimateBar.SetBar(UltimateCharge);
            GameObject projectile = PhotonNetwork.Instantiate(ArachneProjectile.name, ProjectileSpawnPoint.position, Quaternion.identity);
            projectile.GetComponent<CGDArachneProjectile>().OwnPlayer = gameObject;
            Vector3 forwardDirection = new Vector3(_cameraTr.forward.x, _cameraTr.forward.y, _cameraTr.forward.z);
            forwardDirection = forwardDirection.normalized;
            projectile.GetComponent<Rigidbody>().velocity = new Vector3(forwardDirection.x, forwardDirection.y, forwardDirection.z) * _projectileSpeed;
            
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
        _pickupHeld = PickupHeld.None;
        Cursor.lockState = CursorLockMode.Locked;
        if (!_view.IsMine)
        {
            Destroy(MainCamera);
        }
    }
}
