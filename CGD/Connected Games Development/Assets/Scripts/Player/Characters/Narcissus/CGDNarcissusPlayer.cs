using UnityEngine;
using Photon.Pun;

public class CGDNarcissusPlayer : CGDPlayer
{
    [Header("Ultimate Attack")]
    public GameObject UltimateCollider;
    public GameObject BlindFX;

    void Awake()
    {
        InitialPlayerSetup();
    }

    public override void Update()
    {
        base.Update();
        if (View.IsMine)
        {
            if (_enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    UltimateAttack();
                }
            }
        }
    }

    public override void UltimateAttack()
    {
        if (UltimateCharge == 100.0f && _enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            print("Narcissus Ultimate Attack!");
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
        base.InitialPlayerSetup();
        ThisCharacter = Character.Narcissus;
    }
}