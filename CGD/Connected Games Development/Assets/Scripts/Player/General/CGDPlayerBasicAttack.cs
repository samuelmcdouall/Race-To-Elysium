using Photon.Pun;
using UnityEngine;

public class CGDPlayerBasicAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    [SerializeField]
    float _repelBubbleDuration;
    float _repelBubbleDurationTimer;
    [SerializeField]
    float _repelForce;
    [SerializeField]
    float _repelCooldown;
    float _repelCooldownTimer;
    [SerializeField]
    float _ultChargeGain;
    bool _readyToRepel;
    Collider _repelCollider;

    [SerializeField]
    float _attackAnimationDelay;

    void Start()
    {
        _repelCollider = GetComponent<Collider>();
        _repelCollider.enabled = false;
        _repelBubbleDurationTimer = 0.0f;
        _repelCooldownTimer = 0.0f;
        _readyToRepel = true;
    }

    void Update()
    {
        if (_repelCollider.enabled)
        {
            RepelColliderEnabled();
        }
        else
        {
            RepelColliderDisabled();
        }
        if (Input.GetMouseButtonDown(0) 
            && _readyToRepel 
            && OwnPlayer.GetComponent<CGDPlayer>()._enabledControls
            && OwnPlayer.GetComponent<CGDPlayer>().GroundCheck.IsGrounded
            && !CGDGameOverScreenManager.GameOver
            && !CGDPauseManager.Paused
        )
        {
            print("Basic attack pressed");
            PeformBasicAttack();
        }
    }

    void RepelColliderEnabled()
    {
        if (_repelBubbleDurationTimer > _repelBubbleDuration)
        {
            _repelBubbleDurationTimer = 0.0f;
            _repelCollider.enabled = false;
        }
        else
        {
            _repelBubbleDurationTimer += Time.deltaTime;
        }
    }

    void RepelColliderDisabled()
    {
        if (!_readyToRepel)
        {
            if (_repelCooldownTimer > _repelCooldown)
            {
                _repelCooldownTimer = 0.0f;
                _readyToRepel = true;
            }
            else
            {
                _repelCooldownTimer += Time.deltaTime;
            }
        }
    }

    void PeformBasicAttack()
    {
        int randSoundEffect = Random.Range(0, 2);
        if (randSoundEffect == 0)
        {
            OwnPlayer.GetComponent<CGDPlayer>().PlaySoundClipForEveryone(OwnPlayer.transform.position.x, 
                                                                         OwnPlayer.transform.position.y, 
                                                                         OwnPlayer.transform.position.z,
                                                                         "AttackSFX1", 
                                                                         true);
        }
        else
        {
            OwnPlayer.GetComponent<CGDPlayer>().PlaySoundClipForEveryone(OwnPlayer.transform.position.x,
                                                             OwnPlayer.transform.position.y,
                                                             OwnPlayer.transform.position.z,
                                                             "AttackSFX2",
                                                             true);
        }
        if (OwnPlayer.GetComponent<CGDPlayer>().GroundCheck.IsGrounded && !OwnPlayer.GetComponent<CGDPlayer>()._ignoreStateChange)
        {
            OwnPlayer.GetComponent<CGDPlayer>().SwitchAnimationStateTo(OwnPlayer.GetComponent<CGDPlayer>()._basicAttackState, true);
            OwnPlayer.GetComponent<CGDPlayer>()._ignoreStateChange = true;
            Invoke("AttackComplete", _attackAnimationDelay);

        }
        _repelCollider.enabled = true;
        _readyToRepel = false;
    }

    void AttackComplete()
    {
        OwnPlayer.GetComponent<CGDPlayer>()._ignoreStateChange = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            print("Hit other player");
            OwnPlayer.GetComponent<CGDPlayer>().PlaySoundClipForEveryone(OwnPlayer.transform.position.x,
                                                                         OwnPlayer.transform.position.y,
                                                                         OwnPlayer.transform.position.z,
                                                                         "AttackHitPlayer",
                                                                         true);
            Vector3 forceToAdd = DetermineForceVectorToApply(collider);
            ApplyForceToPlayer(collider, forceToAdd);
        }
        else if (collider.tag == "Gate")
        {
            print("Hit gate");
            OwnPlayer.GetComponent<CGDPlayer>().PlaySoundClipForEveryone(OwnPlayer.transform.position.x,
                                                                         OwnPlayer.transform.position.y,
                                                                         OwnPlayer.transform.position.z,
                                                                         "AttackHitGate",
                                                                         true);
            collider.gameObject.GetComponent<CGDGate>().ReduceHealthOfGateForAllPlayers();
        }
    }

    Vector3 DetermineForceVectorToApply(Collider collider)
    {
        Vector3 playerToEnemyDirection = Vector3.Normalize(collider.gameObject.transform.position - OwnPlayer.transform.position);
        Vector3 forceToAdd = playerToEnemyDirection * _repelForce;
        return forceToAdd;
    }

    void ApplyForceToPlayer(Collider collider, Vector3 forceToAdd)
    {
        int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
        OwnPlayer.GetComponent<CGDPlayer>().SendKnockbackCommandToOtherPlayers(forceToAdd, photonViewID);
        OwnPlayer.GetComponent<CGDPlayer>().ModifyUltimateCharge(_ultChargeGain);
    }
}
