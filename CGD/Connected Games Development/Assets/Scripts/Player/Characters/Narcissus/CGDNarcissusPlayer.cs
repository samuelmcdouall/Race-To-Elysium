using UnityEngine;
using Photon.Pun;

public class CGDNarcissusPlayer : CGDPlayer
{
    [Header("Ultimate Attack")]
    public GameObject UltimateCollider;

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
            SwitchAnimationStateTo(_narcissusUltimateAttackState, true);
            _ignoreStateChange = true;
            Invoke("UltimateAttackComplete", UltAttackAnimationDelay);
            PlaySoundClipForEveryone(transform.position.x, transform.position.y, transform.position.z, "NarcissusUltSFX", true);
            PlayFXForEveryone(transform.position.x, transform.position.y, transform.position.z, "NarcissusUltFX", true);
            //PhotonNetwork.Instantiate(NarcissusUltFX.name, transform.position, Quaternion.identity); //todo may not need to put fx into resources
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

    void UltimateAttackComplete()
    {
        _ignoreStateChange = false;
    }
}
