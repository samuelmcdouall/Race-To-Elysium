using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDSpawnGate : MonoBehaviour
{
    [SerializeField]
    float _speed;
    Rigidbody _rigidbody;
    public Transform EndPosition;
    [SerializeField]
    float _positionThreshold;
    [System.NonSerialized]
    public bool Moving;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            _rigidbody.velocity = new Vector3(0.0f, _speed, 0.0f);
            if (Vector3.Distance(transform.position, EndPosition.position) <= _positionThreshold)
            {
                Moving = false;
            }
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
