using UnityEngine;

public class CGDSpawnGate : MonoBehaviour
{
    [SerializeField]
    float _speed;
    public Transform EndPosition;
    [SerializeField]
    float _positionThreshold;
    [System.NonSerialized]
    public bool Moving;
    Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Moving = false;
    }

    void FixedUpdate()
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
