using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDPowerUpManager : MonoBehaviour
{
    [Header("General")]
    [System.NonSerialized]
    public PowerUpHeld _powerUpHeld;
    CGDPlayer _playerScript;
    public Transform ProjectileSpawnPoint;
    [SerializeField]
    float _iconFlashRate;
    public GameObject AreaDenialProjectile;
    [SerializeField]
    float _areaDenialProjectileSpeed;
    public AudioClip AreaDenialProjectileLaunchSFX;

    [Header("Speed Boost")]
    public GameObject SpeedBoostIcon;
    [SerializeField]
    float _speedBoostPerModifier;
    [SerializeField]
    float _speedBoostDuration;
    public AudioClip SpeedBoostSFX;

    [Header("Jump Boost")]
    public GameObject JumpBoostIcon;
    [SerializeField]
    float _jumpBoostPerModifier;
    [SerializeField]
    float _jumpBoostDuration;
    public AudioClip JumpBoostSFX;

    [Header("Speed and Jump Boost")]
    public GameObject SpeedJumpBoostIcon;
    [SerializeField]
    float _speedBothBoostPerModifier;
    [SerializeField]
    float _jumpBothBoostPerModifier;
    [SerializeField]
    float _speedJumpBoostDuration;
    public AudioClip SpeedJumpBoostSFX;

    [Header("Peel")]
    public GameObject PeelIcon;
    public AudioClip PeelSFX;

    [Header("Spikes")]
    public GameObject SpikesIcon;
    public AudioClip SpikesSFX;

    [Header("Poison Cloud")]
    public GameObject PoisonCloudIcon;
    public AudioClip PoisonCloudSFX;

    [Header("Lava Pool")]
    public GameObject LavaPoolIcon;
    public AudioClip LavaPoolSFX;

    PhotonView _view;


    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _powerUpHeld = PowerUpHeld.None;
        _playerScript = GetComponent<CGDPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                switch (_powerUpHeld)
                {
                    case PowerUpHeld.SpeedBoost:
                        ActivateSpeedBoost(_speedBoostPerModifier, _speedBoostDuration);
                        break;
                    case PowerUpHeld.JumpBoost:
                        ActivateJumpBoost(_jumpBoostPerModifier, _jumpBoostDuration);
                        break;
                    case PowerUpHeld.SpeedAndJumpBoost:
                        ActivateSpeedAndJumpBoost(_speedBothBoostPerModifier, _jumpBothBoostPerModifier, _speedJumpBoostDuration);
                        break;
                    case PowerUpHeld.Peel:
                        FireAreaDenialProjectile(PowerUpHeld.Peel);
                        break;
                    case PowerUpHeld.Spikes:
                        FireAreaDenialProjectile(PowerUpHeld.Spikes);
                        break;
                    case PowerUpHeld.PoisonCloud:
                        FireAreaDenialProjectile(PowerUpHeld.PoisonCloud);
                        break;
                    case PowerUpHeld.LavaPool:
                        FireAreaDenialProjectile(PowerUpHeld.LavaPool);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void ActivateSpeedBoost(float modifier, float duration)
    {
        _powerUpHeld = PowerUpHeld.None;
        AudioSource.PlayClipAtPoint(SpeedBoostSFX, transform.position, CGDGameSettings.SoundVolume);
        StartCoroutine(DisplayFlashingIcon(SpeedBoostIcon, duration));
        _playerScript.ApplySpeedModifierForSeconds(-modifier, duration);
    }

    void ActivateJumpBoost(float modifier, float duration)
    {
        _powerUpHeld = PowerUpHeld.None;
        AudioSource.PlayClipAtPoint(JumpBoostSFX, transform.position, CGDGameSettings.SoundVolume);
        StartCoroutine(DisplayFlashingIcon(JumpBoostIcon, duration));
        _playerScript.ApplyJumpModifierForSeconds(-modifier, duration);
    }
    void ActivateSpeedAndJumpBoost(float speedModifier, float jumpModifier, float duration)
    {
        _powerUpHeld = PowerUpHeld.None;
        AudioSource.PlayClipAtPoint(SpeedJumpBoostSFX, transform.position, CGDGameSettings.SoundVolume);
        StartCoroutine(DisplayFlashingIcon(SpeedJumpBoostIcon, duration));
        _playerScript.ApplySpeedModifierForSeconds(-speedModifier, duration);
        _playerScript.ApplyJumpModifierForSeconds(-jumpModifier, duration);
    }
    void FireAreaDenialProjectile(PowerUpHeld areaDenialType)
    {
        _powerUpHeld = PowerUpHeld.None;
        AudioSource.PlayClipAtPoint(AreaDenialProjectileLaunchSFX, transform.position, CGDGameSettings.SoundVolume);
        GameObject projectile = PhotonNetwork.Instantiate(AreaDenialProjectile.name, ProjectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<CGDAreaDenialProjectile>().OwnPlayer = gameObject;
        
        switch (areaDenialType)
        {
            case PowerUpHeld.Peel:
                projectile.GetComponent<CGDAreaDenialProjectile>().ProjectileType = CGDAreaDenialProjectile.AreaDenialProjectileType.Peel;
                PeelIcon.SetActive(false);
                break;
            case PowerUpHeld.Spikes:
                projectile.GetComponent<CGDAreaDenialProjectile>().ProjectileType = CGDAreaDenialProjectile.AreaDenialProjectileType.Spikes;
                SpikesIcon.SetActive(false);
                break;
            case PowerUpHeld.PoisonCloud:
                projectile.GetComponent<CGDAreaDenialProjectile>().ProjectileType = CGDAreaDenialProjectile.AreaDenialProjectileType.PoisonCloud;
                PoisonCloudIcon.SetActive(false);
                break;
            case PowerUpHeld.LavaPool:
                projectile.GetComponent<CGDAreaDenialProjectile>().ProjectileType = CGDAreaDenialProjectile.AreaDenialProjectileType.LavaPool;
                LavaPoolIcon.SetActive(false);
                break;
        }
        
        Vector3 forwardDirection = new Vector3(_playerScript._cameraTr.forward.x, _playerScript._cameraTr.forward.y, _playerScript._cameraTr.forward.z);
        forwardDirection = forwardDirection.normalized;
        projectile.GetComponent<Rigidbody>().velocity = new Vector3(forwardDirection.x, forwardDirection.y, forwardDirection.z) * _areaDenialProjectileSpeed;
    }

    public void DisplayPowerUpIcon(PowerUpHeld powerUp)
    {
        if (_view.IsMine)
        {
            switch (powerUp)
            {
                case PowerUpHeld.SpeedBoost:
                    SpeedBoostIcon.SetActive(true);
                    break;
                case PowerUpHeld.JumpBoost:
                    JumpBoostIcon.SetActive(true);
                    break;
                case PowerUpHeld.SpeedAndJumpBoost:
                    SpeedJumpBoostIcon.SetActive(true);
                    break;
                case PowerUpHeld.Peel:
                    PeelIcon.SetActive(true);
                    break;
                case PowerUpHeld.Spikes:
                    SpikesIcon.SetActive(true);
                    break;
                case PowerUpHeld.PoisonCloud:
                    PoisonCloudIcon.SetActive(true);
                    break;
                case PowerUpHeld.LavaPool:
                    LavaPoolIcon.SetActive(true);
                    break;
            }
        }
    }

    IEnumerator DisplayFlashingIcon(GameObject icon, float powerUpDuration)
    {
        float flashingTimer = 0.0f;
        while (flashingTimer < powerUpDuration)
        {
            if (((int)Mathf.Floor(flashingTimer / _iconFlashRate)) % 2 == 0)
            {
                icon.SetActive(false);
            }
            else
            {
                icon.SetActive(true);
            }
            flashingTimer += Time.deltaTime;
            yield return null;
        }
        icon.SetActive(false);
        yield return null;
    }

    public enum PowerUpHeld
    {
        SpeedBoost,
        JumpBoost,
        SpeedAndJumpBoost,
        Peel,
        Spikes,
        PoisonCloud,
        LavaPool,
        None
    }
}
