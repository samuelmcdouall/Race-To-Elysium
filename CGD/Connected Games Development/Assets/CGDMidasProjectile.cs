using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDMidasProjectile : MonoBehaviour
{
    Rigidbody _projectileRb;
    [System.NonSerialized]
    public GameObject _player;
    [SerializeField]
    float _projectileSpeed;
    Vector3 _projectileDirection;
    [SerializeField]
    float _slowDuration;
    [SerializeField]
    float _slowPercentage;
    // Start is called before the first frame update
    void Start()
    {
        _projectileRb = GetComponent<Rigidbody>();
        _projectileDirection = Vector3.Normalize(transform.position - _player.transform.position);
        _projectileDirection = _projectileDirection * _projectileSpeed;
        _projectileRb.velocity = new Vector3(_projectileDirection.x, _projectileDirection.y, _projectileDirection.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // may need to use different scripts depending on which type of player it is
        if (other.gameObject.tag == "Player")
        {
            // knockback like medusa basic attack
            //other.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(_slowPercentage, _slowDuration);
        }
        print("hit " + other.gameObject.tag);
        Destroy(gameObject);
    }
}
