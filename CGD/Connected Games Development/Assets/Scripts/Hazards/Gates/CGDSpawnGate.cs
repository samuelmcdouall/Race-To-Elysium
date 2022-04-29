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

    void Start()
    {
        Moving = false;
    }

    void Update()
    {
        if (Moving)
        {
            transform.position += new Vector3(0.0f, -_speed * Time.deltaTime, 0.0f);
            if (Vector3.Distance(transform.position, EndPosition.position) <= _positionThreshold)
            {
                Moving = false;
            }
        }
    }
}
