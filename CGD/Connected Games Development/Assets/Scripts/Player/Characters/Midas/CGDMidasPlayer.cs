using UnityEngine;
using Photon.Pun;

public class CGDMidasPlayer : CGDPlayer
{
    [Header("Ultimate Attack")]
    public GameObject UltimateCollider;
    public GameObject SlowFX;

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
            print("Midas Ultimate Attack!");
            UltimateCharge = 0.0f;
            UltimateBar.SetBar(UltimateCharge);
            SwitchAnimationStateTo(_midasUltimateAttackState, true);
            _ignoreStateChange = true;
            Invoke("UltimateAttackComplete", UltAttackAnimationDelay);
            PlaySoundClipForEveryone(transform.position.x, transform.position.y, transform.position.z, "MidasUltSFX", true);
            PhotonNetwork.Instantiate(SlowFX.name, transform.position, Quaternion.identity);
            UltimateCollider.GetComponent<CGDMidasUltimateAttack>().ActivateUltimateCollider();
        }
        else
        {
            print("Not enough charge!");
        }
    }

    public override void InitialPlayerSetup()
    {
        base.InitialPlayerSetup();
        ThisCharacter = Character.Midas;
    }

    void UltimateAttackComplete()
    {
        _ignoreStateChange = false;
    }
}
